using MemoryTrainer.Core;
using MemoryTrainer.Input;
using MemoryTrainer.Rendering;
using Silk.NET.Maths;
using Silk.NET.Windowing;

class Program
{
    static void Main()
    {
        var opts = WindowOptions.Default with
        {
            Size = new Vector2D<int>( 1500, 1000 ),
            Title = "Memory Trainer 3D",
            WindowState = WindowState.Normal
        };

        using var window = Window.Create( opts );

        var game = new Game( 4, 4, 1.5f );
        var renderer = new Renderer( window, game );
        var inputHandler = new InputHandler( window, renderer.Camera, game );

        window.Load += () =>
        {
            renderer.Load();
            game.Initialize();
        };
        window.Update += game.Update;
        window.Render += renderer.Render;

        window.Run();
    }
}



//using Silk.NET.OpenGL;
//using Silk.NET.Windowing;
//using Silk.NET.Input;
//using Silk.NET.Maths;
//using System.Numerics;
//using MemoryTrainer.Core;

//namespace MemoryTrainer;

//class Program
//{
//    static IWindow window;
//    static GL gl;

//    static IInputContext input;
//    static IMouse mouse;

//    static uint vao;
//    static uint vbo;
//    static uint shader;

//    const int Rows = 4;
//    const int Cols = 4;

//    static Tile[,] tiles = new Tile[Rows, Cols];

//    static Tile firstSelected;
//    static Tile secondSelected;

//    static float spacing = 1.5f;

//    static Matrix4x4 projection;
//    static Matrix4x4 view;

//    static float hideTimer = 0f;
//    static bool waitingToHide = false;

//    static uint frontTexture;
//    static Dictionary<int, uint> backTextures = new();

//    static void Main()
//    {
//        var opts = WindowOptions.Default;
//        opts.Size = new Vector2D<int>(1200, 800);
//        opts.Title = "Memory Trainer 3D";

//        window = Window.Create(opts);

//        window.Load += Load;
//        window.Update += Update;
//        window.Render += Render;

//        window.Run();
//    }

//    static void Load()
//    {
//        gl = GL.GetApi(window);

//        input = window.CreateInput();
//        mouse = input.Mice[0];
//        mouse.MouseDown += MouseDown;

//        gl.Enable(GLEnum.DepthTest);

//        CreateShader();
//        CreateCube();
//        GenerateTiles();

//        projection = Matrix4x4.CreatePerspectiveFieldOfView(
//            MathF.PI / 4f,
//            window.Size.X / (float)window.Size.Y,
//            0.1f,
//            100f);

//        view = Matrix4x4.CreateLookAt(
//            new Vector3(6, 8, 10),
//            new Vector3(2.5f, 0, 2.5f),
//            Vector3.UnitY);

//        frontTexture = LoadTexture( "textures/front.png" );

//        for ( int i = 0; i < ( Rows * Cols ) / 2; i++ )
//        {
//            backTextures[ i ] = LoadTexture( $"textures/card{i}.png" );
//        }

//        gl.UseProgram( shader );
//        gl.Uniform1( gl.GetUniformLocation( shader, "tex" ), 0 );
//    }

//    static unsafe uint LoadTexture( string path )
//    {
//        var img = SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Rgba32>( path );

//        uint tex = gl.GenTexture();
//        gl.BindTexture( GLEnum.Texture2D, tex );

//        var pixels = new byte[ 4 * img.Width * img.Height ];
//        img.CopyPixelDataTo( pixels );

//        fixed ( void* p = pixels )
//        {
//            gl.TexImage2D( GLEnum.Texture2D, 0, ( int )GLEnum.Rgba,
//                ( uint )img.Width, ( uint )img.Height,
//                0, GLEnum.Rgba, GLEnum.UnsignedByte, p );
//        }

//        gl.GenerateMipmap( GLEnum.Texture2D );

//        gl.TexParameter( GLEnum.Texture2D, GLEnum.TextureWrapS, ( int )GLEnum.Repeat );
//        gl.TexParameter( GLEnum.Texture2D, GLEnum.TextureWrapT, ( int )GLEnum.Repeat );
//        gl.TexParameter( GLEnum.Texture2D, GLEnum.TextureMinFilter, ( int )GLEnum.LinearMipmapLinear );
//        gl.TexParameter( GLEnum.Texture2D, GLEnum.TextureMagFilter, ( int )GLEnum.Linear );

