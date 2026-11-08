using Silk.NET.Maths;
using Silk.NET.Windowing;
using SnubDodecahedron.Core;

namespace SnubDodecahedron;

class Program
{
    static void Main()
    {
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>( 800, 600 );

        var window = Window.Create( options );
        new WindowApp( window );

        window.Run();
    }
}


//using Silk.NET.Input;
//using Silk.NET.Maths;
//using Silk.NET.OpenGL;
//using Silk.NET.Windowing;
//using SnubDodecahedron;
//using System.Numerics;

//class Program
//{
//    private static IWindow window;
//    private static GL gl;

//    private static IInputContext input;
//    private static IMouse mouse;

//    private static float yaw = 0;
//    private static float pitch = 0;
//    private static float radius = 6f;

//    private static Vector2 lastMouse;
//    private static bool firstMove = true;

//    private static uint vao;
//    private static uint vbo;
//    private static uint ebo;

//    private static DodecahedronShader shader;

//    static Random rand = new Random();

//    static void Main()
//    {
//        var options = WindowOptions.Default;
//        options.Size = new Vector2D<int>( 800, 600 );
//        options.Title = "Snub Dodecahedron";
//        options.API = new GraphicsAPI( ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Default, new APIVersion( 4, 1 ) );

//        window = Window.Create( options );

//        window.Load += OnLoad;
//        window.Render += OnRender;

//        window.Run();
//    }

//    static unsafe void OnLoad()
//    {
//        gl = GL.GetApi( window );
//        shader = new DodecahedronShader( gl );

//        input = window.CreateInput();

//        if ( input.Mice.Count > 0 )
//        {
//            mouse = input.Mice[ 0 ];
//            mouse.MouseMove += MouseMove;
//        }

//        gl.Enable( GLEnum.DepthTest );
//        gl.Disable( GLEnum.CullFace );

//        var vertices = CreateVertices();
//        var indices = CreateIndices();

//        vao = gl.GenVertexArray();
//        vbo = gl.GenBuffer();
//        ebo = gl.GenBuffer();

//        gl.BindVertexArray( vao );

//        gl.BindBuffer( GLEnum.ArrayBuffer, vbo );
//        fixed ( Vertex* v = &vertices[ 0 ] )
//        {
//            gl.BufferData( GLEnum.ArrayBuffer,
//                ( nuint )( vertices.Length * sizeof( Vertex ) ),
//                v,
//                GLEnum.StaticDraw );
//        }

//        gl.BindBuffer( GLEnum.ElementArrayBuffer, ebo );
//        fixed ( uint* i = &indices[ 0 ] )
//        {
//            gl.BufferData( GLEnum.ElementArrayBuffer,
//                ( nuint )( indices.Length * sizeof( uint ) ),
//                i,
//                GLEnum.StaticDraw );
//        }

//        gl.VertexAttribPointer( 0, 3, GLEnum.Float, false, ( uint )sizeof( Vertex ), ( void* )0 );
//        gl.EnableVertexAttribArray( 0 );

//        gl.VertexAttribPointer( 1, 3, GLEnum.Float, false, ( uint )sizeof( Vertex ), ( void* )12 );
//        gl.EnableVertexAttribArray( 1 );

//        gl.VertexAttribPointer( 2, 3, GLEnum.Float, false, ( uint )sizeof( Vertex ), ( void* )24 );
//        gl.EnableVertexAttribArray( 2 );
//    }

//    private static void MouseMove( IMouse mouse, Vector2 position )
//    {
//        if ( firstMove )
//        {
//            lastMouse = position;
//            firstMove = false;
//        }

//        var delta = position - lastMouse;
//        lastMouse = position;

//        if ( mouse.IsButtonPressed( MouseButton.Left ) )
//        {
//            yaw += delta.X * 0.01f;
//            pitch += delta.Y * 0.01f;

//            pitch = Math.Clamp( pitch, -1.5f, 1.5f );
//        }
//    }

//    static Vector3 GetCameraPosition()
//    {
//        float x = radius * MathF.Cos( pitch ) * MathF.Cos( yaw );
//        float y = radius * MathF.Sin( pitch );
//        float z = radius * MathF.Cos( pitch ) * MathF.Sin( yaw );

//        return new Vector3( x, y, z );
//    }

//    static unsafe void OnRender( double delta )
//    {
//        gl.ClearColor( 1f, 1f, 1f, 1f );
//        gl.Clear( ( uint )( ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit ) );

//        shader.Use();

//        Matrix4x4 model = Matrix4x4.Identity;

//        Vector3 cameraPos = GetCameraPosition();

//        Matrix4x4 view =
//            Matrix4x4.CreateLookAt(
//                cameraPos,
//                Vector3.Zero,
//                Vector3.UnitY
//            );

