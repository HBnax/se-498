namespace pitchamon.Backend.Models;

public class ProcessingHistory
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string OriginalSongFile { get; set; } = string.Empty;
    public string PokemonUsed { get; set; } = string.Empty;
    public string CryFileUsed { get; set; } = string.Empty;
    public string ProcessedSongFile { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}