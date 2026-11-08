using Silk.NET.OpenGL;
using StbImageSharp;

namespace Maze3D.Rendering;

public class TextureManager
{
    private readonly GL gl;
    private readonly Dictionary<string, uint> textures = new();

    public TextureManager(GL gl)
    {
        this.gl = gl;
    }

    public uint LoadTexture(string filePath)
    {
        if (textures.TryGetValue(filePath, out var existing))
            return existing;

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"⚠️ Текстура не найдена: {filePath}");
            return CreateWhiteTexture();
        }

        using var stream = File.OpenRead(filePath);
        var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

        uint texture = gl.GenTexture();
        gl.BindTexture(GLEnum.Texture2D, texture);

        gl.TexImage2D(GLEnum.Texture2D, 0, InternalFormat.Rgba,
            (uint)image.Width, (uint)image.Height, 0,
            GLEnum.Rgba, GLEnum.UnsignedByte, image.Data);

        gl.TexParameter(GLEnum.Texture2D, GLEnum.TextureWrapS, (int)GLEnum.Repeat);
        gl.TexParameter(GLEnum.Texture2D, GLEnum.TextureWrapT, (int)GLEnum.Repeat);
        gl.TexParameter(GLEnum.Texture2D, GLEnum.TextureMinFilter, (int)GLEnum.LinearMipmapLinear);
        gl.TexParameter(GLEnum.Texture2D, GLEnum.TextureMagFilter, (int)GLEnum.Linear);
        gl.GenerateMipmap(GLEnum.Texture2D);

        gl.BindTexture(GLEnum.Texture2D, 0);
        textures[filePath] = texture;

        Console.WriteLine($"✓ Загружена текстура: {filePath} ({image.Width}×{image.Height})");
        return texture;
    }

    public void BindTexture(uint texture, int textureUnit = 0)
    {
        gl.ActiveTexture(GLEnum.Texture0 + textureUnit);
        gl.BindTexture(GLEnum.Texture2D, texture);
    }

    private uint CreateWhiteTexture()
    {
        uint texture = gl.GenTexture();
        gl.BindTexture(GLEnum.Texture2D, texture);

        byte[] white = { 255, 255, 255, 255 };
        unsafe
        {
            fixed (byte* p = white)
            {
                gl.TexImage2D(GLEnum.Texture2D, 0, InternalFormat.Rgba,
                    1, 1, 0, GLEnum.Rgba, GLEnum.UnsignedByte, p);
            }
        }
        gl.TexParameter(GLEnum.Texture2D, GLEnum.TextureMinFilter, (int)GLEnum.Linear);
        gl.TexParameter(GLEnum.Texture2D, GLEnum.TextureMagFilter, (int)GLEnum.Linear);
        return texture;
    }

    public void Cleanup()
    {
        foreach (var tex in textures.Values)
            gl.DeleteTexture(tex);
        textures.Clear();
    }
}