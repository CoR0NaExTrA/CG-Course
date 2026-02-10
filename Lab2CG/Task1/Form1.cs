using System.Drawing.Drawing2D;

namespace Task1;

public partial class Form1 : Form
{
    private Image? image;
    private RectangleF imageRect;
    private SizeF originalImageSize;

    private bool dragging;
    private Point lastMousePos;

    public Form1()
    {
        InitializeComponent();
        DoubleBuffered = true;

        Resize += ( _, _ ) =>
        {
            HandleResize();
            Invalidate();
        };

        MouseDown += MainForm_MouseDown;
        MouseMove += MainForm_MouseMove;
        MouseUp += ( _, _ ) => dragging = false;
    }

    private void openToolStripMenuItem_Click( object sender, EventArgs e )
    {
        using OpenFileDialog dlg = new()
        {
            Filter = "Images (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp"
        };

        if ( dlg.ShowDialog() != DialogResult.OK )
            return;

        image?.Dispose();
        image = Image.FromFile( dlg.FileName );
        originalImageSize = image.Size;

        CreateInitialImageRect();
        Invalidate();
    }

    private void CreateInitialImageRect()
    {
        if ( image == null )
            return;

        float scale = 1f;

        if ( image.Width > ClientSize.Width || image.Height > ClientSize.Height )
        {
            scale = Math.Min(
                ( float )ClientSize.Width / image.Width,
                ( float )ClientSize.Height / image.Height
            );
        }

        float w = image.Width * scale;
        float h = image.Height * scale;

        imageRect = new RectangleF(
            ( ClientSize.Width - w ) / 2,
            ( ClientSize.Height - h ) / 2,
            w,
            h
        );
    }

    protected override void OnPaint( PaintEventArgs e )
    {
        base.OnPaint( e );
        if ( image == null )
            return;

        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

        DrawCheckerboardUnderImage( e.Graphics );
        e.Graphics.DrawImage( image, imageRect );
    }

    private void DrawCheckerboardUnderImage( Graphics g )
    {
        const int cell = 20;

        var oldClip = g.Clip;
        g.SetClip( imageRect );

        for ( int y = ( int )imageRect.Top; y < imageRect.Bottom; y += cell )
        {
            for ( int x = ( int )imageRect.Left; x < imageRect.Right; x += cell )
            {
                bool dark = ( ( x / cell ) + ( y / cell ) ) % 2 == 0;
                using Brush b = new SolidBrush( dark ? Color.LightGray : Color.White );
                g.FillRectangle( b, x, y, cell, cell );
            }
        }

        g.Clip = oldClip;
    }

    private void MainForm_MouseDown( object? sender, MouseEventArgs e )
    {
        if ( imageRect.Contains( e.Location ) )
        {
            dragging = true;
            lastMousePos = e.Location;
        }
    }

    private void MainForm_MouseMove( object? sender, MouseEventArgs e )
    {
        if ( !dragging )
            return;

        imageRect.X += e.X - lastMousePos.X;
        imageRect.Y += e.Y - lastMousePos.Y;

        ClampImageInsideWindow();

        lastMousePos = e.Location;
        Invalidate();
    }

    private void ClampImageInsideWindow()
    {
        if ( imageRect.Width <= ClientSize.Width )
            imageRect.X = Math.Clamp( imageRect.X, 0, ClientSize.Width - imageRect.Width );
        else
            imageRect.X = Math.Clamp( imageRect.X, ClientSize.Width - imageRect.Width, 0 );

        if ( imageRect.Height <= ClientSize.Height )
            imageRect.Y = Math.Clamp( imageRect.Y, 0, ClientSize.Height - imageRect.Height );
        else
            imageRect.Y = Math.Clamp( imageRect.Y, ClientSize.Height - imageRect.Height, 0 );
    }

    private void HandleResize()
    {
        if ( image == null )
            return;

        bool fits =
            imageRect.Width <= ClientSize.Width &&
            imageRect.Height <= ClientSize.Height;

        if ( fits )
        {
            // Картинка помещается — просто ограничиваем перемещение
            ClampImageInsideWindow();
            return;
        }

        // Не помещается — уменьшаем с сохранением пропорций
        float scale = Math.Min(
            ( float )ClientSize.Width / originalImageSize.Width,
            ( float )ClientSize.Height / originalImageSize.Height
        );

        float w = originalImageSize.Width * scale;
        float h = originalImageSize.Height * scale;

        imageRect = new RectangleF(
            ( ClientSize.Width - w ) / 2,
            ( ClientSize.Height - h ) / 2,
            w,
            h
        );
    }

}
