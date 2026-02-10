namespace Task1;
public class Letter
{
    public List<Rectangle> Rectangles = new();
    public List<Point[]> Polygons = new();

    public Point position;
    public Color color;

    public float time;
    public float phaseOffset;
    public float jumpHeight = 30f;

    public Letter( Point position, Color color, float phase )
    {
        this.position = position;
        this.color = color;
        phaseOffset = phase;
    }

    public float GetOffsetY( float deltaTime )
    {
        time += deltaTime;
        float t = ( time + phaseOffset ) % 1f;
        return -4 * jumpHeight * t * ( t - 1 );
    }
}

