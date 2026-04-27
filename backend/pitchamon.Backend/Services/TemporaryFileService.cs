namespace pitchamon.Backend.Services;
public class TemporaryFileService
{
    public async Task<string> SaveUploadedFile(IFormFile file)
    {
        var uploadsFolder = Path.Combine(Path.GetTempPath(), "pitchamon_uploads");
        Directory.CreateDirectory(uploadsFolder);
        
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var filePath = Path.Combine(uploadsFolder, $"{Guid.NewGuid()}{extension}");
        
        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
        
        return filePath;
    }

	public async Task<string> SaveBytes(byte[] data, string extension)
	{
		var tempFolder = Path.Combine(Path.GetTempPath(), "pitchamon_temp");
		Directory.CreateDirectory(tempFolder);

		var filePath = Path.Combine(tempFolder, $"{Guid.NewGuid()}{extension}");
		await File.WriteAllBytesAsync(filePath, data);

		return filePath;
	}

}