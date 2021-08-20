using System.Collections.Generic;
using System.Linq;

namespace VoicevoxEngineSharp.Core.Language.Models
{
#nullable enable
    public class Mora
    {
        private Phoneme _vowel { get; init; }

        private Phoneme? _consonant { get; init; }

        public Mora(Phoneme vowel, Phoneme? consonant)

        {
            _vowel = vowel;
            _consonant = consonant;
        }

        public IEnumerable<Phoneme> Phonemes =>
            _consonant == null
                ? new[] { _vowel }
                : new[] { _consonant, _vowel };

        public IEnumerable<string> Label => Phonemes.Select(p => p.Label);

        public void SetContext(string key, string value)
        {
            _vowel.SetContext(key, value);
            if (_consonant != null)
            {
                _consonant.SetContext(key, value);
            }
        }

        public Phoneme Vowel => _vowel;
    }
#nullable disable
}
