using Task4.Game;

namespace Task4.Rendering;
public class ClassicRenderer : IHangmanRenderer
{
    public void Draw( Graphics g, GameState game )
    {
        int e = game.Wrong.Count;
        if ( e > 0 )
            g.DrawLine( Pens.Black, 50, 200, 150, 200 );
        if ( e > 1 )
            g.DrawLine( Pens.Black, 100, 200, 100, 50 );
        if ( e > 2 )
            g.DrawLine( Pens.Black, 100, 50, 180, 50 );
        if ( e > 3 )
            g.DrawLine( Pens.Black, 180, 50, 180, 70 );
        if ( e > 4 )
            g.DrawEllipse( Pens.Black, 160, 70, 40, 40 );
        if ( e > 5 )
            g.DrawLine( Pens.Black, 180, 110, 180, 160 );
        if ( e > 6 )
            g.DrawLine( Pens.Black, 180, 130, 150, 150 );
        if ( e > 7 )
            g.DrawLine( Pens.Black, 180, 130, 210, 150 );
        if ( e > 8 )
            g.DrawLine( Pens.Black, 180, 160, 150, 180 );
        if ( e > 9 )
            g.DrawLine( Pens.Black, 180, 160, 210, 180 );
    }
}

