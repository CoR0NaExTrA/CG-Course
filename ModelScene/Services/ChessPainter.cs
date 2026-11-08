using Silk.NET.OpenGL;
using Task1.Models;
using Task1.Models.Data;
using Task1.Models.Enums;
using Task1.Shaders;

namespace Task1.Services;

public class ChessPainter
{
    private const float PieceScale = 0.09f;
    private readonly Dictionary<ChessPiece, Shape> _pieceShapes = new();
    private readonly Shape _boardShape;
    private bool _initialized;
    private uint _whiteTextureId;
    private uint _blackTextureId;
    private readonly GL _gl;
    private string _shapesDir = "";

    public ChessBoard Board { get; }

    public ChessPainter( GL gl, ChessMove[] moves )
    {
        _gl = gl;
        Board = new ChessBoard();
        _boardShape = new Shape( gl, 0f, 0f, 0f, 4f );
        Initialize( moves );
    }

    public void Update( float deltaTime )
    {
        if ( !_initialized )
            return;
        Board.Update( deltaTime );

        foreach ( ChessPiece piece in Board.GetAllPieces() )
        {
            if ( _pieceShapes.TryGetValue( piece, out Shape? shape ) )
            {
                shape.X = piece.Position.X;
                shape.Y = piece.Position.Y;
                shape.Z = piece.Position.Z;
            }
        }
    }

    public void Paint( ChessShader shader )
    {
        if ( !_initialized )
            return;
        _boardShape.Paint( shader );
        foreach ( ChessPiece piece in Board.GetAllPieces() )
        {
            if ( _pieceShapes.TryGetValue( piece, out Shape? shape ) )
            {
                shape.Paint( shader );
            }
        }
    }

    private void Initialize( ChessMove[] moves )
    {
        if ( _initialized )
            return;

        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        _shapesDir = Path.Combine( baseDir, "shapes" );
        string texturesDir = Path.Combine( baseDir, "textures" );

        _whiteTextureId = TextureLoader.LoadTexture( _gl, Path.Combine( texturesDir, "WhitePiece.jpg" ) );
        _blackTextureId = TextureLoader.LoadTexture( _gl, Path.Combine( texturesDir, "BlackPiece.jpg" ) );

        Board.Initialize();
        Board.SetupGameSequence( moves );

        _boardShape.LoadPicture( Path.Combine( _shapesDir, "Board.3ds" ) );
        _boardShape.SetTexture( _whiteTextureId );

        BuildPieceShapes();
        _initialized = true;
    }

    private void BuildPieceShapes()
    {
        foreach ( ChessPiece piece in Board.GetAllPieces() )
        {
            Shape shape = new Shape( _gl, piece.Position.X, piece.Position.Y, piece.Position.Z, PieceScale );
            string modelFile = piece.GetModelPath();
            string modelPath = Path.Combine( _shapesDir, Path.GetFileName( modelFile ) );
            shape.LoadPicture( modelPath );
            shape.SetTexture( piece.Color == PieceColor.White ? _whiteTextureId : _blackTextureId );
            _pieceShapes[ piece ] = shape;
        }
    }
}