using PaintOnImage.Models;
using PaintOnImage.Services;

namespace PaintOnImage.ViewModels;

public class MainViewModel
{
    public DrawingSettings Settings { get; } = new();
    public DrawingService Drawing { get; }

    public MainViewModel()
    {
        Drawing = new DrawingService( Settings );
    }
}

