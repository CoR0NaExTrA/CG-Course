using System.Windows.Media;

namespace PaintOnImage.Models;

public class DrawingSettings
{
    public Color Color { get; set; } = Colors.Black;
    public double Thickness { get; set; } = 4;
}
