using NumSharp;
using NumSharp.Generic;
using VoicevoxEngineSharp.Core.Acoustic.Models;
using VoicevoxEngineSharp.Core.Acoustic.Native;

namespace VoicevoxEngineSharp.Core.Acoustic.Usecases
{
    public class SynthesisEngineBuilder
    {
        public static SynthesisEngine Initialize(string yukarin_s_forwarder_path,
            string yukarin_sa_forwarder_path,
            string decode_forwarder_path,
            bool use_gpu)
        {
            if (!EachCppForwarder.Initialize(yukarin_s_forwarder_path, yukarin_sa_forwarder_path, decode_forwarder_path, use_gpu))
            {
                throw new ArgumentException("Failed to initialize EachCppForwarder, verify passed arguments");
            }
            return new SynthesisEngine();
        }
    }

    public class SynthesisEngine
    {
        internal SynthesisEngine() { }

        internal IEnumerable<AccentPhrase> ExtractPhonemeF0(IEnumerable<AccentPhrase> accentPhrases, int speakerId)
        {
            // NOTE: こいつに対して破壊的操作をするっぽい
            var flattenMoras = accentPhrases.ToFlattenMoras();

            var phonemeStrList = Enumerable.Concat(
                new[] { "pau" },
                flattenMoras.SelectMany(mora => mora.Consonant == null ? new[] { mora.Vowel } : new[] { mora.Consonant, mora.Vowel }))
                .Concat(new[] { "pau" });

            var startAccentList = np.concatenate(new[]{
                new NDArray<int>(new [] { 0 }, new Shape(1)),
                np.concatenate(accentPhrases.Select(accentPhrase =>
                {
                    var tmp = np.eye(accentPhrase.Moras.Count(), dtype: typeof(int))[accentPhrase.Accent == 1 ? 0 : 1].ToArray<int>();
                    var a = new NDArray<int>(tmp, new Shape(tmp.Length));

                    var b = accentPhrase.PauseMora == null ? a : new NDArray<int>(Enumerable.Concat(new[] { 0 }, a.ToArray<int>()).ToArray(), new Shape(1 + a.Shape[0]));

                    return RepeatWithMora(b, accentPhrase);
                }).ToArray()),
                new NDArray<int>(new [] { 0 }, new Shape(1)),
            });

            var endAccentList = np.concatenate(new[]{
                new NDArray<int>(new [] { 0 }, new Shape(1)),
                np.concatenate(accentPhrases.Select(accentPhrase =>
                {
                    var tmp = np.eye(accentPhrase.Moras.Count(), dtype: typeof(int))[accentPhrase.Accent - 1].ToArray<int>();
                    var a = new NDArray<int>(tmp, new Shape(tmp.Length));

                    var b = accentPhrase.PauseMora == null ? a : new NDArray<int>(Enumerable.Concat(new[] { 0 }, a.ToArray<int>()).ToArray(), new Shape(1 + a.Shape[0]));

                    return RepeatWithMora(b, accentPhrase);
                }).ToArray()),
                new NDArray<int>(new [] { 0 }, new Shape(1)),
            });

            var startAccentPhraseList = np.concatenate(new[]{
                new NDArray<int>(new [] { 0 }, new Shape(1)),
                np.concatenate(accentPhrases.Select(accentPhrase =>
                {
                    var tmp =np.eye(accentPhrase.Moras.Count(), dtype: typeof(int))[0].ToArray<int>();
                    var a =  new NDArray<int>(tmp, new Shape(tmp.Length));

                    var b = accentPhrase.PauseMora == null ? a : new NDArray<int>(Enumerable.Concat(new[] { 0 }, a.ToArray<int>()).ToArray(), new Shape(1 + a.Shape[0]));

                    return RepeatWithMora(b, accentPhrase);
                }).ToArray()),
                new NDArray<int>(new [] { 0 }, new Shape(1)),
            });

            var endAccentPhraseList = np.concatenate(new[]{
                new NDArray<int>(new [] { 0 }, new Shape(1)),
                np.concatenate(accentPhrases.Select(accentPhrase =>
                {
                    var tmp = np.eye(accentPhrase.Moras.Count(), dtype: typeof(int))[accentPhrase.Moras.Count() - 1].ToArray<int>();
                    var a =  new NDArray<int>(tmp, new Shape(tmp.Length));

                    var b = accentPhrase.PauseMora == null ? a : new NDArray<int>(Enumerable.Concat(new[] { 0 }, a.ToArray<int>()).ToArray(), new Shape(1 + a.Shape[0]));

                    return RepeatWithMora(b, accentPhrase);
                }).ToArray()),
                new NDArray<int>(new [] { 0 }, new Shape(1)),
            });

            var phonemeDataList = phonemeStrList.ToOjtPhonemeDataList();

            var (consonantPhonemeDataList, vowelPhonemeDataList, vowelIndexedData) = phonemeDataList.SplitMora();

            var vowelIndexes = np.array(vowelIndexedData, dtype: typeof(long));
            var vowelPhonemeList = np.array(vowelPhonemeDataList.Select(p => p.PhonemeId).ToArray(), dtype: typeof(long));
            var consonantPhonemeList = np.array(consonantPhonemeDataList.Select(p => p == null ? -1 : p.PhonemeId).ToArray(), dtype: typeof(long));

            var f0List = EachCppForwarder.YukarinSaForward(
                vowelPhonemeList.shape[0],
                vowelPhonemeList[np.newaxis].ToArray<long>(),
                consonantPhonemeList[np.newaxis].ToArray<long>(),
                startAccentList[vowelIndexes][np.newaxis].ToArray<int>().Select(v => (long)v).ToArray(),
                endAccentList[vowelIndexes][np.newaxis].ToArray<int>().Select(v => (long)v).ToArray(),
                startAccentPhraseList[vowelIndexes][np.newaxis].ToArray<int>().Select(v => (long)v).ToArray(),
                endAccentPhraseList[vowelIndexes][np.newaxis].ToArray<int>().Select(v => (long)v).ToArray(),
                new long[] { speakerId }
            );

            foreach (var (p, i) in vowelPhonemeDataList.Select((p, i) => (p, i)))
            {
                if (PhonemeList.UnvoicedMoraPhonemes.Contains(p.phoneme))
                {
                    f0List[i] = 0;
                }
            }

            foreach (var (mora, i) in flattenMoras.Select((p, i) => (p, i)))
            {
                mora.Pitch = f0List[i + 1];
            }

            return accentPhrases;
        }

        // [N] な配列
        private NDArray<T> RepeatWithMora<T>(NDArray<T> array, AccentPhrase accentPhrase) where T : unmanaged
        {
            var tmp = accentPhrase.PauseMora == null ? accentPhrase.Moras : Enumerable.Concat(new[] { accentPhrase.PauseMora }, accentPhrase.Moras);
            var eachIndexRepeatCount = tmp.Select(m => m.Consonant == null ? 1 : 2);

            return new NDArray<T>(eachIndexRepeatCount.SelectMany((v, i) => np.repeat(array.ToArray<T>()[i], v).ToArray<T>()).ToArray<T>());
        }
    }
}
