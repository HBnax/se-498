using Microsoft.AspNetCore.Mvc;
using pitchamon.Backend.Models;
using pitchamon.Backend.Services;
using pitchamon.Backend.Data;

namespace pitchamon.Backend.Controllers;

[ApiController]
[Route("process")]
public class ProcessController : ControllerBase
{
    private readonly TemporaryFileService temporaryFileService;
    private readonly PokemonApiClient pokemonApiClient;
    private readonly BackendDbContext dbContext;
	AudioProcessor audioProcessor = new AudioProcessor();

    public ProcessController(
        TemporaryFileService tempfileService,
        PokemonApiClient apiClient,
        BackendDbContext context)
    {
        temporaryFileService = tempfileService;
        pokemonApiClient = apiClient;
        dbContext = context;
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
		
        // returning output Byte
		var outputPath = audioProcessor.ProcessAudio(savedPath, cryPath);
		
        // adding to processed song db
         if (request.UserId.HasValue && outputPath != null)
         {
             var history = new ProcessingHistory
             {
                 UserId = request.UserId.Value,
                 OriginalSongFile = request.Song.FileName,
                 PokemonUsed = request.PokemonName,
                 CryFileUsed = Path.GetFileName(cryPath),
                 ProcessedSongFile = Path.GetFileName(outputPath),
                 CreatedAt = DateTime.UtcNow
             };

             dbContext.ProcessingHistory.Add(history);
             await dbContext.SaveChangesAsync();
        }

		if (outputPath == null)
        {
            return StatusCode(500, new { error = "Audio processing failed" });
        }

		return PhysicalFile(outputPath, "audio/wav", "processed_file.wav");
    }
}