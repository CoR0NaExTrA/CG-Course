using Silk.NET.Maths;
using Silk.NET.Windowing;
using Maze3D.Core;

namespace Maze3D;

class Program
{
    static Game game;

    static void Main()
    {
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(Constants.WINDOW_WIDTH, Constants.WINDOW_HEIGHT);
        options.Title = Constants.WINDOW_TITLE;

        var window = Window.Create(options);
        game = new Game(window);

        window.Load += game.OnLoad;
        window.Update += game.OnUpdate;
        window.Render += game.OnRender;

        window.Run();
    }
}


//using Silk.NET.Input;
//using Silk.NET.Maths;
//using Silk.NET.OpenGL;
//using Silk.NET.Windowing;
//using System.Numerics;

//class Program
//{
//    static IWindow window;
//    static GL gl;

//    static IInputContext input;
//    static IKeyboard keyboard;
//    static IMouse mouse;

//    static uint vao, vbo;
//    static uint shader;

//    const int MAZE_SIZE = 17; // важно: нечётное число
//    const float PLAYER_RADIUS = 0.3f;

//    static int[,] maze = new int[ MAZE_SIZE, MAZE_SIZE ];

//    static Vector3 cameraPos = new Vector3( 1.5f, 1.0f, 1.5f );
//    static float yaw = -90f;
//    static float pitch = 0f;

//    static Vector2 lastMouse;
//    static bool firstMove = true;

//    static void Main()
//    {
//        var options = WindowOptions.Default;
//        options.Size = new Vector2D<int>( 1000, 800 );
//        options.Title = "3D Maze";

//        window = Window.Create( options );

//        window.Load += OnLoad;
//        window.Update += OnUpdate;
//        window.Render += OnRender;

//        window.Run();
//    }

//    static void OnLoad()
//    {
//        gl = GL.GetApi( window );

//        input = window.CreateInput();
//        keyboard = input.Keyboards[ 0 ];
//        mouse = input.Mice[ 0 ];

//        mouse.Cursor.CursorMode = CursorMode.Disabled;

//        GenerateMazeDFS();

//        CreateShader();
//        CreateBuffers();

//        gl.Enable( GLEnum.DepthTest );
//    }

//    // ==========================
//    // 🎮 Управление
//    // ==========================
//    static void OnUpdate( double dt )
//    {
//        float speed = 3f * ( float )dt;

//        Vector3 front = GetFront();
//        front.Y = 0;
//        front = Vector3.Normalize( front );

//        Vector3 right = Vector3.Normalize( Vector3.Cross( front, Vector3.UnitY ) );

//        Vector3 move = Vector3.Zero;

//        if ( keyboard.IsKeyPressed( Key.W ) )
//            move += front;
//        if ( keyboard.IsKeyPressed( Key.S ) )
//            move -= front;
//        if ( keyboard.IsKeyPressed( Key.A ) )
//            move -= right;
//        if ( keyboard.IsKeyPressed( Key.D ) )
//            move += right;

//        if ( move != Vector3.Zero )
//        {
//            move = Vector3.Normalize( move ) * speed;

//            // 🔒 Ограничение шага: не больше половины радиуса за кадр
//            float maxStep = PLAYER_RADIUS * 0.5f;
//            if ( move.Length() > maxStep )
//                move = Vector3.Normalize( move ) * maxStep;

//            // 🔄 Сначала пробуем движение по диагонали
//            Vector3 proposed = cameraPos + move;

//            bool canMoveX = false, canMoveZ = false;

//            // Проверяем движение по X отдельно
//            Vector3 testX = new Vector3( proposed.X, cameraPos.Y, cameraPos.Z );
//            if ( !IsWallColliding( testX, PLAYER_RADIUS ) )
//                canMoveX = true;

//            // Проверяем движение по Z отдельно  
//            Vector3 testZ = new Vector3( cameraPos.X, cameraPos.Y, proposed.Z );
//            if ( !IsWallColliding( testZ, PLAYER_RADIUS ) )
//                canMoveZ = true;

