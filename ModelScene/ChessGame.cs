using System.Numerics;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Task1.Models.Data;
using Task1.Services;
using Task1.Shaders;

namespace Task1;

public class ChessGame
{
    private readonly IWindow _window;
    private GL _gl = null!;
    private IInputContext _input = null!;

    private ChessShader _shader = null!;
    private readonly Matrix4X4<float> _model = Matrix4X4<float>.Identity;
    private Matrix4X4<float> _view = Matrix4X4<float>.Identity;
    private Matrix4X4<float> _projection = Matrix4X4<float>.Identity;

    private bool _isDragging;
    private Vector2 _lastMousePos;
    private Vector3D<float> _cameraPos = new( 0, 20f, 15f );

    private readonly Vector3D<float> _lightPos = new( 5.0f, 0.0f, 5.0f );
    private readonly Vector3D<float> _lightColor = new( 1.0f, 1.0f, 1.0f );
    private readonly float _ambientStrength = 0.3f;

    private ChessPainter _chessPainter = null!;

    public ChessGame( IWindow window ) => _window = window;

    public void OnLoad()
    {
        _gl = _window.CreateOpenGL();

        _input = _window.CreateInput();
        foreach ( var mouse in _input.Mice )
        {
            mouse.MouseDown += OnMouseDown;
            mouse.MouseUp += OnMouseUp;
            mouse.MouseMove += OnMouseMove;
        }

        _gl.ClearColor( 0.2f, 0.2f, 0.2f, 1.0f );
        _gl.Enable( EnableCap.DepthTest );

        uint vertexBufferObj = _gl.GenBuffer();
        _gl.BindBuffer( BufferTargetARB.ArrayBuffer, vertexBufferObj );
        uint vertexArrayObj = _gl.GenVertexArray();
        _gl.BindVertexArray( vertexArrayObj );

        _shader = new ChessShader( _gl );
        _chessPainter = new ChessPainter( _gl, GetChessMoves() );
        UpdateViewMatrix();
        CalculateProjectionMatrix();
    }

    public void OnResize( Vector2D<int> size )
    {
        _gl.Viewport( 0, 0, ( uint )size.X, ( uint )size.Y );
        CalculateProjectionMatrix();
    }

    public void OnUpdateFrame( double delta ) => _chessPainter.Update( ( float )delta );

    public void OnRenderFrame( double delta )
    {
        _gl.Clear( ( uint )( ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit ) );

        _shader.Use();
        _shader.SetMatrix4( "model", _model );
        _shader.SetMatrix4( "view", _view );
        _shader.SetMatrix4( "projection", _projection );
        _shader.SetVector3( "lightPos", _lightPos );
        _shader.SetVector3( "lightColor", _lightColor );
        _shader.SetFloat( "ambientStrength", _ambientStrength );

        _chessPainter.Paint( _shader );
    }

    public void OnMouseDown( IMouse mouse, MouseButton button)
    {
        if ( _isDragging || button != MouseButton.Left )
            return;
        _lastMousePos = mouse.Position;
        _isDragging = true;
    }

    public void OnMouseUp( IMouse mouse, MouseButton button )
    {
        if ( button != MouseButton.Left )
            return;
        _isDragging = false;
    }

    public void OnMouseMove( IMouse mouse, Vector2 delta )
    {
        if ( !_isDragging )
            return;
        float dx = ( _lastMousePos.X - mouse.Position.X ) / 20f;
        _lastMousePos = mouse.Position;
        _cameraPos = RotateVector( _cameraPos, dx );
        UpdateViewMatrix();
    }

    private void UpdateViewMatrix()
    {
        _view = Matrix4X4.CreateLookAt( _cameraPos, Vector3D<float>.Zero, Vector3D<float>.UnitZ );
    }

    private static Vector3D<float> RotateVector( Vector3D<float> vector, float angle )
    {
        float x = vector.X, y = vector.Y;
        float sin = MathF.Sin( angle ), cos = MathF.Cos( angle );
        return new Vector3D<float>( x * cos - y * sin, x * sin + y * cos, vector.Z );
    }

    private void CalculateProjectionMatrix()
    {
        float aspectRatio = _window.Size.X / ( float )_window.Size.Y;
        _projection = Matrix4X4.CreatePerspectiveFieldOfView(
            DegreesToRadians( 45f ), aspectRatio, 0.1f, 100f );
    }

    private static float DegreesToRadians( float deg ) => deg * MathF.PI / 180f;

    private static ChessMove[] GetChessMoves() => new[]
    {
        new ChessMove(5, 1, 5, 2),
        new ChessMove(4, 6, 4, 4),
        new ChessMove(6, 1, 6, 3),
        new ChessMove(3, 7, 7, 3)
    };
}