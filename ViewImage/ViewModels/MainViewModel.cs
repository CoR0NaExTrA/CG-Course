using PaintOnImage.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace ViewImage.ViewModels;
public class MainViewModel : INotifyPropertyChanged
{
    private readonly ImageService _imageService = new();

    private ImageSource? _image;
    public ImageSource? Image
    {
        get => _image;
        set { _image = value; OnPropertyChanged(); }
    }

    public void OpenImage()
    {
        Image = _imageService.OpenImage();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged( [CallerMemberName] string? p = null )
        => PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( p ) );
}
