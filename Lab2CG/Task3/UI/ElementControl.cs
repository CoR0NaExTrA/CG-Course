using Task3.Models;

namespace Task3.UI;
public class ElementControl : PictureBox
{
    public Element Element { get; }

    public ElementControl( Element element )
    {
        Element = element;
        Image = Image.FromFile( element.ImagePath );
        SizeMode = PictureBoxSizeMode.StretchImage;
        Width = Height = 64;

        MouseDown += ( s, e ) =>
        {
            if ( e.Button == MouseButtons.Left )
                DoDragDrop( this, DragDropEffects.Copy );
        };
    }
}
