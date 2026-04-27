namespace pitchamon.Backend.Models;

public class ProcessRequest
{
    public IFormFile Song { get; set; } = default!;
    public string PokemonName { get; set; } = string.Empty;
}