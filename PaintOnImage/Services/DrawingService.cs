using PaintOnImage.Models;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintOnImage.Services;

public class DrawingService
{
    private Point _last;
    private readonly DrawingSettings _settings;

    public DrawingService( DrawingSettings settings )
    {
        _settings = settings;
    }

    public void Start( Point p ) => _last = p;

    public Line Draw( Point p )
    {
        Line line = new()
        {
            X1 = _last.X,
            Y1 = _last.Y,
            X2 = p.X,
            Y2 = p.Y,
            StrokeThickness = _settings.Thickness,
            Stroke = new SolidColorBrush( _settings.Color ),
            StrokeStartLineCap = PenLineCap.Round,
            StrokeEndLineCap = PenLineCap.Round
        };

        _last = p;
        return line;
    }
}
