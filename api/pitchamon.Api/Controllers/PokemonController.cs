using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pitchamon.Api.Data;
using pitchamon.Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace pitchamon.Api.Controllers;

[ApiController]
[Route("pokemon")]
[Authorize]
public class PokemonController : ControllerBase
{
    private readonly AppDbContext dbContext;

    public PokemonController(AppDbContext context)
    {
        dbContext = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<object>> GetAllPokemon()
    {
        var pokemon = await dbContext.Pokemon
            .OrderBy(p => p.Id)
            .ToListAsync();
        
        return Ok(new { pokemon });
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<Pokemon>> GetPokemonByName(string name)
    {
        var pokemon = await dbContext.Pokemon
            .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());

        if (pokemon == null)
        {
            return NotFound(new { error = "Pokémon not found" });
        }

        return Ok(pokemon);
    }

    [HttpGet("{name}/cry")]
    public async Task<IActionResult> GetPokemonCry(string name)
    {
        var pokemon = await dbContext.Pokemon
            .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());

        if (pokemon == null)
        {
            return NotFound(new { error = "Pokemon not found" });
        }

        var filePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Assets",
            "Cries",
            pokemon.Cry
            );

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound(new { error = "Cry file not found" });
        }
        
        return PhysicalFile(filePath, "audio/wav", pokemon.Cry);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CreatePokemon([FromBody] CreatePokemonRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Cry))
        {
            return BadRequest(new { error = "Missing required fields" });
        }

        var exists = await dbContext.Pokemon
            .AnyAsync(p => p.Name.ToLower() == request.Name.ToLower());

        if (exists)
        {
            return Conflict(new { error = "Pokémon already exists" });
        }
        
        var nextId = await dbContext.Pokemon.AnyAsync()
            ? await dbContext.Pokemon.MaxAsync(p => p.Id) + 1
            : 1;
        
        var pokemon = new Pokemon
        {
            Id = nextId,
            Name = request.Name,
            Cry = request.Cry
        };

        dbContext.Pokemon.Add(pokemon);
        await dbContext.SaveChangesAsync();

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