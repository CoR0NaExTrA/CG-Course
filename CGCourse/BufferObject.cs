using OpenTK.Graphics.OpenGL4;

namespace CGCourse;

internal sealed class BufferObject : IDisposable
{
    public int Handle { get; }

    public BufferObject(float[] data)
    {
        Handle = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
        GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StreamDraw);
    }

    public void Dispose()
    {
        if (Handle != 0)
        {
            GL.DeleteBuffer(Handle);
        }
    }
}
