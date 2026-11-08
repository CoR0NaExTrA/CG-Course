namespace MobiusStrip;

class Program
{
    static void Main()
    {
        var app = new WindowApp();
        app.Run();
    }
}


//using Silk.NET.OpenGL;
//using Silk.NET.Windowing;
//using Silk.NET.Input;
//using Silk.NET.Maths;
//using System.Numerics;

//namespace MobiusStrip;

//class Program
//{
//    static IWindow window;
//    static GL gl;

//    static IInputContext input;
//    static IMouse mouse;

//    static uint vao;
//    static uint vbo;
//    static uint ebo;

//    static uint shader;

//    static float yaw = 0;
//    static float pitch = 0;

//    static Vector2 lastMouse;
//    static bool dragging = false;

//    static int indexCount;

//    static void Main()
//    {
//        var options = WindowOptions.Default;
//        options.Size = new Vector2D<int>( 1000, 700 );
//        options.Title = "Mobius Strip";
//        options.API = new GraphicsAPI( ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Default, new APIVersion( 3, 3 ) );

//        window = Window.Create( options );

//        window.Load += OnLoad;
//        window.Render += OnRender;

//        window.Run();
//    }

//    static unsafe void OnLoad()
//    {
//        gl = GL.GetApi( window );

//        gl.Enable( GLEnum.DepthTest );
//        gl.Disable( GLEnum.CullFace );

//        input = window.CreateInput();
//        mouse = input.Mice[ 0 ];

//        mouse.MouseDown += MouseDown;
//        mouse.MouseUp += MouseUp;
//        mouse.MouseMove += MouseMove;

//        var mesh = GenerateMobius( 200, 30 );

//        vao = gl.GenVertexArray();
//        vbo = gl.GenBuffer();
//        ebo = gl.GenBuffer();

//        gl.BindVertexArray( vao );

//        gl.BindBuffer( GLEnum.ArrayBuffer, vbo );

//        fixed ( float* v = mesh.vertices )
//            gl.BufferData( GLEnum.ArrayBuffer,
//                ( nuint )( mesh.vertices.Length * sizeof( float ) ),
//                v,
//                GLEnum.StaticDraw );

//        gl.BindBuffer( GLEnum.ElementArrayBuffer, ebo );

//        fixed ( uint* i = mesh.indices )
//            gl.BufferData( GLEnum.ElementArrayBuffer,
//                ( nuint )( mesh.indices.Length * sizeof( uint ) ),
//                i,
//                GLEnum.StaticDraw );

//        int stride = 6 * sizeof( float );

//        gl.VertexAttribPointer( 0, 3, GLEnum.Float, false, ( uint )stride, ( void* )0 );
//        gl.EnableVertexAttribArray( 0 );

//        gl.VertexAttribPointer( 1, 3, GLEnum.Float, false, ( uint )stride, ( void* )( 3 * sizeof( float ) ) );
//        gl.EnableVertexAttribArray( 1 );

//        shader = CreateShader();
//    }

//    static void MouseDown( IMouse m, MouseButton b )
//    {
//        if ( b == MouseButton.Left )
//        {
//            dragging = true;
//            lastMouse = m.Position;
//        }
//    }

//    static void MouseUp( IMouse m, MouseButton b )
//    {
//        if ( b == MouseButton.Left )
//            dragging = false;
//    }

//    static void MouseMove( IMouse m, Vector2 pos )
//    {
//        if ( !dragging )
//            return;

//        Vector2 delta = pos - lastMouse;

//        yaw += delta.X * 0.01f;
//        pitch += delta.Y * 0.01f;

//        lastMouse = pos;
//    }

//    static unsafe void OnRender( double delta )
//    {
//        gl.ClearColor( 0.08f, 0.08f, 0.1f, 1 );
//        gl.Clear( ( uint )( ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit ) );

//        gl.UseProgram( shader );

//        Matrix4x4 model =
//            Matrix4x4.CreateRotationY( yaw ) *
//            Matrix4x4.CreateRotationX( pitch );

//        Matrix4x4 view =
//            Matrix4x4.CreateTranslation( 0, 0, -4 );

//        float aspect = window.Size.X / ( float )window.Size.Y;

//        Matrix4x4 projection =
//            Matrix4x4.CreatePerspectiveFieldOfView(
//                MathF.PI / 4,
//                aspect,
//                0.1f,
//                100 );

//        int loc;

//        loc = gl.GetUniformLocation( shader, "model" );
//        gl.UniformMatrix4( loc, 1, false, ( float* )&model );

//        loc = gl.GetUniformLocation( shader, "view" );
//        gl.UniformMatrix4( loc, 1, false, ( float* )&view );

//        loc = gl.GetUniformLocation( shader, "projection" );
//        gl.UniformMatrix4( loc, 1, false, ( float* )&projection );

//        Vector3 lightPos = new( 3, 3, 3 );

