using Battleship3D.Core;
using Battleship3D.Models;
using Battleship3D.Services;
using SeaBattle.Services;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System.Numerics;
using Mesh = Battleship3D.Models.Mesh;

namespace Battleship3D;

class Program
{
    static IWindow window;
    static GL gl;

    static Renderer renderer;
    static AudioService audio;
    static Camera camera = new();

    static List<Ship> ships = new();
    static List<Torpedo> torpedoes = new();

    static Mesh ship1Mesh;
    static Mesh ship2Mesh;
    static Mesh ship3Mesh;
    static Mesh torpedoMesh;
    static WaterMesh water;

    static uint ship1Tex;
    static uint ship2Tex;
    static uint ship3Tex;
    static uint torpedoTex;
    static uint waterTexture;

    static uint shootSound;
    static uint explosionSound;
    static uint waterSound;

    static uint waterSource;

    static int lives = 3;
    static int score = 0;
    static float shootCooldown = 0;

    static Random random = new();

    static void Main()
    {
        var options = WindowOptions.Default;

        options.Size = new Vector2D<int>( 1280, 720 );
        options.Title = "Battleship 3D";

        window = Window.Create( options );

        window.Load += Load;
        window.Render += Render;
        window.Update += Update;

        window.Run();
    }

    static void Load()
    {
        gl = GL.GetApi( window );

        gl.Enable( EnableCap.DepthTest );

        string vertex = File.ReadAllText( "D:\\Lab6CG\\SeaBattle\\Shaders\\shader.vert" );
        string fragment = File.ReadAllText( "D:\\Lab6CG\\SeaBattle\\Shaders\\shader.frag" );

        renderer = new Renderer( gl, camera );
        audio = new AudioService();

        ship1Mesh = new Mesh( gl, "models/battleship/battleship.obj" );
        ship2Mesh = new Mesh( gl, "models/cruiser/cruiser.obj" );
        ship3Mesh = new Mesh( gl, "models/destroyer/destroyer.obj" );
        torpedoMesh = new Mesh( gl, "models/torpedo/torpedo.obj" );
        water = new WaterMesh( gl );

        ship1Tex = TextureLoader.Load( gl, "models/battleship/battleship.jpg" );
        ship2Tex = TextureLoader.Load( gl, "models/cruiser/cruiser.jpg" );
        ship3Tex = TextureLoader.Load( gl, "models/destroyer/destroyer.jpg" );
        torpedoTex = TextureLoader.Load( gl, "models/torpedo/torpedo.jpg" );
        waterTexture = TextureLoader.Load( gl, "models/water.jpg" );

        shootSound = audio.LoadWav( "sounds/shoot.wav" );
        explosionSound = audio.LoadWav( "sounds/explosion.wav" );
        waterSource = audio.CreateSource( waterSound, true );

        audio.Play( waterSource );

        SpawnShip();
        SpawnShip();
        SpawnShip();

        var input = window.CreateInput();

        foreach ( var mouse in input.Mice )
        {
            mouse.MouseDown += MouseDown;
        }
    }

    static void MouseDown( IMouse mouse, MouseButton button )
    {
        if ( button != MouseButton.Left )
            return;

        if ( shootCooldown > 0 )
            return;

        shootCooldown = 2f;

        uint src = audio.CreateSource( shootSound );

        audio.Play( src );

        Vector3 dir = new(
            ( mouse.Position.X / 1280f - 0.5f ) * 2f,
            0,
            -1f );

        Torpedo torpedo = new(
            torpedoMesh,
            torpedoTex,
            new Vector3( 0, -0.5f, 10 ),
            dir );

        torpedoes.Add( torpedo );
    }

    static void SpawnShip()
    {
        Ship ship = ShipSpawner.Spawn(
            random,
            ship1Mesh,
            ship2Mesh,
            ship3Mesh,
            ship1Tex,
            ship2Tex,
            ship3Tex );

        ships.Add( ship );
    }

    static void Update( double delta )
    {
        float dt = ( float )delta;

        if ( shootCooldown > 0 )
            shootCooldown -= dt;

        foreach ( var ship in ships )
            ship.Update( dt );

        foreach ( var torpedo in torpedoes )
            torpedo.Update( dt );

        foreach ( var torpedo in torpedoes )
        {
            if ( !torpedo.Active )
                continue;

            foreach ( var ship in ships )
            {
                if ( !ship.Active || ship.Sinking )
                    continue;

                if ( Collision.Check( torpedo.Position, ship.Position, 4f ) )
                {
                    ship.Sinking = true;
                    torpedo.Active = false;

                    uint src = audio.CreateSource( explosionSound );
                    audio.Play( src );

                    score += 100;
                }
            }
        }

        for ( int i = ships.Count - 1; i >= 0; i-- )
        {
            if ( !ships[ i ].Sinking && ships[ i ].Position.X > 100 )
            {
                ships.RemoveAt( i );

                lives--;

                SpawnShip();
            }
            else if ( !ships[ i ].Active )
            {
                ships.RemoveAt( i );
                SpawnShip();
            }
        }

        torpedoes.RemoveAll( t => !t.Active );

        if ( lives <= 0 )
        {
            Console.WriteLine( "GAME OVER" );
            Environment.Exit( 0 );
        }

        string reloadText = shootCooldown > 0 ? $"Reload: {shootCooldown:F1}s" : "READY";

        window.Title = $"Battleship 3D | Lives: {lives} | Score: {score} | {reloadText}";
    }

    static void Render( double delta )
    {
        renderer.BeginFrame();

        renderer.DrawWater( water, waterTexture );

        foreach ( var ship in ships )
        {
            if ( ship.Active )
                renderer.DrawObject( ship );
        }

        foreach ( var torpedo in torpedoes )
        {
            if ( torpedo.Active )
                renderer.DrawObject( torpedo );
        }
    }
}