//            // Применяем движение: если можно по обеим осям — двигаемся диагонально
//            if ( canMoveX && canMoveZ )
//            {
//                cameraPos = proposed;
//            }
//            else if ( canMoveX )
//            {
//                cameraPos.X = testX.X;
//            }
//            else if ( canMoveZ )
//            {
//                cameraPos.Z = testZ.Z;
//            }
//            // Если ни по одной оси нельзя — не двигаемся вообще
//        }

//        cameraPos.Y = 1.0f;

//        // мышь
//        var mousePos = mouse.Position;

//        if ( firstMove )
//        {
//            lastMouse = mousePos;
//            firstMove = false;
//        }
//        else
//        {
//            var delta = mousePos - lastMouse;
//            lastMouse = mousePos;

//            float sensitivity = 0.1f;
//            yaw += delta.X * sensitivity;
//            pitch -= delta.Y * sensitivity;

//            pitch = Math.Clamp( pitch, -89f, 89f );
//        }
//    }

//    // ==========================
//    // 🎨 Рендер
//    // ==========================
//    static void OnRender( double dt )
//    {
//        gl.ClearColor( 0.1f, 0.1f, 0.15f, 1 );
//        gl.Clear( ( uint )( ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit ) );

//        gl.UseProgram( shader );

//        Matrix4x4 view = Matrix4x4.CreateLookAt( cameraPos, cameraPos + GetFront(), Vector3.UnitY );
//        Matrix4x4 proj = Matrix4x4.CreatePerspectiveFieldOfView(
//            MathF.PI / 4,
//            window.Size.X / ( float )window.Size.Y,
//            0.1f,
//            100f );

//        SetMatrix( "view", view );
//        SetMatrix( "projection", proj );

//        gl.BindVertexArray( vao );

//        // ПОЛ
//        SetColor( 0.2f, 0.7f, 0.2f );
//        for ( int x = 0; x < MAZE_SIZE; x++ )
//        {
//            for ( int z = 0; z < MAZE_SIZE; z++ )
//            {
//                Matrix4x4 model =
//                    Matrix4x4.CreateScale( 1f, 0.05f, 1f ) *
//                    Matrix4x4.CreateTranslation( x, -0.025f, z );

//                SetMatrix( "model", model );
//                gl.DrawArrays( GLEnum.Triangles, 0, 36 );
//            }
//        }

//        // СТЕНЫ
//        SetColor( 0.7f, 0.2f, 0.2f );
//        for ( int x = 0; x < MAZE_SIZE; x++ )
//        {
//            for ( int z = 0; z < MAZE_SIZE; z++ )
//            {
//                if ( maze[ x, z ] == 1 )
//                {
//                    Matrix4x4 model =
//                        Matrix4x4.CreateScale( 1f, 2f, 1f ) *
//                        Matrix4x4.CreateTranslation( x, 1f, z );

//                    float shade = 0.5f + 0.5f * ( ( x + z ) % 2 );
//                    SetColor( 0.7f * shade, 0.2f * shade, 0.2f * shade );

//                    SetMatrix( "model", model );
//                    gl.DrawArrays( GLEnum.Triangles, 0, 36 );
//                }
//            }
//        }

//        // ПОТОЛОК
//        SetColor( 0.2f, 0.2f, 0.7f );
//        for ( int x = 0; x < MAZE_SIZE; x++ )
//        {
//            for ( int z = 0; z < MAZE_SIZE; z++ )
//            {
//                Matrix4x4 model =
//                    Matrix4x4.CreateScale( 1f, 0.05f, 1f ) *
//                    Matrix4x4.CreateTranslation( x, 2.025f, z );

//                SetMatrix( "model", model );
//                gl.DrawArrays( GLEnum.Triangles, 0, 36 );
//            }
//        }

//        DrawMiniMap();
//    }

//    // ==========================
//    // 🧠 DFS лабиринт
//    // ==========================
//    static void GenerateMazeDFS()
//    {
//        // всё = стены
//        for ( int x = 0; x < MAZE_SIZE; x++ )
//            for ( int z = 0; z < MAZE_SIZE; z++ )
//                maze[ x, z ] = 1;

//        Random rand = new();

//        void Carve( int x, int z )
//        {
//            int[] dirs = { 0, 1, 2, 3 };
//            dirs = dirs.OrderBy( _ => rand.Next() ).ToArray();

//            foreach ( int dir in dirs )
//            {
//                int dx = 0, dz = 0;

