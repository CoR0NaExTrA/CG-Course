namespace CGCourse;

internal sealed class SceneGeometry
{
    public required float[] AxesVertices { get; init; }
    public required float[] BezierVertices { get; init; }
    public required float[] DashedControlVertices { get; init; }
    public required float[] ControlPointVertices { get; init; }

    public static SceneGeometry BuildDefault() => new()
    {
        AxesVertices = GeometryBuilder.BuildAxesWithTicksAndArrows(),
        BezierVertices = GeometryBuilder.BuildBezierPolyline(SceneConfig.ControlPoints, segmentCount: 350),
        DashedControlVertices = GeometryBuilder.BuildDashedControlPolygon(SceneConfig.ControlPoints, dashLength: 0.75f, gapLength: 0.45f),
        ControlPointVertices = GeometryBuilder.BuildControlPoints(SceneConfig.ControlPoints)
    };
}
