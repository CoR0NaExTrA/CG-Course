using System.Numerics;

namespace MemoryTrainer.Rendering;

public class Camera
{
    public Vector3 Position { get; set; }
    public Vector3 Target { get; set; }
    public Vector3 Up { get; set; } = Vector3.UnitY;

    public float Fov { get; set; }
    public float Near { get; set; }
    public float Far { get; set; }

    public Matrix4x4 ViewMatrix => Matrix4x4.CreateLookAt( Position, Target, Up );
    public Matrix4x4 ProjectionMatrix { get; private set; }

    public Camera( Vector3 position, Vector3 target, Vector3 up,
                  float fov, float near, float far )
    {
        Position = position;
        Target = target;
        Up = up;
        Fov = fov;
        Near = near;
        Far = far;
        UpdateProjection( 16f / 9f );
    }

    public void UpdateProjection( float aspectRatio )
    {
        ProjectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(
            Fov, aspectRatio, Near, Far );
    }
}