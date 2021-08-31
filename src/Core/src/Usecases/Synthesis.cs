using System;
using VoicevoxEngineSharp.Core.Acoustic.Models;
using VoicevoxEngineSharp.Core.Acoustic.Usecases;
using VoicevoxEngineSharp.Core.Language.Extensions;
using VoicevoxEngineSharp.Core.Language.Usecases;

namespace VoicevoxEngineSharp.Core.Usecases
{
    public class Synthesis
    {
        private TextToUtterance _textToUtterance;
        private SynthesisEngine _synthesisEngine;
        public Synthesis(TextToUtterance textToUtterance, SynthesisEngine synthesisEngine)
        {
            _textToUtterance = textToUtterance;
            _synthesisEngine = synthesisEngine;
        }

        public IEnumerable<AccentPhrase> ReplaceMoraPitch(IEnumerable<AccentPhrase> accentPhrases, int speakerId)
        {
            return _synthesisEngine.ExtractPhonemeF0(accentPhrases, speakerId);
        }

        public IEnumerable<AccentPhrase> CreateAccentPhrases(string text, int speakerId)
        {
            if (text.Trim() == "")
            {
                return new AccentPhrase[] { };
            }
            var utterance = _textToUtterance.ToUtterance(text);

            var accentPhrases = utterance.BreathGroups.SelectMany((breathGroup, iBreathGroup) =>
            {
                return breathGroup.AccentPhrases.Select((accentPhrase, iAccentPhrase) =>
                {
                    return new AccentPhrase
                    {
                        Accent = accentPhrase.Accent,
                        PauseMora = (iAccentPhrase == breathGroup.AccentPhrases.Count() - 1 && iBreathGroup != utterance.BreathGroups.Count() - 1) ? new Mora { Text = "、", Consonant = null, Vowel = "pau", Pitch = 0 } : null,
                        Moras = accentPhrase.Moras.Select((mora) => new Mora { Text = mora.ToText(), Consonant = mora.Consonant == null ? null : mora.Consonant.GetPhoneme(), Vowel = mora.Vowel.GetPhoneme(), Pitch = 0 })
                    };
                });
            });

            return ReplaceMoraPitch(accentPhrases, speakerId);
        }

        public IEnumerable<float> SynthesisWave(AudioQuery audioQuery, int speakerId)
        {
            return _synthesisEngine.Synthesis(audioQuery, speakerId);
        }
    }
}
