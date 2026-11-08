using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PaintOnImage.Controls;

public partial class ColorPicker : UserControl
{
    public event Action<Color>? ColorSelected;

    public ColorPicker()
    {
        InitializeComponent();
    }

    private void Pick( object sender, MouseButtonEventArgs e )
    {
        var color = ( ( SolidColorBrush )( ( Border )sender ).Background ).Color;
        ColorSelected?.Invoke( color );
    }
}