//                switch ( dir )
//                {
//                    case 0:
//                        dx = 2;
//                        break;
//                    case 1:
//                        dx = -2;
//                        break;
//                    case 2:
//                        dz = 2;
//                        break;
//                    case 3:
//                        dz = -2;
//                        break;
//                }

//                int nx = x + dx;
//                int nz = z + dz;

//                if ( nx > 0 && nz > 0 && nx < MAZE_SIZE - 1 && nz < MAZE_SIZE - 1 )
//                {
//                    if ( maze[ nx, nz ] == 1 )
//                    {
//                        maze[ x + dx / 2, z + dz / 2 ] = 0;
//                        maze[ nx, nz ] = 0;

//                        Carve( nx, nz );
//                    }
//                }
//            }
//        }

//        maze[ 1, 1 ] = 0;
//        Carve( 1, 1 );
//    }

//    // ✅ УЛУЧШЕННАЯ проверка коллизий с запасом по границам
//    static bool IsWallColliding( Vector3 pos, float radius )
//    {
//        // Добавляем небольшой запас, чтобы игрок не "прилипал" к стенам
//        float checkRadius = radius + 0.05f;

//        // Проверяем 4 угла хитбокса + центр для надёжности
//        float[] offsets = { -checkRadius, 0f, +checkRadius };

//        foreach ( float ox in offsets )
//        {
//            foreach ( float oz in offsets )
//            {
//                // Пропускаем центр, если уже проверили углы (оптимизация)
//                // но оставляем для надёжности

//                int x = ( int )MathF.Floor( pos.X + ox );
//                int z = ( int )MathF.Floor( pos.Z + oz );

//                // Границы карты = стена
//                if ( x < 0 || z < 0 || x >= MAZE_SIZE || z >= MAZE_SIZE )
//                    return true;

//                // Если хоть одна точка в стене — коллизия
//                if ( maze[ x, z ] == 1 )
//                    return true;
//            }
//        }
//        return false;
//    }

//    static Vector3 GetFront()
//    {
//        return Vector3.Normalize( new Vector3(
//            MathF.Cos( MathF.PI / 180 * yaw ) * MathF.Cos( MathF.PI / 180 * pitch ),
//            MathF.Sin( MathF.PI / 180 * pitch ),
//            MathF.Sin( MathF.PI / 180 * yaw ) * MathF.Cos( MathF.PI / 180 * pitch )
//        ) );
//    }

//    static void CreateShader()
//    {
//        string vertex = @"
//        #version 330 core
//        layout (location = 0) in vec3 aPos;

//        uniform mat4 model;
//        uniform mat4 view;
//        uniform mat4 projection;

//        void main()
//        {
//            gl_Position = projection * view * model * vec4(aPos, 1.0);
//        }";

//        string fragment = @"
//        #version 330 core
//        out vec4 FragColor;
//        uniform vec3 color;

//        void main()
//        {
//            FragColor = vec4(color, 1.0);
//        }";

//        uint vs = gl.CreateShader( GLEnum.VertexShader );
//        gl.ShaderSource( vs, vertex );
//        gl.CompileShader( vs );

//        uint fs = gl.CreateShader( GLEnum.FragmentShader );
//        gl.ShaderSource( fs, fragment );
//        gl.CompileShader( fs );

//        shader = gl.CreateProgram();
//        gl.AttachShader( shader, vs );
//        gl.AttachShader( shader, fs );
//        gl.LinkProgram( shader );

//        gl.DeleteShader( vs );
//        gl.DeleteShader( fs );
//    }

//    static void CreateBuffers()
//    {
//        float[] cube =
//        {
//            -0.5f,-0.5f,-0.5f,  0.5f,-0.5f,-0.5f,  0.5f, 0.5f,-0.5f,
//             0.5f, 0.5f,-0.5f, -0.5f, 0.5f,-0.5f, -0.5f,-0.5f,-0.5f,

//            -0.5f,-0.5f, 0.5f,  0.5f,-0.5f, 0.5f,  0.5f, 0.5f, 0.5f,
//             0.5f, 0.5f, 0.5f, -0.5f, 0.5f, 0.5f, -0.5f,-0.5f, 0.5f,

