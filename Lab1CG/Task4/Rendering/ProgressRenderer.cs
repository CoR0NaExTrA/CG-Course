using Task4.Game;

namespace Task4.Rendering;

public class ProgressRenderer : IHangmanRenderer
{
    public void Draw( Graphics g, GameState game )
    {
        int max = game.MaxErrors;
        int errors = game.Wrong.Count;

        g.DrawRectangle( Pens.Black, 50, 50, 30, 200 );

        float percent = ( float )( max - errors ) / max;
        int fillHeight = ( int )( 200 * percent );

        Brush brush =
            percent > 0.6f ? Brushes.LightGreen :
            percent > 0.3f ? Brushes.Gold :
            Brushes.IndianRed;

        g.FillRectangle(
            brush,
            50,
            50 + ( 200 - fillHeight ),
            30,
            fillHeight
        );

        using var font = new Font( "Arial", 10 );
        g.DrawString(
            $"Ошибки: {errors}/{max}",
            font,
            Brushes.Black,
            20,
            260
        );

        if ( game.Wrong.Any() )
        {
            g.DrawString(
                "Неверные буквы:",
                font,
                Brushes.Black,
                20,
                290
            );

            g.DrawString(
                string.Join( " ", game.Wrong ),
                font,
                Brushes.DarkRed,
                20,
                310
            );
        }
    }
}