//        return tex;
//    }

//    static void GenerateTiles()
//    {
//        List<int> ids = new();
//        for (int i = 0; i < (Rows * Cols) / 2; i++)
//        {
//            ids.Add(i);
//            ids.Add(i);
//        }

//        Random r = new();
//        ids = ids.OrderBy(x => r.Next()).ToList();

//        int index = 0;
//        for (int r1 = 0; r1 < Rows; r1++)
//        {
//            for (int c = 0; c < Cols; c++)
//            {
//                tiles[r1, c] = new Tile
//                {
//                    Id = ids[index++],
//                    Position = new Vector3(c * spacing, 0, r1 * spacing)
//                };
//            }
//        }
//    }

//    static void Update(double dt)
//    {
//        foreach (var t in tiles)
//        {
//            float target = t.IsRevealed ? MathF.PI : 0;
//            t.Rotation = Lerp(t.Rotation, target, (float)dt * 6f);
//        }

//        if (waitingToHide)
//        {
//            hideTimer -= (float)dt;
//            if (hideTimer <= 0f)
//            {
//                if (firstSelected != null) firstSelected.IsRevealed = false;
//                if (secondSelected != null) secondSelected.IsRevealed = false;

//                firstSelected = null;
//                secondSelected = null;
//                waitingToHide = false;
//            }
//        }
//    }

//    static float Lerp(float a, float b, float t)
//    {
//        return a + (b - a) * t;
//    }

//    static void Render(double dt)
//    {
//        gl.Clear((uint)(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));

//        gl.UseProgram(shader);

//        foreach (var tile in tiles)
//        {
//            if (tile.IsMatched) continue;

//            Matrix4x4 model =
//                Matrix4x4.CreateTranslation( -0.5f, -0.1f, -0.5f ) *
//                Matrix4x4.CreateRotationX( tile.Rotation ) *
//                Matrix4x4.CreateTranslation( tile.Position + new Vector3( 0.5f, 0.1f, 0.5f ) );

//            SetMatrix("model", model);
//            SetMatrix("view", view);
//            SetMatrix("projection", projection);

//            gl.Uniform3( gl.GetUniformLocation( shader, "lightPos" ), new Vector3( 5, 10, 5 ) );
//            gl.Uniform3( gl.GetUniformLocation( shader, "viewPos" ), new Vector3( 6, 8, 10 ) );

//            uint tex;

//            if ( tile.Rotation >= MathF.PI / 2 )
//            {
//                bool isBack = tile.Rotation >= MathF.PI / 2;

//                gl.Uniform1( gl.GetUniformLocation( shader, "isBack" ), isBack ? 1 : 0 );
//                tex = backTextures[ tile.Id ];
//            }
//            else
//            {
//                tex = frontTexture;
//            }

//            gl.ActiveTexture( GLEnum.Texture0 );
//            gl.BindTexture( GLEnum.Texture2D, tex );

//            gl.BindVertexArray( vao );
//            gl.DrawArrays( GLEnum.Triangles, 0, 36 );
//        }
//    }

//    static void MouseDown( IMouse mouse, MouseButton button )
//    {
//        if ( button != MouseButton.Left )
//            return;

//        var pos = mouse.Position;

//        Vector3 rayOrigin;
//        Vector3 rayDir;
//        GetRay( pos.X, pos.Y, out rayOrigin, out rayDir );

//        float closest = float.MaxValue;
//        Tile hitTile = null;

//        foreach ( var t in tiles )
//        {
//            if ( t.IsMatched )
//                continue;

//            Vector3 min = t.Position;
//            Vector3 max = t.Position + new Vector3( 1f, 0.2f, 1f );

//            if ( RayIntersectsAABB( rayOrigin, rayDir, min, max, out float tHit ) )
//            {
//                if ( tHit < closest )
//                {
//                    closest = tHit;
//                    hitTile = t;
//                }
//            }
//        }

//        if ( hitTile != null )
//            OnTileClicked( hitTile );
//    }

//    static void GetRay( float mouseX, float mouseY, out Vector3 origin, out Vector3 dir )
//    {
//        float x = ( 2f * mouseX ) / window.Size.X - 1f;
//        float y = 1f - ( 2f * mouseY ) / window.Size.Y;

//        Vector4 rayClip = new Vector4( x, y, -1f, 1f );

