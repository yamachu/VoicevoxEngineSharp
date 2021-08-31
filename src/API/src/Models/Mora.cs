using System.Text.Json.Serialization;

internal record Mora
{
    [JsonPropertyName("text")]
    public string Text { get; init; }

#nullable enable
    [JsonPropertyName("consonant")]
    public string? Consonant { get; init; }

#nullable disable
    [JsonPropertyName("vowel")]
    public string Vowel { get; init; }

    [JsonPropertyName("pitch")]

    public float Pitch { get; init; } = 0;

    public static Mora FronDomain(VoicevoxEngineSharp.Core.Acoustic.Models.Mora domainMora)
        => new Mora { Consonant = domainMora.Consonant, Text = domainMora.Text, Pitch = domainMora.Pitch, Vowel = domainMora.Vowel };

    public static VoicevoxEngineSharp.Core.Acoustic.Models.Mora ToDomain(Mora mora)
        => new VoicevoxEngineSharp.Core.Acoustic.Models.Mora
        {
            Consonant = mora.Consonant,
            Pitch = mora.Pitch,
            Text = mora.Text,
            Vowel = mora.Vowel
        };
}
