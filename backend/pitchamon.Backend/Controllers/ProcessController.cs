using Microsoft.AspNetCore.Mvc;
using pitchamon.Backend.Models;
using pitchamon.Backend.Services;

namespace pitchamon.Backend.Controllers;

[ApiController]
[Route("process")]
public class ProcessController : ControllerBase
{
    private readonly TemporaryFileService temporaryFileService;
    private readonly PokemonApiClient pokemonApiClient;

    public ProcessController(
        TemporaryFileService tempfileService,
        PokemonApiClient apiClient)
    {
        temporaryFileService = tempfileService;
        pokemonApiClient = apiClient;
    }
    
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ProcessSong([FromForm] ProcessRequest request)
    {
        if (request.Song == null || request.Song.Length == 0)
        {
            return BadRequest(new { error = "Song file is required" });
        }

        if (string.IsNullOrWhiteSpace(request.PokemonName))
        {
            return BadRequest(new { error = "Pokemon name is required" });
        }
        
        var allowedExtensions = new[] { ".mp3", ".wav" };
        var extension = Path.GetExtension(request.Song.FileName).ToLowerInvariant();
    
        if (!allowedExtensions.Contains(extension))
        {
             return BadRequest(new { error = "Only .mp3 and .wav files are supported." });
        }
       
        var savedPath = await temporaryFileService.SaveUploadedFile(request.Song);

        var cryBytes = await pokemonApiClient.GetCry(request.PokemonName);
        var cryPath = await temporaryFileService.SaveBytes(cryBytes, ".wav");
        //processing logic

        return Ok(new
        {
            message = "Song upload successful",
			pokemonName = request.PokemonName,
            originalFileName = request.Song.FileName,
			savedPath,
            cryPath,
			fileSize = request.Song.Length,
            crySize = cryBytes.Length,
        });
    }
}