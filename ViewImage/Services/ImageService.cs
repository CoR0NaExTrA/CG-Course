using Microsoft.Win32;
using System.Windows.Media.Imaging;

namespace PaintOnImage.Services;

public class ImageService
{
    public BitmapImage? OpenImage()
    {
        OpenFileDialog dlg = new()
        {
            Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp"
        };

        if ( dlg.ShowDialog() != true )
            return null;

        BitmapImage image = new();
        image.BeginInit();
        image.UriSource = new Uri( dlg.FileName );
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.EndInit();

        return image;
    }
}
