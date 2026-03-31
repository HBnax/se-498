using Microsoft.AspNetCore.Mvc;
using pitchamon.Api.Models;

namespace pitchamon.Api.Controllers;

[ApiController]
[Route("pokemon")]
public class PokemonController : ControllerBase
{
    private static readonly List<Pokemon> PokemonList =
    [
        new Pokemon { Id = 25, Name = "Pikachu", Cry = "025.wav" },
        new Pokemon { Id = 4, Name = "Charmander", Cry = "004.wav" },
        new Pokemon { Id = 1, Name = "Bulbasaur", Cry = "001.wav" }
    ];

    [HttpGet]
    public ActionResult<object> GetAllPokemon()
    {
        return Ok(new { pokemon = PokemonList });
    }

    [HttpGet("{name}")]
    public ActionResult<Pokemon> GetPokemonByName(string name)
    {
        var pokemon = PokemonList.FirstOrDefault(p =>
            p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (pokemon == null)
        {
            return NotFound(new { error = "Pokémon not found" });
        }

        return Ok(pokemon);
    }

    [HttpPost]
    public ActionResult<object> CreatePokemon([FromBody] CreatePokemonRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Cry))
        {
            return BadRequest(new { error = "Missing required fields" });
        }

        var exists = PokemonList.Any(p =>
            p.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase));

        if (exists)
        {
            return Conflict(new { error = "Pokémon already exists" });
        }

        var nextId = PokemonList.Count == 0 ? 1 : PokemonList.Max(p => p.Id) + 1;

        var pokemon = new Pokemon
        {
            Id = nextId,
            Name = request.Name,
            Cry = request.Cry
        };

        PokemonList.Add(pokemon);

        return CreatedAtAction(
            nameof(GetPokemonByName),
            new { name = pokemon.Name },
            new
            {
                message = "Pokémon added successfully",
                pokemon
            });
    }
}