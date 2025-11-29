using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<AccentPhrase> ReplaceMoraData(IEnumerable<AccentPhrase> accentPhrases, int speakerId)
        {
            return _synthesisEngine.ReplaceMoraPitch(
                _synthesisEngine.ReplacePhonemeLength(accentPhrases, speakerId),
                speakerId
            );
        }

        public IEnumerable<AccentPhrase> ReplaceMoraLength(IEnumerable<AccentPhrase> accentPhrases, int speakerId)
        {
            return _synthesisEngine.ReplacePhonemeLength(accentPhrases, speakerId);
        }

        public IEnumerable<AccentPhrase> ReplaceMoraPitch(IEnumerable<AccentPhrase> accentPhrases, int speakerId)
        {
            return _synthesisEngine.ReplaceMoraPitch(accentPhrases, speakerId);
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
                        Moras = accentPhrase.Moras.Select((mora) =>
                            new Mora
                            {
                                Text = mora.ToText(),
                                Consonant = mora.Consonant == null ? null : mora.Consonant.GetPhoneme(),
                                ConsonantLength = mora.Consonant == null ? null : 0,
                                Vowel = mora.Vowel.GetPhoneme(),
                                VowelLength = 0,
                                Pitch = 0
                            }),
                        Accent = accentPhrase.Accent,
                        PauseMora = (iAccentPhrase == breathGroup.AccentPhrases.Count() - 1 && iBreathGroup != utterance.BreathGroups.Count() - 1)
                            ? new Mora
                            {
                                Text = "、",
                                Consonant = null,
                                ConsonantLength = null,
                                Vowel = "pau",
                                VowelLength = 0,
                                Pitch = 0
                            }
                            : null,
                    };
                });
            });

            return ReplaceMoraData(accentPhrases, speakerId);
        }

        public IEnumerable<float> SynthesisWave(AudioQuery audioQuery, int speakerId)
        {
            return _synthesisEngine.Synthesis(audioQuery, speakerId);
        }
    }
}
