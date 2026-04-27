using System;


class Program
{
    static void Main(string[] args)
    {
        var processor = new AudioProcessor();
        var result = processor.ProcessAudio(
            "vocal.mp3",
            "bulbasaur.wav"
        );

        Console.WriteLine(result);
    }
}