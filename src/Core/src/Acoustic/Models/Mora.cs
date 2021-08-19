namespace VoicevoxEngineSharp.Core.Acoustic.Models
{
    public record Mora
    {
        public string Text { get; init; }

#nullable enable
        public string? Consonant { get; init; }

#nullable disable
        public string Vowel { get; init; }

        public float Pitch { get; init; } = 0;
    }

}
