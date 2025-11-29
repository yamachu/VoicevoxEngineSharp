using System.Collections.Generic;
using System.Text.Json.Serialization;

internal record AudioQuery
{
    [JsonPropertyName("accent_phrases")]
    public IEnumerable<AccentPhrase> AccentPhrases { get; init; }

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
}
