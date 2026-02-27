using OpenTK.Mathematics;

namespace CGCourse;

internal static class SceneConfig
{
    public const float BaseHalfRange = 12f;

    public static readonly Vector2[] ControlPoints =
    {
        new(-8f, -6f),
        new(-4f, 8f),
        new(4f, -8f),
        new(8f, 6f)
    };

    public static readonly Vector3 AxesColor = new(0.10f, 0.10f, 0.10f);
    public static readonly Vector3 GraphColor = new(0.20f, 0.35f, 0.95f);
    public static readonly Vector3 ControlPolygonColor = new(0.85f, 0.25f, 0.25f);
    public static readonly Vector3 ControlPointColor = new(0.05f, 0.60f, 0.20f);
}
