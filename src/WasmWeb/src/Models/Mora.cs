#nullable enable
using System.Text.Json.Serialization;

namespace VoicevoxEngineSharp.WasmWeb.Models
{
    internal record Mora
    {
        [JsonPropertyName("text")]
        public string Text { get; init; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("consonant")]
        public string? Consonant { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("consonant_length")]
        public float? ConsonantLength { get; init; }

        [JsonPropertyName("vowel")]
        public string Vowel { get; init; } = string.Empty;

        [JsonPropertyName("vowel_length")]
        public float VowelLength { get; init; }

        [JsonPropertyName("pitch")]
        public float Pitch { get; init; } = 0;

        public static Mora FromDomain(VoicevoxEngineSharp.Core.Acoustic.Models.Mora domainMora)
            => new Mora
            {
                Text = domainMora.Text,
                Consonant = domainMora.Consonant,
                ConsonantLength = domainMora.ConsonantLength,
                Vowel = domainMora.Vowel,
                VowelLength = domainMora.VowelLength,
                Pitch = domainMora.Pitch
            };

        public static VoicevoxEngineSharp.Core.Acoustic.Models.Mora ToDomain(Mora mora)
            => new VoicevoxEngineSharp.Core.Acoustic.Models.Mora
            {
                Text = mora.Text,
                Consonant = mora.Consonant,
                ConsonantLength = mora.ConsonantLength,
                Vowel = mora.Vowel,
                VowelLength = mora.VowelLength,
                Pitch = mora.Pitch
            };
    }
}