//        Matrix4x4 projection =
//            Matrix4x4.CreatePerspectiveFieldOfView(
//                MathF.PI / 4,
//                800f / 600f,
//                0.1f,
//                100f );

//        int modelLoc = gl.GetUniformLocation( shader.GetProgram(), "model" );
//        gl.UniformMatrix4( modelLoc, 1, false, ( float* )&model );

//        int viewLoc = gl.GetUniformLocation( shader.GetProgram(), "view" );
//        gl.UniformMatrix4( viewLoc, 1, false, ( float* )&view );

//        int projLoc = gl.GetUniformLocation( shader.GetProgram(), "projection" );
//        gl.UniformMatrix4( projLoc, 1, false, ( float* )&projection );

//        int lightPosLoc = gl.GetUniformLocation( shader.GetProgram(), "lightPos" );
//        int isWireframeLoc = gl.GetUniformLocation( shader.GetProgram(), "isWireframe" );
//        Vector3 lightPos = new Vector3( 20, 30, 20 );

//        gl.Uniform3( lightPosLoc, lightPos );

//        gl.BindVertexArray( vao );

//        int indexCount = 348;

//        gl.Uniform1( isWireframeLoc, 0 );
//        gl.DrawElements( GLEnum.Triangles, ( uint )indexCount, GLEnum.UnsignedInt, null );

//        gl.PolygonMode( GLEnum.FrontAndBack, GLEnum.Line );
//        gl.LineWidth( 1.5f );
//        gl.Uniform1( isWireframeLoc, 1 );
//        gl.DrawElements( GLEnum.Triangles, ( uint )indexCount, GLEnum.UnsignedInt, null );
//        gl.PolygonMode( GLEnum.FrontAndBack, GLEnum.Fill );
//    }

//    static Vertex[] CreateVertices()
//    {
//        float phi = ( 1f + MathF.Sqrt( 5f ) ) / 2f;
//        float discriminant = phi - 5f / 27f;
//        float sqrtDisc = MathF.Sqrt( discriminant );
//        float x = MathF.Cbrt( ( phi + sqrtDisc ) / 2f ) + MathF.Cbrt( ( phi - sqrtDisc ) / 2f );

//        float[] C =
//        [
//            phi * MathF.Sqrt( 3f - x * x ) / 2f,
//            x * phi * MathF.Sqrt( 3f - x * x ) / 2f,
//            phi * MathF.Sqrt( ( x - 1f - 1f / x ) * phi ) / 2f,
//            x * x * phi * MathF.Sqrt( 3f - x * x ) / 2f,
//            x * phi * MathF.Sqrt( ( x - 1f - 1f / x ) * phi ) / 2f,
//            phi * MathF.Sqrt( 1f - x + ( 1f + phi ) / x ) / 2f,
//            phi * MathF.Sqrt( x + 1f - phi ) / 2f,
//            x * x * phi * MathF.Sqrt( ( x - 1f - 1f / x ) * phi ) / 2f,
//            x * phi * MathF.Sqrt( 1f - x + ( 1f + phi ) / x ) / 2f,
//            MathF.Sqrt( ( x + 2f ) * phi + 2f ) / 2f,
//            x * MathF.Sqrt( x * ( 1f + phi ) - phi ) / 2f,
//            MathF.Sqrt( x * x * ( 1f + 2f * phi ) - phi ) / 2f,
//            phi * MathF.Sqrt( x * x + x ) / 2f,
//            phi * phi * MathF.Sqrt( x * ( x + phi ) + 1f ) / ( 2f * x ),
//            phi * MathF.Sqrt( x * ( x + phi ) + 1f ) / 2f,
//        ];

//        Vector3[] V =
//        [
//            new Vector3( C[ 2 ], C[ 1 ], C[ 14 ] ),
//            new Vector3( C[ 2 ], -C[ 1 ], -C[ 14 ] ),
//            new Vector3( -C[ 2 ], -C[ 1 ], C[ 14 ] ),
//            new Vector3( -C[ 2 ], C[ 1 ], -C[ 14 ] ),
//            new Vector3( C[ 14 ], C[ 2 ], C[ 1 ] ),
//            new Vector3( C[ 14 ], -C[ 2 ], -C[ 1 ] ),
//            new Vector3( -C[ 14 ], -C[ 2 ], C[ 1 ] ),
//            new Vector3( -C[ 14 ], C[ 2 ], -C[ 1 ] ),
//            new Vector3( C[ 1 ], C[ 14 ], C[ 2 ] ),
//            new Vector3( C[ 1 ], -C[ 14 ], -C[ 2 ] ),
//            new Vector3( -C[ 1 ], -C[ 14 ], C[ 2 ] ),
//            new Vector3( -C[ 1 ], C[ 14 ], -C[ 2 ] ),

