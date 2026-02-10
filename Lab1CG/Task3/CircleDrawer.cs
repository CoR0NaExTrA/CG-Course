namespace Task3;
public static class CircleDrawer
{
    public static void DrawPixel(
        Graphics g, int x, int y, Color color, Size bounds )
    {
        if ( x < 0 || y < 0 || x >= bounds.Width || y >= bounds.Height )
            return;

        using var brush = new SolidBrush( color );
        g.FillRectangle( brush, x, y, 1, 1 );
    }

    public static void DrawCircle(
        Graphics g, int cx, int cy, int r, Color color, Size bounds )
    {
        int x = 0;
        int y = r;
        int d = 3 - 2 * r;

        while ( x <= y )
        {
            PlotCirclePoints( g, cx, cy, x, y, color, bounds );

            if ( d < 0 )
                d += 4 * x + 6;
            else
            {
                d += 4 * ( x - y ) + 10;
                y--;
            }
            x++;
        }
    }

    private static void PlotCirclePoints(
        Graphics g, int cx, int cy, int x, int y, Color c, Size b )
    {
        DrawPixel( g, cx + x, cy + y, c, b );
        DrawPixel( g, cx - x, cy + y, c, b );
        DrawPixel( g, cx + x, cy - y, c, b );
        DrawPixel( g, cx - x, cy - y, c, b );
        DrawPixel( g, cx + y, cy + x, c, b );
        DrawPixel( g, cx - y, cy + x, c, b );
        DrawPixel( g, cx + y, cy - x, c, b );
        DrawPixel( g, cx - y, cy - x, c, b );
    }

    public static void DrawThickCircle(
        Graphics g, int cx, int cy, int r,
        int thickness, Color color, Size bounds )
    {
        for ( int i = 0; i < thickness; i++ )
            DrawCircle( g, cx, cy, r - i, color, bounds );
    }

    public static void FillCircle(
        Graphics g, int cx, int cy, int r,
        Color fill, Size bounds )
    {
        for ( int y = -r; y <= r; y++ )
        {
            int dx = ( int )Math.Sqrt( r * r - y * y );
            for ( int x = -dx; x <= dx; x++ )
                DrawPixel( g, cx + x, cy + y, fill, bounds );
        }
    }
}

