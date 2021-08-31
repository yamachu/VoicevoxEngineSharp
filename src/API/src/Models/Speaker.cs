using System.Text.Json.Serialization;

internal record Speaker
{
    [JsonPropertyName("name")]
    public string Name { get; init; }

    [JsonPropertyName("speaker_id")]
    public int SpeakerId { get; init; }
}
