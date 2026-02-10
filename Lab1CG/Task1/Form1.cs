namespace Task1;

public partial class Form1 : Form
{
    private readonly List<Letter> letters = new();
    private readonly System.Windows.Forms.Timer timer = new();
    private DateTime lastFrameTime;

    public Form1()
    {
        InitializeComponent();
        DoubleBuffered = true;

        letters.Add( CreateT( new Point( 50, 150 ), Color.Red, 0f ) );
        letters.Add( CreateYa( new Point( 220, 150 ), Color.Blue, 0.2f ) );
        letters.Add( CreateA( new Point( 270, 150 ), Color.Green, 0.4f ) );

        timer.Interval = 16;
        timer.Tick += Timer_Tick;
        timer.Start();

        lastFrameTime = DateTime.Now;
    }

    private void Timer_Tick( object? sender, EventArgs e )
    {
        Invalidate();
    }

    protected override void OnPaint( PaintEventArgs e )
    {
        base.OnPaint( e );

        var now = DateTime.Now;
        float delta = ( float )( now - lastFrameTime ).TotalSeconds;
        lastFrameTime = now;

        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        foreach ( var letter in letters )
        {
            float dy = letter.GetOffsetY( delta );
            int offsetY = ( int )dy;

            using var brush = new SolidBrush( letter.color );

            foreach ( var r in letter.Rectangles )
            {
                var rect = new Rectangle(
                    letter.position.X + r.X,
                    letter.position.Y + r.Y + offsetY,
                    r.Width,
                    r.Height );

                g.FillRectangle( brush, rect );
            }

            foreach ( var poly in letter.Polygons )
            {
                var points = poly
                    .Select( p => new Point(
                        letter.position.X + p.X,
                        letter.position.Y + p.Y + offsetY ) )
                    .ToArray();

                g.FillPolygon( brush, points );
            }
        }
    }

    Letter CreateT( Point pos, Color color, float phase )
    {
        var l = new Letter( pos, color, phase );

        l.Rectangles.Add( new Rectangle( 0, 0, 80, 20 ) );
        l.Rectangles.Add( new Rectangle( 30, 20, 20, 100 ) );

        return l;
    }

    Letter CreateYa( Point pos, Color color, float phase )
    {
        var l = new Letter( pos, color, phase );

        l.Rectangles.Add( new Rectangle( 0, 0, 20, 120 ) );
        l.Rectangles.Add( new Rectangle( -60, 0, 60, 20 ) );
        l.Rectangles.Add( new Rectangle( -60, 50, 60, 20 ) );
        l.Rectangles.Add( new Rectangle( -60, 20, 20, 30 ) );
        l.Polygons.Add(
        [
            new Point(0, 70),
            new Point(-20, 70),
            new Point(-60, 120),
            new Point(-40, 120)
        ] );

        return l;
    }

    Letter CreateA( Point pos, Color color, float phase )
    {
        var l = new Letter( pos, color, phase );

        l.Polygons.Add(
        [
            new Point(0, 120),
            new Point(20, 120),
            new Point(50, 20),
            new Point(50, 0)
        ] );
        l.Polygons.Add(
        [
            new Point(50, 0),
            new Point(50, 20),
            new Point(70, 120),
            new Point(90, 120),
        ] );
        l.Rectangles.Add( new Rectangle( 25, 60, 40, 15 ) );

        return l;
    }


}

