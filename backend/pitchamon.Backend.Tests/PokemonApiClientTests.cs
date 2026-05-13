using FluentAssertions;
using Microsoft.Extensions.Configuration;
using pitchamon.Backend.Services;
using System.Net;
using System.Text;

namespace pitchamon.Backend.Tests;

public class PokemonApiClientTests
{
    [Fact]
    public async Task GetPokemonDetails_ReturnsPokemonDetails_WhenApiCallSucceeds()
    {
        var handler = new FakeHttpMessageHandler(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("""
                                        {
                                          "id": 25,
                                          "name": "pikachu",
                                          "cry": "pikachu.wav"
                                        }
                                        """, Encoding.UTF8, "application/json")
        });

        var httpClient = new HttpClient(handler);

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["PokemonApi:BaseUrl"] = "http://localhost:8080",
                ["PokemonApi:BearerToken"] = "pitchamon"
            })
            .Build();

        var client = new PokemonApiClient(httpClient, config);

        var pokemon = await client.GetPokemonDetails("pikachu");

        pokemon.Id.Should().Be(25);
        pokemon.Name.Should().Be("pikachu");
        pokemon.Cry.Should().Be("pikachu.wav");
    }

    [Fact]
    public async Task GetPokemonDetails_Throws_WhenTokenMissing()
    {
        var handler = new FakeHttpMessageHandler(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK
        });

        var httpClient = new HttpClient(handler);

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["PokemonApi:BaseUrl"] = "http://localhost:8080",
                ["PokemonApi:BearerToken"] = ""
            })
            .Build();

        var client = new PokemonApiClient(httpClient, config);
        
        Func<Task> act = async () => await client.GetPokemonDetails("pikachu");
        
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("*Bearer token for Pokemon API is not configured*");
    }

    [Fact]
    public async Task GetPokemonDetails_Throws_WhenApiCallFails()
    {
        var handler = new FakeHttpMessageHandler(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound,
            ReasonPhrase = "Not Found"
        });

        var httpClient = new HttpClient(handler);

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["PokemonApi:BaseUrl"] = "http://localhost:8080",
                ["PokemonApi:BearerToken"] = "pitchamon"
            })
            .Build();
        
        var client = new PokemonApiClient(httpClient, config);
        
        Func<Task> act = async () => await client.GetPokemonDetails("unown");
        
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("*Failed to get details for unown: Not Found*");
    }
    
    [Fact]
    public async Task GetCry_ReturnsBytes_WhenApiCallSucceeds()
    {
        var handler = new FakeHttpMessageHandler(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new ByteArrayContent(Encoding.UTF8.GetBytes("fake-audio"))
        });

        var httpClient = new HttpClient(handler);

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["PokemonApi:BaseUrl"] = "http://localhost:8080",
                ["PokemonApi:BearerToken"] = "pitchamon"
            })
            .Build();

        var client = new PokemonApiClient(httpClient, config);

        var bytes = await client.GetCry("pikachu");

        bytes.Should().NotBeNull();
        bytes.Length.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetCry_Throws_WhenTokenMissing()
    {
        var handler = new FakeHttpMessageHandler(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK
        });

        var httpClient = new HttpClient(handler);

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["PokemonApi:BaseUrl"] = "http://localhost:8080",
                ["PokemonApi:BearerToken"] = ""
            })
            .Build();

        var client = new PokemonApiClient(httpClient, config);

        Func<Task> act = async () => await client.GetCry("pikachu");

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("*Bearer token*");
    }

    private class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;

        public FakeHttpMessageHandler(HttpResponseMessage response)
        {
            _response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_response);
        }
    }
}