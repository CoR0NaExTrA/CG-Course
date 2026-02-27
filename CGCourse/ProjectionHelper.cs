using OpenTK.Mathematics;

namespace CGCourse;

internal static class ProjectionHelper
{
    public static Matrix4 CreatePreservingAspectProjection(int width, int height)
    {
        var aspect = height == 0 ? 1f : width / (float)height;
        var halfWidth = aspect >= 1f ? SceneConfig.BaseHalfRange * aspect : SceneConfig.BaseHalfRange;
        var halfHeight = aspect >= 1f ? SceneConfig.BaseHalfRange : SceneConfig.BaseHalfRange / aspect;

        return Matrix4.CreateOrthographicOffCenter(-halfWidth, halfWidth, -halfHeight, halfHeight, -1f, 1f);
    }
}
