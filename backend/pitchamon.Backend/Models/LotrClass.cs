using System.Text.Json.Serialization;


namespace pitchamon.Backend.Models;

public class LotrClass
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("desc")]
    public string Desc { get; set; } = string.Empty;

    [JsonPropertyName("racialids")]
    public int[] RacialIds { get; set; } = [];
}