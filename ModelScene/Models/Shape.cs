using Assimp;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Task1.Services;
using Task1.Shaders;
using PrimitiveType = Silk.NET.OpenGL.PrimitiveType;

namespace Task1.Models;

public class Shape
{
    private Scene _scene = null!;
    private readonly Dictionary<Mesh, uint> _meshVbos = [];
    private readonly Dictionary<Material, uint> _textureIds = [];
    private readonly Dictionary<Mesh, int> _meshVertexCounts = [];
    private readonly GL _gl;

    private float _x, _y, _z;
    private readonly float _scale;
    private readonly bool _isInverse;
    private uint _forcedTextureId = uint.MaxValue;
    private string _modelPath = null!;

    public float X
    {
        get => _x;
        set => _x = value;
    }

    public float Y
    {
        get => _y;
        set => _y = value;
    }

    public float Z
    {
        get => _z;
        set => _z = value;
    }

    public Shape( GL gl, float x, float y, float z, float scale, bool isInverse = false )
    {
        _gl = gl;
        _x = x;
        _y = y;
        _z = z;
        _scale = scale;
        _isInverse = isInverse;
    }

    public void LoadPicture( string path )
    {
        AssimpContext context = new AssimpContext();
        _scene = context.ImportFile( path,
            PostProcessSteps.Triangulate |
            PostProcessSteps.GenerateNormals |
            PostProcessSteps.CalculateTangentSpace |
            PostProcessSteps.FlipUVs );

        _modelPath = path;
        string? modelDirectory = Path.GetDirectoryName( path );

        if ( path.Contains( "Board.3ds" ) )
        {
            foreach ( Material material in _scene.Materials )
            {
                LoadMaterialTexture( material, modelDirectory! );
            }
        }

        foreach ( Mesh mesh in _scene.Meshes )
        {
            ProcessMesh( mesh );
        }
    }

    public void SetTexture( uint textureId ) => _forcedTextureId = textureId;

    public void Paint( ChessShader shader )
    {
        Matrix4X4<float> model = Matrix4X4.CreateTranslation( _x, _y, _z );
        shader.SetMatrix4( "model", model );

        foreach ( Mesh mesh in _scene.Meshes )
        {
            uint vbo = _meshVbos[ mesh ];
            int vertexCount = _meshVertexCounts[ mesh ];

            _gl.BindBuffer( GLEnum.ArrayBuffer, vbo );
            ConfigureVertexAttributes();

            Material material = _scene.Materials[ mesh.MaterialIndex ];
            SetupTextureAndColor( shader, material );

            _gl.DrawArrays( PrimitiveType.Triangles, 0, ( uint )vertexCount );
        }
    }

    private void ProcessMesh( Mesh mesh )
    {
        Vector3D<float>[] vertices = mesh.Vertices
            .Select( v => new Vector3D<float>( v.X, v.Y, v.Z ) )
            .ToArray();

        Vector3D<float>[] normals = mesh.Normals
            .Select( n => new Vector3D<float>( n.X, n.Y, n.Z ) )
            .ToArray();

        Vector2D<float>[] texCoords = mesh.TextureCoordinateChannels[ 0 ]?
            .Select( t => new Vector2D<float>( t.X, t.Y ) )
            .ToArray() ?? Array.Empty<Vector2D<float>>();

        int[] indices = mesh.GetIndices();
        List<float> vertexData = BuildVertexData( vertices, normals, texCoords, indices );

        uint vbo = CreateBuffer( vertexData );
        _meshVbos[ mesh ] = vbo;
        _meshVertexCounts[ mesh ] = vertexData.Count / 8;
    }

    private List<float> BuildVertexData( Vector3D<float>[] vertices, Vector3D<float>[] normals,
        Vector2D<float>[] texCoords, int[] indices )
    {
        List<float> vertexData = new();
        for ( int i = 0; i < indices.Length; i++ )
        {
            int index = indices[ i ];
            vertexData.Add( vertices[ index ].X * _scale );
            vertexData.Add( vertices[ index ].Y * _scale );
            vertexData.Add( vertices[ index ].Z * _scale );
            vertexData.Add( normals[ index ].X );
            vertexData.Add( normals[ index ].Y );
            vertexData.Add( normals[ index ].Z );
            vertexData.Add( texCoords.Length > index ? texCoords[ index ].X : 0f );
            vertexData.Add( texCoords.Length > index ? texCoords[ index ].Y : 0f );
        }
        return vertexData;
    }

    private uint CreateBuffer( List<float> vertexData )
    {
        uint vbo = _gl.GenBuffer();
        _gl.BindBuffer( GLEnum.ArrayBuffer, vbo );
        _gl.BufferData( GLEnum.ArrayBuffer,
        ( nuint )( vertexData.Count * sizeof( float ) ),
                vertexData.ToArray(), BufferUsageARB.DynamicDraw );
        return vbo;
    }

    private unsafe void ConfigureVertexAttributes()
    {
        const int stride = 8 * sizeof( float );

        _gl.VertexAttribPointer( 0, 3, VertexAttribPointerType.Float, false, stride, null );
        _gl.EnableVertexAttribArray( 0 );

        _gl.VertexAttribPointer( 1, 3, VertexAttribPointerType.Float, false, stride, ( void* )( 3 * sizeof( float ) ) );
        _gl.EnableVertexAttribArray( 1 );

        _gl.VertexAttribPointer( 2, 2, VertexAttribPointerType.Float, false, stride, ( void* )( 6 * sizeof( float ) ) );
        _gl.EnableVertexAttribArray( 2 );
    }

    private void SetColorToShader( ChessShader shader, Color4D color, string name )
    {
        float r = _isInverse ? 1 - color.R : color.R;
        float g = _isInverse ? 1 - color.G : color.G;
        float b = _isInverse ? 1 - color.B : color.B;
        shader.SetVector3( name, new Vector3D<float>( r, g, b ) );
    }

    private void SetupTextureAndColor( ChessShader shader, Material material )
    {
        bool applyForced = _forcedTextureId != uint.MaxValue && !_modelPath.Contains( "Board.3ds" );

        if ( applyForced )
        {
            _gl.ActiveTexture( TextureUnit.Texture0 );
            _gl.BindTexture( TextureTarget.Texture2D, _forcedTextureId );
            shader.SetInt( "mainTexture", 0 );
            shader.SetBool( "useTexture", true );
        }
        else if ( _textureIds.ContainsKey( material ) && _textureIds[ material ] != 0 )
        {
            _gl.ActiveTexture( TextureUnit.Texture0 );
            _gl.BindTexture( TextureTarget.Texture2D, _textureIds[ material ] );
            shader.SetInt( "mainTexture", 0 );
            shader.SetBool( "useTexture", true );
        }
        else
        {
            shader.SetBool( "useTexture", false );
        }

        SetColorToShader( shader, material.ColorDiffuse, "diffuseColor" );
    }

    private void LoadMaterialTexture( Material material, string modelDirectory )
    {
        if ( material.HasTextureDiffuse )
        {
            string texturePath = material.TextureDiffuse.FilePath;
            string fullPath = Path.Combine( modelDirectory, texturePath );

            if ( !File.Exists( fullPath ) )
                fullPath = Path.Combine( modelDirectory, Path.GetFileName( texturePath ) );
            if ( !File.Exists( fullPath ) )
                fullPath = texturePath;

            if ( File.Exists( fullPath ) )
            {
                uint textureId = TextureLoader.LoadTexture( _gl, fullPath );
                _textureIds[ material ] = textureId;
            }
        }
    }
}