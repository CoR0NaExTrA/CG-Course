using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace Task1;

class Program
{
    static void Main( string[] args )
    {
        var options = WindowOptions.Default with
        {
            Title = "Chess Scene",
            Size = new Vector2D<int>( 1500, 800 ),
            VSync = true,
            ShouldSwapAutomatically = true
        };

        using var window = Window.Create( options );
        var chessGame = new ChessGame( window );

        window.Load += chessGame.OnLoad;
        window.Render += chessGame.OnRenderFrame;
        window.Update += chessGame.OnUpdateFrame;
        window.Resize += chessGame.OnResize;

        window.Run();
    }
}