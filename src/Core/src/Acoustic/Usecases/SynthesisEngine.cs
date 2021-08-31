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

            var ind = 0;
            var gen = accentPhrases.Select(v =>
            {
                var ac = new AccentPhrase
                {
                    Accent = v.Accent,
                    PauseMora = v.PauseMora,
                    Moras = flattenMoras.Skip(ind).Take(v.Moras.Count()).ToArray()
                };
                ind += v.Moras.Count();
                return ac;
            }).ToArray();

            return gen;
        }

        internal IEnumerable<float> Synthesis(AudioQuery audioQuery, int speakerId)
        {
            const int rate = 200;

            var flattenMoras = audioQuery.AccentPhrases.ToFlattenMoras();
            var phonemeStrList = Enumerable.Concat(
               new[] { "pau" },
               flattenMoras.SelectMany(mora => mora.Consonant == null ? new[] { mora.Vowel } : new[] { mora.Consonant, mora.Vowel }))
               .Concat(new[] { "pau" });

            var phonemeDataList = phonemeStrList.ToOjtPhonemeDataList();
            var phonemeListS = phonemeDataList.Select(v => (long)v.PhonemeId);

            var phonemeLength = EachCppForwarder.YukarinSForward(phonemeDataList.Count(), phonemeListS.ToArray(), new long[] { speakerId });
            phonemeLength[0] = audioQuery.PrePhonemeLength;
            phonemeLength[^1] = audioQuery.PostPhonemeLength;

            var ndPhonemeLength = (np.round_(new NDArray<float>(phonemeLength, new Shape(phonemeLength.Length)) * rate, typeof(float)) / rate) / audioQuery.SpeedScale;

            // pitch
            var f0List = Enumerable.Concat(new float[] { 0 }, flattenMoras.Select(v => v.Pitch)).Concat(new float[] { 0 });
            var f0 = np.array(f0List.ToArray(), typeof(float)) * Math.Pow(2.0, audioQuery.PitchScale);

            var voiced = f0 > 0f;
            var meanF0 = f0[voiced].mean();
            if (!float.IsNaN(meanF0.GetSingle()))
            {
                foreach (var (_, i) in voiced.ToArray<bool>().Select((v, i) => (v, i)).Where((v) => v.v))
                {
                    f0[i] = (f0[i] - meanF0.GetSingle()) * audioQuery.IntonationScale + meanF0.GetSingle();
                }
            }

            var (_, _, vowelIndexesData) = phonemeDataList.SplitMora();
            var vowelIndexes = np.array(vowelIndexesData);

            // forward decode
            var phonemeBinNum = np.round_(np.array(phonemeLength) * np.array(new[] { rate })).astype(NPTypeCode.Int32);

            //var phoneme = np.repeat(np.array(phonemeListS), phonemeBinNum);
            var phoneme = np.array(phonemeBinNum.ToArray<int>().SelectMany((v, i) => np.repeat(phonemeListS.ElementAt(i), v).ToArray<long>()));

            var tmp = (vowelIndexes[":-1"] + 1).ToArray<int>();

            //var repeatedF0 = np.repeat(f0, np.array(tmp.Zip(tmp.Skip(1), (p, n) => $"{p}:{n}").Select(v => phonemeBinNum[v].sum()).ToArray()));
            var repeatedF0 = np.array(tmp.Zip(tmp.Skip(1), (p, n) => $"{p}:{n}").Select(v => phonemeBinNum[v].sum().astype(NPTypeCode.Int64)).ToArray()
                .SelectMany((v, i) => np.repeat(f0[i], (int)v.GetInt64()).ToArray<float>()));

            var arr = np.zeros(new Shape(new int[] { phoneme.size, OjtPhoneme.numPhoneme }), typeof(float));
            foreach (var (outer, inner) in phoneme.ToArray<long>().Select((inner, outer) => (outer, inner)))
            {
                arr[outer, (int)inner] = 1;
            }
            //arr[np.arange(phoneme.size), phoneme] = 1;

            var sampleF0 = new SamplingData(repeatedF0, rate).Resample(24000 / 256);
            var samplePhoneme = new SamplingData(arr, rate).Resample(24000 / 256);

            var dLength = samplePhoneme.shape[0];
            var dPhonemeSize = samplePhoneme.shape[1];
            var dF0 = sampleF0[":", np.newaxis];
            var dF0Float = dF0.ToArray<float>();
            var dPhoneme = samplePhoneme.ToArray<float>();
            var dSpeakerId = new long[] { speakerId };

            var wave = EachCppForwarder.DecodeForward(
                dLength,
                dPhonemeSize,
                dF0Float,
                dPhoneme,
                dSpeakerId
            );

            if (audioQuery.VolumeScale != 1f)
            {
                return wave.Select(v => v * audioQuery.VolumeScale);
            }

            return wave;
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
