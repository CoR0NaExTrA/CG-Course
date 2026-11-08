using Battleship3D.Core;
using Battleship3D.Models;
using Silk.NET.OpenGL;
using System.Numerics;
using Shader = Battleship3D.Shaders.Shader;

namespace SeaBattle.Services;

internal class Renderer
{
    private GL gl;
    private Shader shader;
    private Camera camera;

    public Renderer( GL gl, Camera camera )
    {
        this.gl = gl;
        this.camera = camera;

        gl.Enable( EnableCap.DepthTest );

        string vertex =
            File.ReadAllText( "D:\\Lab6CG\\SeaBattle\\Shaders\\shader.vert" );

        string fragment =
            File.ReadAllText( "D:\\Lab6CG\\SeaBattle\\Shaders\\shader.frag" );

        shader = new Shader( gl, vertex, fragment );
    }

    public void BeginFrame()
    {
        gl.ClearColor( 0.3f, 0.6f, 0.9f, 1f );
        gl.Clear( ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit );

        shader.Use();

        Matrix4x4 projection = Matrix4x4.CreatePerspectiveFieldOfView(
            MathF.PI / 4f,
            1280f / 720f,
            0.1f,
            500f );

        Matrix4x4 view = camera.GetViewMatrix();

        shader.SetMatrix4( "projection", projection );
        shader.SetMatrix4( "view", view );

        shader.SetVector3( "lightPos", new Vector3( 0, 50, 20 ) );
        shader.SetVector3( "viewPos", camera.Position );
    }

    public void DrawObject( GameObject obj )
    {
        shader.SetMatrix4( "model", obj.GetModelMatrix() );

        gl.BindTexture( TextureTarget.Texture2D, obj.Texture );

        obj.Mesh.Draw();
    }

    public void DrawWater(WaterMesh water, uint texture)
    {
        Matrix4x4 waterModel =
            Matrix4x4.CreateTranslation( 0, -2f, 0 );

        shader.SetMatrix4( "model", waterModel );

        gl.BindTexture( TextureTarget.Texture2D, texture );

        water.Draw();
    }
}
