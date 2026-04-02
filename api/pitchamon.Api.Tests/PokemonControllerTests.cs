using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pitchamon.Api.Controllers;
using pitchamon.Api.Data;
using pitchamon.Api.Models;

namespace pitchamon.Api.Tests;

public class PokemonControllerTests
{
    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        var context = new AppDbContext(options);
        
        context.Pokemon.AddRange(
            new Pokemon { Id = 1, Name = "Bulbasaur", Cry = "001.wav" },
            new Pokemon { Id = 2, Name = "Charmander", Cry = "004.wav" },
            new Pokemon { Id = 3, Name = "Squirtle", Cry = "007.wav" }
        );

        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task GetAllPokemon_ReturnsExpectedResult()
    {
       var context = CreateDbContext();
       var controller = new PokemonController(context);
       
       var result = await controller.GetAllPokemon();

       var expectedResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
       expectedResult.StatusCode.Should().Be(200);

       var value = expectedResult.Value;
       value.Should().NotBeNull();
    }
}
