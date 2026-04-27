using System.Diagnostics;


public class AudioProcessor
{
    private readonly string pythonPath = "python";
    private readonly string scriptPath = "audio_processor.py";
    public string ProcessAudio(string vocalPath, string cryPath)
    {
        var psi = new ProcessStartInfo
        {
            FileName = pythonPath,
            Arguments = $"{scriptPath} \"{vocalPath}\" \"{cryPath}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi);

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        if (!string.IsNullOrEmpty(error))
        {
            throw new Exception($"Python Error: {error}");
        }

        return output.Trim(); // output.wav path
    }
}