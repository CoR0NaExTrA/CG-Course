using Microsoft.Win32;
using PaintOnImage.ViewModels;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PaintOnImage;

public partial class MainWindow : Window
{
    private readonly MainViewModel _vm = new();
    private bool _drawing;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _vm;
    }

    private void New_Click( object sender, RoutedEventArgs e )
    {
        Canvas.Children.Clear();
    }

    private void Open_Click( object sender, RoutedEventArgs e )
    {
        OpenFileDialog dlg = new()
        {
            Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp"
        };

        if ( dlg.ShowDialog() == true )
        {
            BitmapImage img = new( new Uri( dlg.FileName ) );
            Canvas.Children.Clear();
            Canvas.Children.Add( new Image { Source = img } );
        }
    }

    private void Save_Click( object sender, RoutedEventArgs e )
    {
        SaveFileDialog dlg = new()
        {
            Filter = "PNG|*.png|BMP|*.bmp|JPEG|*.jpg"
        };

        if ( dlg.ShowDialog() == true )
        {
            RenderTargetBitmap rtb = new(
                ( int )Canvas.ActualWidth,
                ( int )Canvas.ActualHeight,
                96, 96, PixelFormats.Pbgra32 );

            rtb.Render( Canvas );

            BitmapEncoder encoder = dlg.FilterIndex switch
            {
                1 => new PngBitmapEncoder(),
                2 => new BmpBitmapEncoder(),
                _ => new JpegBitmapEncoder()
            };

            encoder.Frames.Add( BitmapFrame.Create( rtb ) );

            using var fs = File.Create( dlg.FileName );
            encoder.Save( fs );
        }
    }

    private void Exit_Click( object sender, RoutedEventArgs e )
    {
        Close();
    }

    private void OnColorSelected( Color c )
    {
        _vm.Settings.Color = c;
    }

    private void Down( object s, MouseButtonEventArgs e )
    {
        _drawing = true;
        _vm.Drawing.Start( e.GetPosition( Canvas ) );
    }

    private void Move( object s, MouseEventArgs e )
    {
        if ( !_drawing )
            return;
        Canvas.Children.Add( _vm.Drawing.Draw( e.GetPosition( Canvas ) ) );
    }

    private void Up( object s, MouseButtonEventArgs e )
    {
        _drawing = false;
    }
}