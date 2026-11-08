using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace MobiusStrip;

class RenderContext
{
    public GL GL { get; }
    private readonly IWindow _window;

    public RenderContext( IWindow window )
    {
        _window = window;
        GL = GL.GetApi( window );
    }

    public void Initialize()
    {
        GL.Enable( GLEnum.DepthTest );
        GL.Disable( GLEnum.CullFace );
    }

    public void Clear()
    {
        GL.ClearColor( 0.08f, 0.08f, 0.1f, 1 );
        GL.Clear( ( uint )( ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit ) );
    }
}