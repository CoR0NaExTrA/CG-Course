using OpenTK.Mathematics;

namespace CGCourse;

internal static class GeometryBuilder
{
    public static float[] BuildAxesWithTicksAndArrows()
    {
        var vertices = new List<float>();

        AddLine(vertices, -SceneConfig.BaseHalfRange, 0f, SceneConfig.BaseHalfRange, 0f);
        AddLine(vertices, 0f, -SceneConfig.BaseHalfRange, 0f, SceneConfig.BaseHalfRange);

        const float arrowLength = 0.7f;
        const float arrowHalfWidth = 0.24f;

        AddLine(vertices, SceneConfig.BaseHalfRange, 0f, SceneConfig.BaseHalfRange - arrowLength, arrowHalfWidth);
        AddLine(vertices, SceneConfig.BaseHalfRange, 0f, SceneConfig.BaseHalfRange - arrowLength, -arrowHalfWidth);

        AddLine(vertices, 0f, SceneConfig.BaseHalfRange, arrowHalfWidth, SceneConfig.BaseHalfRange - arrowLength);
        AddLine(vertices, 0f, SceneConfig.BaseHalfRange, -arrowHalfWidth, SceneConfig.BaseHalfRange - arrowLength);

        const float tickHalfSize = 0.15f;

        for (var i = (int)-SceneConfig.BaseHalfRange; i <= (int)SceneConfig.BaseHalfRange; i++)
        {
            if (i == 0)
            {
                continue;
            }

            AddLine(vertices, i, -tickHalfSize, i, tickHalfSize);
            AddLine(vertices, -tickHalfSize, i, tickHalfSize, i);
        }

        return vertices.ToArray();
    }

    public static float[] BuildBezierPolyline(IReadOnlyList<Vector2> points, int segmentCount)
    {
        var vertices = new float[(segmentCount + 1) * 2];

        for (var i = 0; i <= segmentCount; i++)
        {
            var t = i / (float)segmentCount;
            var p = EvaluateCubicBezier(points, t);
            vertices[i * 2] = p.X;
            vertices[i * 2 + 1] = p.Y;
        }

        return vertices;
    }

    public static float[] BuildDashedControlPolygon(IReadOnlyList<Vector2> points, float dashLength, float gapLength)
    {
        var vertices = new List<float>();

        for (var i = 0; i < points.Count - 1; i++)
        {
            AddDashedSegment(vertices, points[i], points[i + 1], dashLength, gapLength);
        }

        return vertices.ToArray();
    }

    public static float[] BuildControlPoints(IReadOnlyList<Vector2> points) =>
        points.SelectMany(p => new[] { p.X, p.Y }).ToArray();

    private static void AddDashedSegment(List<float> output, Vector2 start, Vector2 end, float dashLength, float gapLength)
    {
        var direction = end - start;
        var length = direction.Length;

        if (length <= float.Epsilon)
        {
            return;
        }

        var unit = direction / length;
        var cursor = 0f;
        var draw = true;

        while (cursor < length)
        {
            var segmentSize = draw ? dashLength : gapLength;
            var next = MathF.Min(length, cursor + segmentSize);

            if (draw)
            {
                var p0 = start + unit * cursor;
                var p1 = start + unit * next;
                AddLine(output, p0.X, p0.Y, p1.X, p1.Y);
            }

            draw = !draw;
            cursor = next;
        }
    }

    private static Vector2 EvaluateCubicBezier(IReadOnlyList<Vector2> points, float t)
    {
        var oneMinusT = 1f - t;
        var oneMinusTSquared = oneMinusT * oneMinusT;
        var tSquared = t * t;

        return oneMinusT * oneMinusTSquared * points[0]
               + 3f * oneMinusTSquared * t * points[1]
               + 3f * oneMinusT * tSquared * points[2]
               + tSquared * t * points[3];
    }

    private static void AddLine(List<float> vertices, float x0, float y0, float x1, float y1)
    {
        vertices.Add(x0);
        vertices.Add(y0);
        vertices.Add(x1);
        vertices.Add(y1);
    }
}
