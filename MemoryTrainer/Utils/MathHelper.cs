namespace MemoryTrainer.Utils;

public static class MathHelper
{
    public static float Lerp( float a, float b, float t )
    {
        return a + ( b - a ) * t;
    }
}