namespace VoicevoxEngineSharp.Core.Acoustic.Models
{
    public record Mora
    {
        public string Text { get; init; }

#nullable enable
        public string? Consonant { get; init; }

        public float? ConsonantLength { get; set; }
#nullable disable

        public string Vowel { get; init; }

        public float VowelLength { get; set; }

        public float Pitch { get; set; } = 0;
    }

}
