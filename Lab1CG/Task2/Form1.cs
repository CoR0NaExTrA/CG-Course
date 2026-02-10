namespace Task2;

public partial class Form1 : Form
{
    Point housePosition = new( 100, 150 );
    bool dragging = false;
    Point dragStart;

    public Form1()
    {
        InitializeComponent();
        DoubleBuffered = true;
    }

    protected override void OnPaint( PaintEventArgs e )
    {
        base.OnPaint( e );
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        int x = housePosition.X;
        int y = housePosition.Y;

        g.FillRectangle( Brushes.BurlyWood, x, y, 200, 120 );
        
        Point[] roof =
        {
            new(x - 20, y),
            new(x + 100, y - 70),
            new(x + 220, y)
        };

        g.FillPolygon( Brushes.Maroon, roof );
        g.FillRectangle( Brushes.LightBlue, x + 40, y + 30, 40, 40 );
        g.FillRectangle( Brushes.SaddleBrown, x + 120, y + 40, 40, 80 );

        for ( int i = 0; i < 10; i++ )
        {
            g.FillRectangle(
                Brushes.Peru,
                x - 60 + i * 20,
                y + 80,
                10,
                50 );
        }
    }

    protected override void OnMouseDown( MouseEventArgs e )
    {
        dragging = true;
        dragStart = e.Location;
    }

    protected override void OnMouseMove( MouseEventArgs e )
    {
        if ( !dragging )
            return;

        housePosition.X += e.X - dragStart.X;
        housePosition.Y += e.Y - dragStart.Y;
        dragStart = e.Location;

        Invalidate();
    }

    protected override void OnMouseUp( MouseEventArgs e )
    {
        dragging = false;
    }
}
