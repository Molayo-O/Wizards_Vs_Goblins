global using static Program; // Makes static fields on Program available globally (for `RNG`).

using COIS2020.MolayoOgunfowora0772346.Assignment3;
using COIS2020.StarterCode.Assignment3;

static class Program
{
    /// <summary>
    /// The random number generator used for all RNG in the program.
    /// </summary>
    public static Random RNG = new(10);


    private static void Main()
    {
        // The wizard/goblin ToString methods use emojis.
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var renderer = new CastleGameRenderer()
        {
            CaptureConsoleOutput = true,
            FrameDelayMS = 100,             // Controls how fast the animation plays
        };

        renderer.Run(new CastleDefender(), startPaused: false);
    }
}
