using System.Collections.Generic;

namespace VoicevoxEngineSharp.Core.Acoustic.Models
{
    public record AudioQuery
    {
        public IEnumerable<AccentPhrase> AccentPhrases { get; init; }

        public float SpeedScale { get; init; }

        public float PitchScale { get; init; }

        public float IntonationScale { get; init; }
    }

}
