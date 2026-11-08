using System.Windows;
using ViewImage.ViewModels;

namespace ViewImage;

public partial class MainWindow : Window
{
    private readonly MainViewModel _vm = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _vm;
    }

    private void Open_Click( object sender, RoutedEventArgs e )
    {
        _vm.OpenImage();
    }
}