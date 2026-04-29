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
       expectedResult.Value.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetPokemonByName_ReturnsExpectedResult_WhenPokemonExists()
    {
        var context = CreateDbContext();
        var controller = new PokemonController(context);

        var result = await controller.GetPokemonByName("Charmander");

        var expectedResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var pokemon = expectedResult.Value.Should().BeOfType<Pokemon>().Subject;

        pokemon.Name.Should().Be("Charmander");
        pokemon.Cry.Should().Be("004.wav");
    }

    [Fact]
    public async Task GetPokemonByName_ReturnsNotFound_WhenPokemonDoesNotExist()
    {
        var context = CreateDbContext();
        var controller = new PokemonController(context);

        var result = await controller.GetPokemonByName("MissingNo");

        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFoundResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task CreatePokemon_ReturnsBadRequest_WhenNameIsMissing()
    {
        var context = CreateDbContext();
        var controller = new PokemonController(context);

        var request = new CreatePokemonRequest
        {
            Name = "",
            Cry = "999.wav"
        };

        var result = await controller.CreatePokemon(request);

        var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task CreatePokemon_ReturnsBadRequest_WhenCryIsMissing()
    {
        var context = CreateDbContext();
        var controller = new PokemonController(context);

        var request = new CreatePokemonRequest
        {
            Name = "Testmon",
            Cry = ""
        };

        var result = await controller.CreatePokemon(request);

        var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task CreatePokemon_ReturnsConflict_WhenPokemonAlreadyExists()
    {
        var context = CreateDbContext();
        var controller = new PokemonController(context);

        var request = new CreatePokemonRequest
        {
            Name = "Bulbasaur",
            Cry = "001.wav"
        };

        var result = await controller.CreatePokemon(request);

        var conflictResult = result.Result.Should().BeOfType<ConflictObjectResult>().Subject;
        conflictResult.StatusCode.Should().Be(409);
    }

    [Fact]
    public async Task CreatePokemon_ReturnsCreated_WhenRequestIsValid()
    {
        var context = CreateDbContext();
        var controller = new PokemonController(context);

        var request = new CreatePokemonRequest
        {
            Name = "Pikachu",
            Cry = "025.wav"
        };

        var result = await controller.CreatePokemon(request);

        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);

        context.Pokemon.Any(p => p.Name == "Pikachu").Should().BeTrue();
    }
    
    
}
