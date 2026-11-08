using MemoryTrainer.Core;
using MemoryTrainer.Rendering;
using Silk.NET.Input;
using Silk.NET.Windowing;

namespace MemoryTrainer.Input;

public class InputHandler
{
    private readonly IWindow _window;
    private readonly Camera _camera;
    private readonly Game _game;
    private readonly Raycaster _raycaster;

    private IInputContext _input = null!;
    private IMouse _mouse = null!;

    public InputHandler( IWindow window, Camera camera, Game game )
    {
        _window = window;
        _camera = camera;
        _game = game;
        _raycaster = new Raycaster();

        _window.Load += SetupInput;
    }

    private void SetupInput()
    {
        _input = _window.CreateInput();
        _mouse = _input.Mice[ 0 ];
        _mouse.MouseDown += OnMouseDown;
    }

    private void OnMouseDown( IMouse mouse, MouseButton button )
    {
        if ( button != MouseButton.Left )
            return;

        var ray = _raycaster.GetRay(
            mouse.Position.X,
            mouse.Position.Y,
            _camera,
            _window.Size );

        foreach ( var tile in _game.GetAllTiles() )
        {
            if ( tile.IsMatched )
                continue;

            if ( _raycaster.IntersectsTile( ray, tile, out _ ) )
            {
                _game.TrySelectTile( tile );
                break;
            }
        }
    }
}