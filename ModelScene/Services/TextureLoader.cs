using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Task1.Services;

public static class TextureLoader
{
    public static uint LoadTexture( GL gl, string filePath )
    {
        return LoadTextureInternal(
            gl,
            filePath,
            TextureMinFilter.Linear,
            TextureMagFilter.Linear );
    }

    private static uint LoadTextureInternal(
        GL gl,
        string filePath,
        TextureMinFilter minFilter,
        TextureMagFilter magFilter )
    {
        if ( !File.Exists( filePath ) )
        {
            throw new FileNotFoundException( "Texture file not found: " + filePath );
        }

        using Image<Rgba32> image = Image.Load<Rgba32>( filePath );
        image.Mutate( x => x.Flip( FlipMode.Vertical ) );

        uint textureId = gl.GenTexture();
        gl.BindTexture( TextureTarget.Texture2D, textureId );

        byte[] pixelData = new byte[ image.Width * image.Height * 4 ];
        image.CopyPixelDataTo( pixelData );

        unsafe
        {
            fixed ( byte* pData = pixelData )
            {
                gl.TexImage2D(
                    GLEnum.Texture2D,
                    0,
                    ( int )GLEnum.Rgba,
                    ( uint )image.Width,
                    ( uint )image.Height,
                    0,
                    GLEnum.Rgba, 
                    GLEnum.UnsignedByte,
                    pData );
            }
        }

        SetupTextureSampling( gl, minFilter, magFilter );
        return textureId;
    }

    private static void SetupTextureSampling( GL gl, TextureMinFilter minFilter, TextureMagFilter magFilter )
    {
        gl.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, ( int )minFilter );
        gl.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ( int )magFilter );
        gl.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureWrapS, ( int )TextureWrapMode.Repeat );
        gl.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureWrapT, ( int )TextureWrapMode.Repeat );
    }
}