//            new Vector3( C[ 3 ], -C[ 4 ], C[ 13 ] ),
//            new Vector3( C[ 3 ], C[ 4 ], -C[ 13 ] ),
//            new Vector3( -C[ 3 ], C[ 4 ], C[ 13 ] ),
//            new Vector3( -C[ 3 ], -C[ 4 ], -C[ 13 ] ),
//            new Vector3( C[ 13 ], -C[ 3 ], C[ 4 ] ),
//            new Vector3( C[ 13 ], C[ 3 ], -C[ 4 ] ),
//            new Vector3( -C[ 13 ], C[ 3 ], C[ 4 ] ),
//            new Vector3( -C[ 13 ], -C[ 3 ], -C[ 4 ] ),
//            new Vector3( C[ 4 ], -C[ 13 ], C[ 3 ] ),
//            new Vector3( C[ 4 ], C[ 13 ], -C[ 3 ] ),
//            new Vector3( -C[ 4 ], C[ 13 ], C[ 3 ] ),
//            new Vector3( -C[ 4 ], -C[ 13 ], -C[ 3 ] ),

//            new Vector3( C[ 0 ], C[ 8 ], C[ 12 ] ),
//            new Vector3( C[ 0 ], -C[ 8 ], -C[ 12 ] ),
//            new Vector3( -C[ 0 ], -C[ 8 ], C[ 12 ] ),
//            new Vector3( -C[ 0 ], C[ 8 ], -C[ 12 ] ),
//            new Vector3( C[ 12 ], C[ 0 ], C[ 8 ] ),
//            new Vector3( C[ 12 ], -C[ 0 ], -C[ 8 ] ),
//            new Vector3( -C[ 12 ], -C[ 0 ], C[ 8 ] ),
//            new Vector3( -C[ 12 ], C[ 0 ], -C[ 8 ] ),
//            new Vector3( C[ 8 ], C[ 12 ], C[ 0 ] ),
//            new Vector3( C[ 8 ], -C[ 12 ], -C[ 0 ] ),
//            new Vector3( -C[ 8 ], -C[ 12 ], C[ 0 ] ),
//            new Vector3( -C[ 8 ], C[ 12 ], -C[ 0 ] ),

//            new Vector3( C[ 7 ], C[ 6 ], C[ 11 ] ),
//            new Vector3( C[ 7 ], -C[ 6 ], -C[ 11 ] ),
//            new Vector3( -C[ 7 ], -C[ 6 ], C[ 11 ] ),
//            new Vector3( -C[ 7 ], C[ 6 ], -C[ 11 ] ),
//            new Vector3( C[ 11 ], C[ 7 ], C[ 6 ] ),
//            new Vector3( C[ 11 ], -C[ 7 ], -C[ 6 ] ),
//            new Vector3( -C[ 11 ], -C[ 7 ], C[ 6 ] ),
//            new Vector3( -C[ 11 ], C[ 7 ], -C[ 6 ] ),
//            new Vector3( C[ 6 ], C[ 11 ], C[ 7 ] ),
//            new Vector3( C[ 6 ], -C[ 11 ], -C[ 7 ] ),
//            new Vector3( -C[ 6 ], -C[ 11 ], C[ 7 ] ),
//            new Vector3( -C[ 6 ], C[ 11 ], -C[ 7 ] ),

//            new Vector3( C[ 9 ], -C[ 5 ], C[ 10 ] ),
//            new Vector3( C[ 9 ], C[ 5 ], -C[ 10 ] ),
//            new Vector3( -C[ 9 ], C[ 5 ], C[ 10 ] ),
//            new Vector3( -C[ 9 ], -C[ 5 ], -C[ 10 ] ),
//            new Vector3( C[ 10 ], -C[ 9 ], C[ 5 ] ),
//            new Vector3( C[ 10 ], C[ 9 ], -C[ 5 ] ),
//            new Vector3( -C[ 10 ], C[ 9 ], C[ 5 ] ),
//            new Vector3( -C[ 10 ], -C[ 9 ], -C[ 5 ] ),
//            new Vector3( C[ 5 ], -C[ 10 ], C[ 9 ] ),
//            new Vector3( C[ 5 ], C[ 10 ], -C[ 9 ] ),
//            new Vector3( -C[ 5 ], C[ 10 ], C[ 9 ] ),
//            new Vector3( -C[ 5 ], -C[ 10 ], -C[ 9 ] ),
//        ];

//        Vertex[] vertices = new Vertex[ 60 ];

//        Vector3 pentagonColor = new Vector3( 0.2f, 0.6f, 0.9f );
//        Vector3 triangleColor = new Vector3( 0.9f, 0.4f, 0.2f );

