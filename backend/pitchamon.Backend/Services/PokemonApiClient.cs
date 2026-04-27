namespace pitchamon.Backend.Services;

public class PokemonApiClient
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration configuration;

    public PokemonApiClient(
        HttpClient client,
        IConfiguration config)
    {
        httpClient = client;
        configuration = config;
    }

    public async Task<byte[]> GetCry(string pokemonName)
    {
        var baseUrl = configuration["PokemonApi:BaseUrl"] ?? "http://localhost:8080";
        var response = await httpClient.GetAsync($"{baseUrl}/pokemon/{pokemonName}/cry");
        
        if(!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get cry for {pokemonName}: {response.ReasonPhrase}");
        }

        return await response.Content.ReadAsByteArrayAsync();
    }
}