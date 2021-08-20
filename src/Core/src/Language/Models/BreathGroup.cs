using System.Collections.Generic;
using System.Linq;

namespace VoicevoxEngineSharp.Core.Language.Models
{
    public class BreathGroup
    {
        private IEnumerable<AccentPhrase> _accentPhrases { get; init; }

        public static BreathGroup FromPhonemes(IEnumerable<Phoneme> phonemes)
        {
            var accentPhrases = new List<AccentPhrase>();
            var accentPhonemes = new List<Phoneme>();

            foreach (var (phoneme, nextPhoneme) in Enumerable.Zip(phonemes, Enumerable.Concat(phonemes.Skip(1), new Phoneme?[] { null })))
            {
                accentPhonemes.Add(phoneme);

                if (nextPhoneme == null
                    || phoneme.Context("i3") != nextPhoneme.Context("i3")
                    || phoneme.Context("f5") != nextPhoneme.Context("f5"))
                {
                    accentPhrases.Add(AccentPhrase.FromPhonemes(accentPhonemes));
                    accentPhonemes.Clear();
                }
            }

            return new BreathGroup(accentPhrases);
        }

        private BreathGroup(IEnumerable<AccentPhrase> accentPhrases) { _accentPhrases = accentPhrases; }

        public void SetContext(string key, string value)
        {
            foreach (var accentPhrase in _accentPhrases)
            {
                accentPhrase.SetContext(key, value);
            }
        }

        public IEnumerable<Phoneme> Phonemes => _accentPhrases.SelectMany(ap => ap.Phonemes);

        public IEnumerable<string> Labels => Phonemes.Select(p => p.Label);

        public IEnumerable<AccentPhrase> AccentPhrases => _accentPhrases;
    }
}