//        for ( int i = 0; i < 60; i++ )
//        {
//            Vector3 normal = Vector3.Normalize( V[ i ] );
//            Vector3 color = ( i < 12 ) ? pentagonColor : triangleColor;

//            vertices[ i ] = new Vertex( V[ i ], normal, color );
//        }

//        return vertices;
//    }

//    static uint[] CreateIndices()
//    {
//        var indices = new List<uint>();

//        var pentagons = new[]
//        {
//        new[] { 0, 12, 48, 28, 36 }, new[] { 1, 13, 49, 29, 37 },
//        new[] { 2, 14, 50, 30, 38 }, new[] { 3, 15, 51, 31, 39 },
//        new[] { 4, 17, 53, 32, 40 }, new[] { 5, 16, 52, 33, 41 },
//        new[] { 6, 19, 55, 34, 42 }, new[] { 7, 18, 54, 35, 43 },
//        new[] { 8, 22, 58, 24, 44 }, new[] { 9, 23, 59, 25, 45 },
//        new[] { 10, 20, 56, 26, 46 }, new[] { 11, 21, 57, 27, 47 }
//    };

//        foreach ( var p in pentagons )
//        {
//            for ( int i = 1; i < 4; i++ )
//            {
//                indices.Add( ( uint )p[ 0 ] );
//                indices.Add( ( uint )p[ i ] );
//                indices.Add( ( uint )p[ i + 1 ] );
//            }
//        }

//        var triangles = new[]
//        {
//        new[] { 0, 14, 2 }, new[] { 1, 15, 3 }, new[] { 2, 12, 0 }, new[] { 3, 13, 1 },
//        new[] { 4, 16, 5 }, new[] { 5, 17, 4 }, new[] { 6, 18, 7 }, new[] { 7, 19, 6 },
//        new[] { 8, 21, 11 }, new[] { 9, 20, 10 }, new[] { 10, 23, 9 }, new[] { 11, 22, 8 },
//        new[] { 12, 56, 48 }, new[] { 13, 57, 49 }, new[] { 14, 58, 50 }, new[] { 15, 59, 51 },
//        new[] { 16, 48, 52 }, new[] { 17, 49, 53 }, new[] { 18, 50, 54 }, new[] { 19, 51, 55 },
//        new[] { 20, 52, 56 }, new[] { 21, 53, 57 }, new[] { 22, 54, 58 }, new[] { 23, 55, 59 },
//        new[] { 24, 36, 44 }, new[] { 25, 37, 45 }, new[] { 26, 38, 46 }, new[] { 27, 39, 47 },
//        new[] { 28, 40, 36 }, new[] { 29, 41, 37 }, new[] { 30, 42, 38 }, new[] { 31, 43, 39 },
//        new[] { 32, 44, 40 }, new[] { 33, 45, 41 }, new[] { 34, 46, 42 }, new[] { 35, 47, 43 },
//        new[] { 36, 24, 0 }, new[] { 37, 25, 1 }, new[] { 38, 26, 2 }, new[] { 39, 27, 3 },
//        new[] { 40, 28, 4 }, new[] { 41, 29, 5 }, new[] { 42, 30, 6 }, new[] { 43, 31, 7 },
//        new[] { 44, 32, 8 }, new[] { 45, 33, 9 }, new[] { 46, 34, 10 }, new[] { 47, 35, 11 },
//        new[] { 48, 16, 28 }, new[] { 49, 17, 29 }, new[] { 50, 18, 30 }, new[] { 51, 19, 31 },
//        new[] { 52, 20, 33 }, new[] { 53, 21, 32 }, new[] { 54, 22, 35 }, new[] { 55, 23, 34 },
//        new[] { 56, 12, 26 }, new[] { 57, 13, 27 }, new[] { 58, 14, 24 }, new[] { 59, 15, 25 },
//        new[] { 24, 14, 0 }, new[] { 25, 15, 1 }, new[] { 26, 12, 2 }, new[] { 27, 13, 3 },
//        new[] { 28, 16, 4 }, new[] { 29, 17, 5 }, new[] { 30, 18, 6 }, new[] { 31, 19, 7 },
//        new[] { 32, 21, 8 }, new[] { 33, 20, 9 }, new[] { 34, 23, 10 }, new[] { 35, 22, 11 },
//        new[] { 36, 40, 44 }, new[] { 37, 41, 45 }, new[] { 38, 42, 46 }, new[] { 39, 43, 47 },
//        new[] { 48, 56, 52 }, new[] { 49, 57, 53 }, new[] { 50, 58, 54 }, new[] { 51, 59, 55 }
//    };

//        foreach ( var t in triangles )
//        {
//            indices.Add( ( uint )t[ 0 ] );
//            indices.Add( ( uint )t[ 1 ] );
//            indices.Add( ( uint )t[ 2 ] );
//        }

//        return indices.ToArray();
//    }
//}