//            -0.5f, 0.5f, 0.5f, -0.5f, 0.5f,-0.5f, -0.5f,-0.5f,-0.5f,
//            -0.5f,-0.5f,-0.5f, -0.5f,-0.5f, 0.5f, -0.5f, 0.5f, 0.5f,

//             0.5f, 0.5f, 0.5f,  0.5f, 0.5f,-0.5f,  0.5f,-0.5f,-0.5f,
//             0.5f,-0.5f,-0.5f,  0.5f,-0.5f, 0.5f,  0.5f, 0.5f, 0.5f,

//            -0.5f,-0.5f,-0.5f,  0.5f,-0.5f,-0.5f,  0.5f,-0.5f, 0.5f,
//             0.5f,-0.5f, 0.5f, -0.5f,-0.5f, 0.5f, -0.5f,-0.5f,-0.5f,

//            -0.5f, 0.5f,-0.5f,  0.5f, 0.5f,-0.5f,  0.5f, 0.5f, 0.5f,
//             0.5f, 0.5f, 0.5f, -0.5f, 0.5f, 0.5f, -0.5f, 0.5f,-0.5f
//        };

//        vao = gl.GenVertexArray();
//        vbo = gl.GenBuffer();

//        gl.BindVertexArray( vao );
//        gl.BindBuffer( GLEnum.ArrayBuffer, vbo );

//        unsafe
//        {
//            fixed ( float* v = cube )
//            {
//                gl.BufferData( GLEnum.ArrayBuffer, ( nuint )( cube.Length * sizeof( float ) ), v, GLEnum.StaticDraw );
//            }
//        }

//        gl.VertexAttribPointer( 0, 3, GLEnum.Float, false, 3 * sizeof( float ), 0 );
//        gl.EnableVertexAttribArray( 0 );
//    }

//    static void SetMatrix( string name, Matrix4x4 mat )
//    {
//        int loc = gl.GetUniformLocation( shader, name );
//        unsafe
//        {
//            gl.UniformMatrix4( loc, 1, false, ( float* )&mat );
//        }
//    }

//    static void SetColor( float r, float g, float b )
//    {
//        int loc = gl.GetUniformLocation( shader, "color" );
//        gl.Uniform3( loc, r, g, b );
//    }

//    static void DrawMiniMap()
//    {
//        gl.Viewport( 0, 0, 200, 200 );

//        Matrix4x4 view = Matrix4x4.Identity;
//        // Ортографическая проекция, совпадающая с координатами лабиринта [0..16]
//        Matrix4x4 proj = Matrix4x4.CreateOrthographicOffCenter(
//            0,                  // left
//            MAZE_SIZE,          // right  
//            0,                  // bottom
//            MAZE_SIZE,          // top
//            -1,                 // near
//            1                   // far
//        );

//        SetMatrix( "view", view );
//        SetMatrix( "projection", proj );

//        for ( int x = 0; x < MAZE_SIZE; x++ )
//        {
//            for ( int z = 0; z < MAZE_SIZE; z++ )
//            {
//                if ( maze[ x, z ] == 1 )
//                    SetColor( 1, 1, 1 );
//                else
//                    SetColor( 0, 0, 0 );

//                Matrix4x4 model =
//                    Matrix4x4.CreateScale( 1f, 1f, 1f ) *
//                    Matrix4x4.CreateTranslation( x, z, 0 );

//                SetMatrix( "model", model );
//                gl.DrawArrays( GLEnum.Triangles, 0, 6 );
//            }
//        }

//        // 🎯 Рисуем игрока (зелёная точка)
//        SetColor( 0f, 1f, 0f );
//        float playerX = cameraPos.X;
//        float playerZ = cameraPos.Z;

//        // Рисуем маленький квадрат 0.3x0.3 вокруг позиции игрока
//        Matrix4x4 playerModel =
//            Matrix4x4.CreateScale( 0.3f, 0.3f, 1f ) *
//            Matrix4x4.CreateTranslation( playerX, playerZ, 0.01f ); // чуть выше, чтобы не мерцало
//        SetMatrix( "model", playerModel );
//        gl.DrawArrays( GLEnum.Triangles, 0, 6 );

//        // Возврат viewport
//        gl.Viewport( 0, 0, ( uint )window.Size.X, ( uint )window.Size.Y );
//    }
//}