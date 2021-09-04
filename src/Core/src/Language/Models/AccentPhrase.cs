using System;
using System.Collections.Generic;
using System.Linq;
using VoicevoxEngineSharp.Core.Language.Extensions;

namespace VoicevoxEngineSharp.Core.Language.Models
{
    public class AccentPhrase
    {
        private IEnumerable<Mora> _moras { get; init; }
        private int _accent { get; init; }

        public static AccentPhrase FromPhonemes(IEnumerable<Phoneme> phonemes)
        {
            var groupedMoras = phonemes.GroupBy(s => s.Context("a2"))
                .Select(v => (v.Key, v.ToArray()))
                .SelectMany((v) =>
                {
                    if (v.Key == "49")
                    {
                        return v.Item2.Aggregate((new Mora[] { }, new Phoneme[] { }), (prev, curr) =>
                        {
                            return (curr.IsVowel(), prev.Item2.Length) switch
                            {
                                (true, 0) => (prev.Item1.Concat(new[] { new Mora(vowel: curr, consonant: null) }).ToArray(), new Phoneme[] { }),
                                (true, 1) => (prev.Item1.Concat(new[] { new Mora(vowel: curr, consonant: prev.Item2[0]) }).ToArray(), new Phoneme[] { }),
                                _ => (prev.Item1, prev.Item2.Concat(new[] { curr }).ToArray())
                            };
                        }).Item1;
                    }
                    else
                    {
                        var (consonant, vowel) = v.Item2.Length switch
                        {
                            1 => (null, v.Item2[0]),
                            2 => (v.Item2[0], v.Item2[1]),
                            _ => throw new ArgumentException()
                        };
                        return new[] { new Mora(vowel: vowel, consonant: consonant) };
                    }
                });

            var moras = groupedMoras.ToArray();
            // NOTE: 1~49のレンジで収まる数になっていないとなので、Maxの数は48で固定している
            // see: https://github.com/Hiroshiba/voicevox_engine/issues/57
            var limitedMoras = moras.Take(48).ToArray();

            return new AccentPhrase(limitedMoras, limitedMoras.Length);
        }

        private AccentPhrase(IEnumerable<Mora> moras, int accent)
        {
            _moras = moras;
            _accent = accent;
        }

        public void SetContext(string key, string value)
        {
            foreach (var mora in _moras)
            {
                mora.SetContext(key, value);
            }
        }

        public IEnumerable<Phoneme> Phonemes => _moras.SelectMany(v => v.Phonemes);

        public IEnumerable<string> Labels => Phonemes.Select(p => p.Label);

        public IEnumerable<Mora> Moras => _moras;

        public int Accent => _accent;

        public AccentPhrase Merge(AccentPhrase accentPhrase)
        {
            return new AccentPhrase(Enumerable.Concat(_moras, accentPhrase._moras), _accent);
        }
    }
}
