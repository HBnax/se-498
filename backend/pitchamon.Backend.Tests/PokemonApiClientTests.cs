using FluentAssertions;
using Microsoft.Extensions.Configuration;
using pitchamon.Backend.Services;
using System.Net;
using System.Text;

namespace pitchamon.Backend.Tests;

public class PokemonApiClientTests
{
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