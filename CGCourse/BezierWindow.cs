using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace CGCourse;

internal sealed class BezierWindow : GameWindow
{
    private Matrix4 _projection;
    private BezierRenderer? _renderer;

    public BezierWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.95f, 0.95f, 0.95f, 1f);
        GL.Enable(EnableCap.Multisample);

        _renderer = new BezierRenderer(SceneGeometry.BuildDefault());
        _projection = ProjectionHelper.CreatePreservingAspectProjection(Size.X, Size.Y);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
        _projection = ProjectionHelper.CreatePreservingAspectProjection(e.Width, e.Height);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        _renderer?.Render(_projection);
        SwapBuffers();
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        _renderer?.Dispose();
    }
}
