using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Task2;

public partial class Form1 : Form
{
    private Bitmap? canvas;
    private Graphics? graphics;

    private bool drawing;
    private Point lastPoint;

    private Color currentColor = Color.Black;
    private readonly int penWidth = 4;

    public Form1()
    {
        InitializeComponent();

        MouseDown += MainForm_MouseDown;
        MouseMove += MainForm_MouseMove;
        MouseUp += ( _, _ ) => drawing = false;
    }

    // ---------- File -> New ----------
    private void newToolStripMenuItem_Click( object sender, EventArgs e )
    {
        CreateNewCanvas( ClientSize.Width, ClientSize.Height );
    }

    private void CreateNewCanvas( int width, int height )
    {
        canvas?.Dispose();
        graphics?.Dispose();

        canvas = new Bitmap( width, height );
        graphics = Graphics.FromImage( canvas );
        graphics.Clear( Color.White );
        graphics.SmoothingMode = SmoothingMode.AntiAlias;

        Invalidate();
    }

    // ---------- File -> Open ----------
    private void openToolStripMenuItem_Click( object sender, EventArgs e )
    {
        using OpenFileDialog dlg = new()
        {
            Filter = "Images (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp"
        };

        if ( dlg.ShowDialog() != DialogResult.OK )
            return;

        canvas?.Dispose();
        graphics?.Dispose();

        canvas = new Bitmap( dlg.FileName );
        graphics = Graphics.FromImage( canvas );
        graphics.SmoothingMode = SmoothingMode.AntiAlias;

        Invalidate();
    }

    // ---------- File -> Save As ----------
    private void saveAsToolStripMenuItem_Click( object sender, EventArgs e )
    {
        if ( canvas == null )
            return;

        using SaveFileDialog dlg = new()
        {
            Filter = "PNG (*.png)|*.png|JPEG (*.jpg)|*.jpg|BMP (*.bmp)|*.bmp"
        };

        if ( dlg.ShowDialog() != DialogResult.OK )
            return;

        ImageFormat format = Path.GetExtension( dlg.FileName ).ToLower() switch
        {
            ".jpg" => ImageFormat.Jpeg,
            ".bmp" => ImageFormat.Bmp,
            _ => ImageFormat.Png
        };

        canvas.Save( dlg.FileName, format );
    }

    // ---------- Color ----------
    private void colorToolStripMenuItem_Click( object sender, EventArgs e )
    {
        using ColorDialog dlg = new();
        if ( dlg.ShowDialog() == DialogResult.OK )
            currentColor = dlg.Color;
    }

    // ---------- Drawing ----------
    private void MainForm_MouseDown( object? sender, MouseEventArgs e )
    {
        if ( canvas == null )
            return;
        if ( e.Button != MouseButtons.Left )
            return;

        drawing = true;
        lastPoint = e.Location;
    }

    private void MainForm_MouseMove( object? sender, MouseEventArgs e )
    {
        if ( !drawing || canvas == null || graphics == null )
            return;

        using Pen pen = new( currentColor, penWidth )
        {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round
        };

        graphics.DrawLine( pen, lastPoint, e.Location );
        lastPoint = e.Location;

        Invalidate();
    }

    protected override void OnPaint( PaintEventArgs e )
    {
        base.OnPaint( e );
        if ( canvas != null )
            e.Graphics.DrawImageUnscaled( canvas, Point.Empty );
    }
}
