namespace Task3;

public partial class Form1 : Form
{
    private readonly CircleSettings settings = new();

    public Form1()
    {
        InitializeComponent();
        DoubleBuffered = true;
    }

    protected override void OnPaint( PaintEventArgs e )
    {
        base.OnPaint( e );

        var g = e.Graphics;

        if ( settings.Fill )
            CircleDrawer.FillCircle(
                g,
                settings.CenterX,
                settings.CenterY,
                settings.Radius,
                settings.FillColor,
                ClientSize );

        CircleDrawer.DrawThickCircle(
            g,
            settings.CenterX,
            settings.CenterY,
            settings.Radius,
            settings.Thickness,
            settings.BorderColor,
            ClientSize );
    }
}
