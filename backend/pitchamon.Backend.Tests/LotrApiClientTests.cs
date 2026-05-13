using FluentAssertions;
using Microsoft.Extensions.Configuration;
using pitchamon.Backend.Services;
using System.Net;
using System.Text;

namespace pitchamon.Backend.Tests;

public class LotrApiClientTests
{
    [Fact]
    public async Task GetClasses_ReturnsClasses_WhenApiCallSucceeds()
    {
        var handler = new FakeHttpMessageHandler(_ => new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = CreateJsonContent("""
                                        [
                                          {
                                            "id": 1,
                                            "name": "Warrior",
                                            "desc": "A fierce fighter trained for front-line combat.",
                                            "racialids": [1, 2, 3, 4]
                                          },
                                          {
                                            "id": 2,
                                            "name": "Istari",
                                            "desc": "A member of the order of wizards.",
                                            "racialids": [2, 3]
                                          }
                                        ]
                                        """)
        });
        
        var client = CreateClient(handler);
        
        var classes = await client.GetClasses();
        
        classes.Should().HaveCount(2);
        classes[0].Id.Should().Be(1);
        classes[0].Name.Should().Be("Warrior");
        classes[0].Desc.Should().Contain("fighter");
        classes[0].RacialIds.Should().BeEquivalentTo(new[] { 1, 2, 3, 4 });
        handler.LastRequest!.Method.Should().Be(HttpMethod.Get);
        handler.LastRequest.RequestUri!.ToString().Should().Be("http://localhost:8090/classes");
    }

    [Fact]
    public async Task GetClasses_Throws_WhenApiCallFails()
    {
        var handler = new FakeHttpMessageHandler(_ => new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.InternalServerError,
            ReasonPhrase = "Server Error"
        });

        var client = CreateClient(handler);

        Func<Task> act = async () => await client.GetClasses();

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("*Failed to get LOTR classes*");
    }

    [Fact]
    public async Task SelectClass_ReturnsExpectedClass_BasedOnSongLengthAndPokemonId()
    {
        var handler = new FakeHttpMessageHandler(_ => new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = CreateJsonContent("""
                                        [
                                          { "id": 1, "name": "Warrior", "desc": "A fierce fighter trained for front-line combat.", "racialids": [1, 2, 3, 4] },
                                          { "id": 2, "name": "Istari", "desc": "A member of the order of wizards.", "racialids": [2, 3] },
                                          { "id": 3, "name": "Burglar", "desc": "Stealth and cleverness for perilous journeys.", "racialids": [1, 2, 3] },
                                          { "id": 4, "name": "Creature", "desc": "Twisted and sustained by unnatural will.", "racialids": [1] }
                                        ]
                                        """)
        });

        var client = CreateClient(handler);

        var selectedClass = await client.SelectClass(songLengthBytes: 100, pokemonId: 27);

        selectedClass.Id.Should().Be(4);
        selectedClass.Name.Should().Be("Creature");
    }

    [Fact]
    public async Task SelectClass_Throws_WhenNoClassesAreReturned()
    {
        var handler = new FakeHttpMessageHandler(_ => new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = CreateJsonContent("[]")
        });
        
        var client = CreateClient(handler);
        
        Func<Task> act = async () => await client.SelectClass(songLengthBytes: 100, pokemonId: 27);
        
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("No LOTR classes available");
    }
    
    private static LotrApiClient CreateClient(FakeHttpMessageHandler handler, string bearerToken = "")
    {
        var httpClient = new HttpClient(handler);

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["LotrApi:BaseUrl"] = "http://localhost:8090",
                ["LotrApi:BearerToken"] = bearerToken
            })
            .Build();

        return new LotrApiClient(httpClient, config);
    }
    
    private static StringContent CreateJsonContent(string json)
    {
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> responseFactory;

        public FakeHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responseFactory)
        {
            this.responseFactory = responseFactory;
        }
        
        public HttpRequestMessage? LastRequest { get; private set; }
        
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            return Task.FromResult(responseFactory(request));
        }
    }
}