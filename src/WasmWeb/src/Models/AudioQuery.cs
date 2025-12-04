#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace VoicevoxEngineSharp.WasmWeb.Models
{
    internal record AudioQuery
    {
        [JsonPropertyName("accent_phrases")]
        public IEnumerable<AccentPhrase> AccentPhrases { get; init; } = [];

        [JsonPropertyName("speedScale")]
        public float SpeedScale { get; init; }

        [JsonPropertyName("pitchScale")]
        public float PitchScale { get; init; }

        [JsonPropertyName("intonationScale")]
        public float IntonationScale { get; init; }

        [JsonPropertyName("volumeScale")]
        public float VolumeScale { get; init; }

        [JsonPropertyName("prePhonemeLength")]
        public float PrePhonemeLength { get; init; }

        [JsonPropertyName("postPhonemeLength")]
        public float PostPhonemeLength { get; init; }

        [JsonPropertyName("outputSamplingRate")]
        public int OutputSamplingRate { get; init; }

        [JsonPropertyName("pauseLength")]
        public float? PauseLength { get; init; } = null;

        [JsonPropertyName("pauseLengthScale")]
        public float PauseLengthScale { get; init; } = 1.0f;

        [JsonPropertyName("outputStereo")]
        public bool OutputStereo { get; init; }

        public static AudioQuery FromDomain(VoicevoxEngineSharp.Core.Acoustic.Models.AudioQuery domainAudioQuery)
            => new AudioQuery
            {
                AccentPhrases = domainAudioQuery.AccentPhrases.Select(ap => AccentPhrase.FromDomain(ap)),
                SpeedScale = domainAudioQuery.SpeedScale,
                PitchScale = domainAudioQuery.PitchScale,
                IntonationScale = domainAudioQuery.IntonationScale,
                VolumeScale = domainAudioQuery.VolumeScale,
                PrePhonemeLength = domainAudioQuery.PrePhonemeLength,
                PostPhonemeLength = domainAudioQuery.PostPhonemeLength,
                OutputSamplingRate = domainAudioQuery.OutputSamplingRate,
                PauseLength = domainAudioQuery.PauseLength,
                PauseLengthScale = domainAudioQuery.PauseLengthScale,
                OutputStereo = domainAudioQuery.OutputStereo,
            };

        public static VoicevoxEngineSharp.Core.Acoustic.Models.AudioQuery ToDomain(AudioQuery audioQuery)
            => new VoicevoxEngineSharp.Core.Acoustic.Models.AudioQuery
            {
                AccentPhrases = audioQuery.AccentPhrases.Select(ap => AccentPhrase.ToDomain(ap)),
                SpeedScale = audioQuery.SpeedScale,
                PitchScale = audioQuery.PitchScale,
                IntonationScale = audioQuery.IntonationScale,
                VolumeScale = audioQuery.VolumeScale,
                PrePhonemeLength = audioQuery.PrePhonemeLength,
                PostPhonemeLength = audioQuery.PostPhonemeLength,
                OutputSamplingRate = audioQuery.OutputSamplingRate,
                PauseLength = audioQuery.PauseLength,
                PauseLengthScale = audioQuery.PauseLengthScale,
                OutputStereo = audioQuery.OutputStereo,
            };
    }
}
