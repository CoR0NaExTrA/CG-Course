using AsteroidGame;
using AsteroidGame.Objects;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System.Numerics;

class Program
{
    static void Main()
    {
        var options = WindowOptions.Default;
        options.Size = new Silk.NET.Maths.Vector2D<int>( 1000, 800 );
        options.Title = "Asteroids";

        var window = Window.Create( options );

        GL gl = null;
        Game game = null;

        window.Load += () =>
        {
            gl = GL.GetApi( window );
            game = new Game( gl, window );
            game.Load();
        };

        window.Update += ( dt ) =>
        {
            game.Update( ( float )dt );
        };

        window.Render += ( dt ) =>
        {
            game.Render();
        };

        window.FramebufferResize += size =>
        {
            gl.Viewport( 0, 0, ( uint )size.X, ( uint )size.Y );
            game.Resize( size.X, size.Y );
        };

        window.Run();
    }
}

public static class Collision
{
    public static bool Circle( Vector2 a, float ra, Vector2 b, float rb )
    {
        return Vector2.Distance( a, b ) < ra + rb;
    }
}