//        Matrix4x4.Invert( projection, out var invProj );
//        Vector4 rayEye = Vector4.Transform( rayClip, invProj );
//        rayEye = new Vector4( rayEye.X, rayEye.Y, -1f, 0f );

//        Matrix4x4.Invert( view, out var invView );
//        Vector4 rayWorld = Vector4.Transform( rayEye, invView );

//        dir = Vector3.Normalize( new Vector3( rayWorld.X, rayWorld.Y, rayWorld.Z ) );
//        origin = new Vector3( 6, 8, 10 );
//    }

//    static bool RayIntersectsAABB( Vector3 rayOrigin, Vector3 rayDir, Vector3 min, Vector3 max, out float t )
//    {
//        t = 0;

//        float tmin = ( min.X - rayOrigin.X ) / rayDir.X;
//        float tmax = ( max.X - rayOrigin.X ) / rayDir.X;

//        if ( tmin > tmax )
//            (tmin, tmax) = (tmax, tmin);

//        float tymin = ( min.Y - rayOrigin.Y ) / rayDir.Y;
//        float tymax = ( max.Y - rayOrigin.Y ) / rayDir.Y;

//        if ( tymin > tymax )
//            (tymin, tymax) = (tymax, tymin);

//        if ( ( tmin > tymax ) || ( tymin > tmax ) )
//            return false;

//        if ( tymin > tmin )
//            tmin = tymin;
//        if ( tymax < tmax )
//            tmax = tymax;

//        float tzmin = ( min.Z - rayOrigin.Z ) / rayDir.Z;
//        float tzmax = ( max.Z - rayOrigin.Z ) / rayDir.Z;

//        if ( tzmin > tzmax )
//            (tzmin, tzmax) = (tzmax, tzmin);

//        if ( ( tmin > tzmax ) || ( tzmin > tmax ) )
//            return false;

//        t = tmin;
//        return true;
//    }

//    static void OnTileClicked(Tile tile)
//    {
//        if (tile.IsMatched || tile.IsAnimating) return;

//        if (tile == firstSelected)
//        {
//            tile.IsRevealed = false;
//            firstSelected = null;
//            return;
//        }

//        if (firstSelected == null)
//        {
//            firstSelected = tile;
//            tile.IsRevealed = true;
//            return;
//        }

//        if (secondSelected == null)
//        {
//            secondSelected = tile;
//            tile.IsRevealed = true;

//            if (firstSelected.Id == secondSelected.Id)
//            {
//                firstSelected.IsMatched = true;
//                secondSelected.IsMatched = true;
//                firstSelected = null;
//                secondSelected = null;
//            }
//            else
//            {
//                hideTimer = 1.0f;
//                waitingToHide = true;
//            }
//        }
//    }

//    static unsafe void CreateCube()
//    {
//        float s = 1f;
//        float h = 0.2f;

//        float[] v =
//        {
//        // ВЕРХ (0,1,0)
//        0,h,0, 0,1,0,   0,0,
//        s,h,0, 0,1,0,   1,0,
//        s,h,s, 0,1,0,   1,1,
//        0,h,0, 0,1,0,   0,0,
//        s,h,s, 0,1,0,   1,1,
//        0,h,s, 0,1,0,   0,1,

//        // НИЗ (0,-1,0)
//        0,0,0, 0,-1,0,  0,0,
//        s,0,s, 0,-1,0,  1,1,
//        s,0,0, 0,-1,0,  1,0,
//        0,0,0, 0,-1,0,  0,0,
//        0,0,s, 0,-1,0,  0,1,
//        s,0,s, 0,-1,0,  1,1,

//        // ЛЕВО (-1,0,0)
//        0,0,0, -1,0,0,  0,0,
//        0,h,s, -1,0,0,  1,1,
//        0,h,0, -1,0,0,  1,0,
//        0,0,0, -1,0,0,  0,0,
//        0,0,s, -1,0,0,  0,1,
//        0,h,s, -1,0,0,  1,1,

//        // ПРАВО (1,0,0)
//        s,0,0, 1,0,0,   0,0,
//        s,h,0, 1,0,0,   1,0,
//        s,h,s, 1,0,0,   1,1,
//        s,0,0, 1,0,0,   0,0,
//        s,h,s, 1,0,0,   1,1,
//        s,0,s, 1,0,0,   0,1,

