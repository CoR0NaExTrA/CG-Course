using Silk.NET.Input;
using System.Numerics;

namespace SnubDodecahedron.Input;

public class CameraController
{
    public float Yaw = 0;
    public float Pitch = 0;
    public float Radius = 6f;

    private Vector2 lastMouse;
    private bool firstMove = true;

    public void Attach( IMouse mouse )
    {
        mouse.MouseMove += OnMouseMove;
    }

    private void OnMouseMove( IMouse mouse, Vector2 pos )
    {
        if ( firstMove )
        {
            lastMouse = pos;
            firstMove = false;
        }

        var delta = pos - lastMouse;
        lastMouse = pos;

        if ( mouse.IsButtonPressed( MouseButton.Left ) )
        {
            Yaw += delta.X * 0.01f;
            Pitch += delta.Y * 0.01f;
            Pitch = Math.Clamp( Pitch, -1.5f, 1.5f );
        }
    }

    public Vector3 GetPosition()
    {
        float x = Radius * MathF.Cos( Pitch ) * MathF.Cos( Yaw );
        float y = Radius * MathF.Sin( Pitch );
        float z = Radius * MathF.Cos( Pitch ) * MathF.Sin( Yaw );

        return new Vector3( x, y, z );
    }
}