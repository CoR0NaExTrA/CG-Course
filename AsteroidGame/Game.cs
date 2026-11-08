using AsteroidGame.Objects;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System.Numerics;

namespace AsteroidGame;

public class Game
{
    GL gl;
    IWindow window;

    Renderer renderer;

    Ship ship;

    List<Asteroid> asteroids = new();
    List<Bullet> bullets = new();
    float shootCooldown = 0;

    int lives = 3;
    int score = 0;
    int wave = 1;
    bool gameOver = false;
    float respawnTimer = 0;
    bool shipAlive = true;

    IKeyboard keyboard;
    IInputContext input;

    public Game( GL gl, IWindow window )
    {
        this.gl = gl;
        this.window = window;
    }

    public void Load()
    {
        renderer = new Renderer( gl );

        renderer.SetProjection( window.Size.X, window.Size.Y );

        ship = new Ship()
        {
            Position = new Vector2( 500, 400 )
        };

        input = window.CreateInput();
        keyboard = input.Keyboards[ 0 ];

        SpawnAsteroids();
    }

    void SpawnAsteroids()
    {
        int count = 4 + wave;

        for ( int i = 0; i < count; i++ )
        {
            Vector2 pos;

            do
            {
                pos = new Vector2(
                    Random.Shared.Next( window.Size.X ),
                    Random.Shared.Next( window.Size.Y ) );

            } while ( Vector2.Distance( pos, ship.Position ) < 300 );

            var asteroid = new Asteroid( pos, 3 );

            asteroid.Velocity = new Vector2(
                Random.Shared.NextSingle() * 100 - 50,
                Random.Shared.NextSingle() * 100 - 50 );

            asteroids.Add( asteroid );
        }
    }

    public void Update( float dt )
    {
        if ( !gameOver )
        {
            window.Title = $"Asteroids | Score: {score} | Lives: {lives} | Wave: {wave}";
        }
        else
        {
            window.Title = $"GAME OVER | Score: {score}. Press R for restart ";
        }

        ship.Thrust = 0;

        if ( keyboard.IsKeyPressed( Key.Left ) )
            ship.AngularVelocity = -4;

        if ( keyboard.IsKeyPressed( Key.Right ) )
            ship.AngularVelocity = 4;

        if ( keyboard.IsKeyPressed( Key.Up ) )
            ship.Thrust = 200;

        if ( gameOver && keyboard.IsKeyPressed( Key.R ) )
        {
            Restart();
        }

        if ( gameOver && keyboard.IsKeyPressed( Key.Escape ))
        {
            window.Close();
        }

        shootCooldown -= dt;

        if ( shipAlive && keyboard.IsKeyPressed( Key.Space ) && shootCooldown <= 0 )
        {
            shootCooldown = 0.25f;

            Vector2 dir = new Vector2(
                MathF.Cos( ship.Rotation ),
                MathF.Sin( ship.Rotation ) );

            bullets.Add( new Bullet()
            {
                Position = ship.Position,
                Velocity = dir * 500
            } );
        }

        if ( shipAlive )
        {
            ship.Update( dt );
            Wrap( ship );
        }

        foreach ( var a in asteroids )
        {
            a.Update( dt );
            Wrap( a );
        }

        foreach ( var b in bullets )
        {
            b.Update( dt );
            Wrap( b );
        }

        bullets.RemoveAll( b => b.Life <= 0 );

        CheckCollisions();
        CheckShipCollision();

        if ( asteroids.Count == 0 )
        {
            wave++;
            SpawnAsteroids();
        }

        if ( !shipAlive )
        {
            respawnTimer -= dt;

            if ( respawnTimer <= 0 && !gameOver )
            {
                ship.Position = new Vector2( window.Size.X / 2, window.Size.Y / 2 );
                ship.Velocity = Vector2.Zero;
                ship.Rotation = 0;
                shipAlive = true;
            }
        }
    }

    void CheckCollisions()
    {
        for ( int i = bullets.Count - 1; i >= 0; i-- )
        {
            for ( int j = asteroids.Count - 1; j >= 0; j-- )
            {
                if ( Collision.Circle(
                    bullets[ i ].Position,
                    5,
                    asteroids[ j ].Position,
                    asteroids[ j ].Size * 20 ) )
                {
                    score += 10;
                    var asteroid = asteroids[ j ];

                    bullets.RemoveAt( i );
                    asteroids.RemoveAt( j );

                    SplitAsteroid( asteroid );

                    break;
                }
            }
        }
    }

    public void Render()
    {
        gl.ClearColor( 0, 0, 0, 1 );
        gl.Clear( ClearBufferMask.ColorBufferBit );

        if ( shipAlive )
            ship.Draw( renderer );

        foreach ( var a in asteroids )
            a.Draw( renderer );

        foreach ( var b in bullets )
            b.Draw( renderer );
    }

    public void Resize( int width, int height )
    {
        renderer.SetProjection( width, height );
    }

    void Wrap( GameObject obj )
    {
        float w = window.Size.X;
        float h = window.Size.Y;

        if ( obj.Position.X < 0 )
            obj.Position.X = w;
        if ( obj.Position.X > w )
            obj.Position.X = 0;

        if ( obj.Position.Y < 0 )
            obj.Position.Y = h;
        if ( obj.Position.Y > h )
            obj.Position.Y = 0;
    }

    void SplitAsteroid( Asteroid asteroid )
    {
        if ( asteroid.Size <= 1 )
            return;

        for ( int i = 0; i < 2; i++ )
        {
            var newAsteroid = new Asteroid(
                asteroid.Position,
                asteroid.Size - 1 );

            float angle = Random.Shared.NextSingle() * MathF.PI * 2;

            newAsteroid.Velocity = new Vector2(
                MathF.Cos( angle ),
                MathF.Sin( angle ) ) * 120;

            asteroids.Add( newAsteroid );
        }
    }

    void CheckShipCollision()
    {
        if ( !shipAlive )
            return;

        foreach ( var asteroid in asteroids )
        {
            if ( Collision.Circle(
                ship.Position,
                20,
                asteroid.Position,
                asteroid.Size * 20 ) )
            {
                ShipDestroyed();
                break;
            }
        }
    }

    void ShipDestroyed()
    {
        lives--;
        shipAlive = false;
        respawnTimer = 2f;

        if ( lives <= 0 )
        {
            gameOver = true;
        }
    }

    void Restart()
    {
        score = 0;
        lives = 3;
        wave = 1;
        gameOver = false;

        asteroids.Clear();
        bullets.Clear();

        ship.Position = new Vector2( window.Size.X / 2, window.Size.Y / 2 );
        ship.Velocity = Vector2.Zero;

        SpawnAsteroids();
    }
}