//        // ПЕРЕД (0,0,1)
//        0,0,s, 0,0,1,   0,0,
//        s,h,s, 0,0,1,   1,1,
//        s,0,s, 0,0,1,   1,0,
//        0,0,s, 0,0,1,   0,0,
//        0,h,s, 0,0,1,   0,1,
//        s,h,s, 0,0,1,   1,1,

//        // ЗАД (0,0,-1)
//        0,0,0, 0,0,-1,  0,0,
//        s,0,0, 0,0,-1,  1,0,
//        s,h,0, 0,0,-1,  1,1,
//        0,0,0, 0,0,-1,  0,0,
//        s,h,0, 0,0,-1,  1,1,
//        0,h,0, 0,0,-1,  0,1,
//    };

//        vao = gl.GenVertexArray();
//        vbo = gl.GenBuffer();

//        gl.BindVertexArray( vao );
//        gl.BindBuffer( GLEnum.ArrayBuffer, vbo );

//        fixed ( float* ptr = v )
//            gl.BufferData( GLEnum.ArrayBuffer, ( nuint )( v.Length * sizeof( float ) ), ptr, GLEnum.StaticDraw );

//        gl.VertexAttribPointer( 0, 3, GLEnum.Float, false, 8 * sizeof( float ), 0 );
//        gl.EnableVertexAttribArray( 0 );

//        gl.VertexAttribPointer( 1, 3, GLEnum.Float, false, 8 * sizeof( float ), 3 * sizeof( float ) );
//        gl.EnableVertexAttribArray( 1 );

//        gl.VertexAttribPointer( 2, 2, GLEnum.Float, false, 8 * sizeof( float ), 6 * sizeof( float ) );
//        gl.EnableVertexAttribArray( 2 );
//    }

//    static void CreateShader()
//    {
//        string vertex = @"
//            #version 330 core
//            layout (location = 0) in vec3 aPos;
//            layout (location = 1) in vec3 aNormal;
//            layout (location = 2) in vec2 aTex;

//            out vec3 FragPos;
//            out vec3 Normal;
//            out vec2 TexCoord;

//            uniform mat4 model;
//            uniform mat4 view;
//            uniform mat4 projection;

//            void main()
//            {
//                FragPos = vec3(model * vec4(aPos,1.0));
//                Normal = mat3(transpose(inverse(model))) * aNormal;
//                TexCoord = aTex;

//                gl_Position = projection * view * model * vec4(aPos,1.0);
//            }";

//        string fragment = @"
//            #version 330 core
//            out vec4 FragColor;

//            in vec3 FragPos;
//            in vec3 Normal;
//            in vec2 TexCoord;

//            uniform vec3 lightPos;
//            uniform vec3 viewPos;
//            uniform vec3 color;
//            uniform sampler2D tex;
//            uniform int isBack;

//            void main()
//            {
//                float ambientStrength = 0.5;
//                vec3 ambient = ambientStrength * vec3(1.0);

//                vec3 norm = normalize(Normal);
//                vec3 lightDir = normalize(lightPos - FragPos);

//                float diff = max(dot(norm, lightDir), 0.0);
//                vec3 diffuse = diff * vec3(1.0);

//                vec3 result = ambient + diffuse;

//                vec2 uv = TexCoord;

//                if (isBack == 1)
//                    uv.y = 1.0 - uv.y;

//                FragColor = texture(tex, uv) * vec4(result, 1.0);
//            }";

//        uint vs = gl.CreateShader(GLEnum.VertexShader);
//        gl.ShaderSource(vs, vertex);
//        gl.CompileShader(vs);

//        uint fs = gl.CreateShader(GLEnum.FragmentShader);
//        gl.ShaderSource(fs, fragment);
//        gl.CompileShader(fs);

//        shader = gl.CreateProgram();
//        gl.AttachShader(shader, vs);
//        gl.AttachShader(shader, fs);
//        gl.LinkProgram(shader);

//        gl.DeleteShader(vs);
//        gl.DeleteShader(fs);
//    }

//    static unsafe void SetMatrix(string name, Matrix4x4 mat)
//    {
//        int loc = gl.GetUniformLocation(shader, name);
//        gl.UniformMatrix4(loc, 1, false, (float*)&mat);
//    }
//}

//class Tile
//{
//    public int Id;
//    public bool IsRevealed;
//    public bool IsMatched;
//    public float Rotation;
//    public Vector3 Position;
//    public bool IsAnimating;
//}