//        loc = gl.GetUniformLocation( shader, "lightPos" );
//        gl.Uniform3( loc, lightPos );

//        gl.BindVertexArray( vao );

//        gl.DrawElements(
//            GLEnum.Triangles,
//            ( uint )indexCount,
//            GLEnum.UnsignedInt,
//            null );
//    }

//    static (float[] vertices, uint[] indices) GenerateMobius( int segU, int segV )
//    {
//        List<float> v = new();
//        List<uint> ind = new();

//        float width = 0.3f;

//        for ( int i = 0; i <= segU; i++ )
//        {
//            float u = ( float )i / segU * MathF.PI * 2;

//            for ( int j = 0; j <= segV; j++ )
//            {
//                float vv = ( float )j / segV * 2 - 1;
//                vv *= width;

//                float x = ( 1 + vv * MathF.Cos( u / 2 ) ) * MathF.Cos( u );
//                float y = ( 1 + vv * MathF.Cos( u / 2 ) ) * MathF.Sin( u );
//                float z = vv * MathF.Sin( u / 2 );

//                Vector3 pos = new( x, y, z );

//                Vector3 du = new(
//                    -MathF.Sin( u ) * ( 1 + vv * MathF.Cos( u / 2 ) ) - vv * 0.5f * MathF.Sin( u / 2 ) * MathF.Cos( u ),
//                    MathF.Cos( u ) * ( 1 + vv * MathF.Cos( u / 2 ) ) - vv * 0.5f * MathF.Sin( u / 2 ) * MathF.Sin( u ),
//                    vv * 0.5f * MathF.Cos( u / 2 )
//                );

//                Vector3 dv = new(
//                    MathF.Cos( u / 2 ) * MathF.Cos( u ),
//                    MathF.Cos( u / 2 ) * MathF.Sin( u ),
//                    MathF.Sin( u / 2 )
//                );

//                Vector3 normal = Vector3.Normalize( Vector3.Cross( du, dv ) );

//                v.Add( pos.X );
//                v.Add( pos.Y );
//                v.Add( pos.Z );

//                v.Add( normal.X );
//                v.Add( normal.Y );
//                v.Add( normal.Z );
//            }
//        }

//        int stride = 6;

//        for ( int j = 0; j <= segV; j++ )
//        {
//            int first = j * stride;
//            int last = ( segU * ( segV + 1 ) + j ) * stride;

//            v[ last + 3 ] = v[ first + 3 ];
//            v[ last + 4 ] = v[ first + 4 ];
//            v[ last + 5 ] = v[ first + 5 ];
//        }

//        for ( uint i = 0; i < segU; i++ )
//        {
//            for ( uint j = 0; j < segV; j++ )
//            {
//                uint a = i * ( uint )( segV + 1 ) + j;
//                uint b = a + ( uint )( segV + 1 );

//                ind.Add( a );
//                ind.Add( b );
//                ind.Add( a + 1 );

//                ind.Add( b );
//                ind.Add( b + 1 );
//                ind.Add( a + 1 );
//            }
//        }

//        indexCount = ind.Count;

//        return (v.ToArray(), ind.ToArray());
//    }

//    static uint CreateShader()
//    {
//        string vert = @"
//        #version 330 core

//        layout (location=0) in vec3 aPos;
//        layout (location=1) in vec3 aNormal;

//        out vec3 FragPos;
//        out vec3 Normal;

//        uniform mat4 model;
//        uniform mat4 view;
//        uniform mat4 projection;

//        void main()
//        {
//            FragPos = vec3(model*vec4(aPos,1.0));
//            Normal = mat3(transpose(inverse(model))) * aNormal;

//            gl_Position = projection * view * vec4(FragPos,1.0);
//        }";

//        string frag = @"
//        #version 330 core

//        in vec3 FragPos;
//        in vec3 Normal;

//        out vec4 FragColor;

//        uniform vec3 lightPos;

//        void main()
//        {
//            vec3 lightColor = vec3(1.0);

//            vec3 norm = normalize(Normal);
//            vec3 lightDir = normalize(lightPos-FragPos);

//            float diff = max(dot(norm,lightDir),0.0);

//            vec3 color = vec3(0.8,0.3,0.6);

//            vec3 result = (0.2 + diff)*color;

//            FragColor = vec4(result,1.0);
//        }";

//        uint vs = gl.CreateShader( GLEnum.VertexShader );
//        gl.ShaderSource( vs, vert );
//        gl.CompileShader( vs );

//        uint fs = gl.CreateShader( GLEnum.FragmentShader );
//        gl.ShaderSource( fs, frag );
//        gl.CompileShader( fs );

//        uint prog = gl.CreateProgram();

//        gl.AttachShader( prog, vs );
//        gl.AttachShader( prog, fs );

//        gl.LinkProgram( prog );

//        gl.DeleteShader( vs );
//        gl.DeleteShader( fs );

//        return prog;
//    }
//}