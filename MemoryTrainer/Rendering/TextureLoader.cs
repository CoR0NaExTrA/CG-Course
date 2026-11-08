using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace MemoryTrainer.Rendering;

public class TextureLoader
{
    private readonly GL _gl;

    public TextureLoader( GL gl ) => _gl = gl;

    public uint Load( string path )
    {
        using var img = Image.Load<Rgba32>( path );

        uint tex = _gl.GenTexture();
        _gl.BindTexture( GLEnum.Texture2D, tex );

        var pixels = new byte[ img.Width * img.Height * 4 ];
        img.CopyPixelDataTo( pixels );

        unsafe
        {
            fixed ( void* p = pixels )
            {
                _gl.TexImage2D( GLEnum.Texture2D, 0, ( int )GLEnum.Rgba,
                    ( uint )img.Width, ( uint )img.Height, 0,
                    GLEnum.Rgba, GLEnum.UnsignedByte, p );
            }
        }

        _gl.GenerateMipmap( GLEnum.Texture2D );
        ConfigureTexture();

        return tex;
    }

    private void ConfigureTexture()
    {
        _gl.TexParameter( GLEnum.Texture2D, GLEnum.TextureWrapS, ( int )GLEnum.Repeat );
        _gl.TexParameter( GLEnum.Texture2D, GLEnum.TextureWrapT, ( int )GLEnum.Repeat );
        _gl.TexParameter( GLEnum.Texture2D, GLEnum.TextureMinFilter, ( int )GLEnum.LinearMipmapLinear );
        _gl.TexParameter( GLEnum.Texture2D, GLEnum.TextureMagFilter, ( int )GLEnum.Linear );
    }
}