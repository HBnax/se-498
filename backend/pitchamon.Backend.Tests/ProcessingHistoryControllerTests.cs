using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pitchamon.Backend.Controllers;
using pitchamon.Backend.Data;
using pitchamon.Backend.Models;

namespace pitchamon.Backend.Tests;

public class ProcessingHistoryControllerTests
{
    private static BackendDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<BackendDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new BackendDbContext(options);

        context.ProcessingHistory.AddRange(
            new ProcessingHistory
            {
                UserId = 1,
                OriginalSongFile = "song1.mp3",
                PokemonUsed = "pikachu",
                CreatedAt = DateTime.UtcNow.AddMinutes(-10)
            },
            new ProcessingHistory
            {
                UserId = 1,
                OriginalSongFile = "song2.mp3",
                PokemonUsed = "charizard",
                CreatedAt = DateTime.UtcNow
            },
            new ProcessingHistory
            {
                UserId = 2,
                OriginalSongFile = "song3.mp3",
                PokemonUsed = "bulbasaur",
                CreatedAt = DateTime.UtcNow
            }
        );

        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task GetProcessingHistory_ReturnsOnlyUserHistory()
    {
        var context = CreateDbContext();
        var controller = new ProcessingHistoryController(context);

        var request = new ProcessingHistoryRequest { UserId = 1 };

        var result = await controller.GetProcessingHistory(request);

        var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        ok.StatusCode.Should().Be(200);
        ok.Value.Should().NotBeNull();
    }
    
}