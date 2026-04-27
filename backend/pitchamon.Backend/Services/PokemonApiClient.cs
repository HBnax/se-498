using System.Net.Http.Headers;

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
        var bearerToken = configuration["PokemonApi:BearerToken"];

        if (string.IsNullOrEmpty(bearerToken))
        {
            throw new Exception("Bearer token for Pokemon API is not configured.");
        }

        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{baseUrl}/pokemon/{pokemonName}/cry"
        );
        
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        
        var response = await httpClient.SendAsync(request);
        
        if(!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get cry for {pokemonName}: {response.ReasonPhrase}");
        }

        return await response.Content.ReadAsByteArrayAsync();
    }
}