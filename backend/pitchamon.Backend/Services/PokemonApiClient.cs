using System.Net.Http.Headers;
using System.Net.Http.Json;
using pitchamon.Backend.Models;

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
    
    public async Task<PokemonDetails> GetPokemonDetails(string pokemonName)
    {
       var baseUrl = configuration["PokemonApi:BaseUrl"] ?? "http://localhost:8080";
       var bearerToken = configuration["PokemonApi:BearerToken"];
         
       if (string.IsNullOrEmpty(bearerToken))
       {
           throw new Exception("Bearer token for Pokemon API is not configured.");
       }
       
       using var request = new HttpRequestMessage(
           HttpMethod.Get,
           $"{baseUrl}/pokemon/{pokemonName}"
       );
       
       request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
       
       var response = await httpClient.SendAsync(request);
       
       if (!response.IsSuccessStatusCode)
       {
           throw new Exception($"Failed to get details for {pokemonName}: {response.ReasonPhrase}");
       }
       
       var pokemon = await response.Content.ReadFromJsonAsync<PokemonDetails>();
       
       if (pokemon == null) 
       {
           throw new Exception($"Pokemon details for {pokemonName} could not be deserialized.");
       }
         
       return pokemon;
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