using FluentAssertions;
using Microsoft.AspNetCore.Http;
using pitchamon.Backend.Services;
using System.Text;

namespace pitchamon.Backend.Tests;

public class TemporaryFileServiceTests
{
    [Fact]
    public async Task SaveBytes_CreatesAFile()
    {
        var service = new TemporaryFileService();
        var data = Encoding.UTF8.GetBytes("hello");

        var path = await service.SaveBytes(data, ".txt");

        File.Exists(path).Should().BeTrue();
        var content = await File.ReadAllTextAsync(path);
        content.Should().Be("hello");

        File.Delete(path);
    }

    [Fact]
    public async Task SaveUploadedFile_CreatesAFile()
    {
        var service = new TemporaryFileService();
        var bytes = Encoding.UTF8.GetBytes("test audio");
        var stream = new MemoryStream(bytes);
        IFormFile file = new FormFile(stream, 0, bytes.Length, "Song", "song.mp3");

        var path = await service.SaveUploadedFile(file);

        File.Exists(path).Should().BeTrue();
        File.Delete(path);
    }
}