using System.Numerics;

namespace MobiusStrip;

class Camera
{
    public float Yaw { get; set; } = 0f;
    public float Pitch { get; set; } = 0f;

    private const float RotationSpeed = 0.01f;

    public void Rotate( float deltaX, float deltaY )
    {
        Yaw += deltaX * RotationSpeed;
        Pitch += deltaY * RotationSpeed;
    }

    public Matrix4x4 ModelMatrix =>
        Matrix4x4.CreateRotationY( Yaw ) * Matrix4x4.CreateRotationX( Pitch );

    public Matrix4x4 ViewMatrix =>
        Matrix4x4.CreateTranslation( 0, 0, -4 );
}