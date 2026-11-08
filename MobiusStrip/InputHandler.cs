using Silk.NET.Input;
using Silk.NET.Windowing;
using System.Numerics;

namespace MobiusStrip;

class InputHandler
{
    private readonly IWindow _window;
    private readonly Camera _camera;

    private IInputContext _input;
    private IMouse _mouse;

    private Vector2 _lastMouse;
    private bool _dragging;

    public InputHandler( IWindow window, Camera camera )
    {
        _window = window;
        _camera = camera;
    }

    public void Initialize()
    {
        _input = _window.CreateInput();
        _mouse = _input.Mice[ 0 ];

        _mouse.MouseDown += OnMouseDown;
        _mouse.MouseUp += OnMouseUp;
        _mouse.MouseMove += OnMouseMove;
    }

    private void OnMouseDown( IMouse m, MouseButton b )
    {
        if ( b == MouseButton.Left )
        {
            _dragging = true;
            _lastMouse = m.Position;
        }
    }

    private void OnMouseUp( IMouse m, MouseButton b )
    {
        if ( b == MouseButton.Left )
            _dragging = false;
    }

    private void OnMouseMove( IMouse m, Vector2 pos )
    {
        if ( !_dragging )
            return;

        var delta = pos - _lastMouse;
        _camera.Rotate( delta.X, delta.Y );
        _lastMouse = pos;
    }
}