using System.Net.Http.Headers;
using System.Net.Http.Json;
using pitchamon.Backend.Models;

namespace pitchamon.Backend.Services;

public class LotrApiClient
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration configuration;
    
    public LotrApiClient(
        HttpClient client,
        IConfiguration config)
    {
        httpClient = client;
        configuration = config;
    }

    public async Task<IReadOnlyList<LotrClass>> GetClasses(CancellationToken cancellationToken = default)
    {
        var baseUrl = configuration["LotrApi:BaseUrl"] ?? "http://localhost:8090";
        var bearerToken = configuration["LotrApi:BearerToken"];
        
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{baseUrl}/classes"
        );
        
        if (!string.IsNullOrEmpty(bearerToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        }
        
        var response = await httpClient.SendAsync(request, cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get LOTR classes: {response.ReasonPhrase}");
        }
        
        var classes = await response.Content.ReadFromJsonAsync<List<LotrClass>>(cancellationToken: cancellationToken);
        return classes ?? [];
    }
    
    public async Task<LotrClass> SelectClass(long songLengthBytes, int pokemonId, CancellationToken cancellationToken = default)
    {
        var classes = await GetClasses(cancellationToken);
        
        if (classes.Count == 0)
        {
            throw new Exception("No LOTR classes available");
        }

        var selectedIndex = (int)((songLengthBytes + pokemonId) % classes.Count);
        return classes[selectedIndex];
    }
}