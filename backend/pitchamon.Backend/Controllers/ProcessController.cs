using Microsoft.AspNetCore.Mvc;
using pitchamon.Backend.Models;
using pitchamon.Backend.Services;

namespace pitchamon.Backend.Controllers;

[ApiController]
[Route("process")]
public class ProcessController : ControllerBase
{
    private readonly TemporaryFileService temporaryFileService;

    public ProcessController(TemporaryFileService tempfileService)
    {
        temporaryFileService = tempfileService;
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
        
        //processing logic

        return Ok(new
        {
            message = "Song upload successful",
			pokemonName = request.PokemonName,
            originalFileName = request.Song.FileName,
			savedPath,
			fileSize = request.Song.Length
        });
    }
}