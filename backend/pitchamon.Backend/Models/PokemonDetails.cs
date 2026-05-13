using System.Text.Json.Serialization;

namespace pitchamon.Backend.Models;

public class PokemonDetails
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("cry")]
    public string Cry { get; set; } = string.Empty;
}