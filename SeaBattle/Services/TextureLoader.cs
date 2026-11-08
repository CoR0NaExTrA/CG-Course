using Silk.NET.OpenGL;
using StbImageSharp;

namespace Battleship3D.Services;

public static class TextureLoader
{
    public static unsafe uint Load( GL gl, string path )
    {
        if ( !File.Exists( path ) )
        {
            throw new FileNotFoundException( "Texture file not found: " + path );
        }

        uint texture = gl.GenTexture();
        gl.BindTexture( TextureTarget.Texture2D, texture );

        using var stream = File.OpenRead( path );
        var image = ImageResult.FromStream( stream, ColorComponents.RedGreenBlueAlpha );

        fixed ( byte* data = image.Data )
        {
            gl.TexImage2D(
                TextureTarget.Texture2D,
                0,
                InternalFormat.Rgba,
                ( uint )image.Width,
                ( uint )image.Height,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                data );
        }

        gl.GenerateMipmap( TextureTarget.Texture2D );

        gl.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureWrapS, ( int )GLEnum.Repeat );
        gl.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureWrapT, ( int )GLEnum.Repeat );
        gl.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, ( int )GLEnum.LinearMipmapLinear );
        gl.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ( int )GLEnum.Linear );

        return texture;
    }
}
