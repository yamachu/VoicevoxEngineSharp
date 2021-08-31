using System.Collections.Generic;

namespace VoicevoxEngineSharp.Core.Acoustic.Models
{
    public record AudioQuery
    {
        public IEnumerable<AccentPhrase> AccentPhrases { get; init; }

        public float SpeedScale { get; init; }

        public float PitchScale { get; init; }

        public float IntonationScale { get; init; }

        public float VolumeScale { get; init; }

        public float PrePhonemeLength { get; init; }

        public float PostPhonemeLength { get; init; }

        public int OutputSamlingRate { get; init; }

        public bool OutputStereo { get; init; }
    }

}
