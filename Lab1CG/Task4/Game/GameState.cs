namespace Task4.Game;
public class GameState
{
    public string Word;
    public string Hint;

    public HashSet<char> Guessed = new();
    public HashSet<char> Wrong = new();

    public int MaxErrors = 10;

    public GameState( string word, string hint )
    {
        Word = word.ToUpperInvariant();
        Hint = hint;
    }

    public void Guess( char c )
    {
        if ( Guessed.Contains( c ) || Wrong.Contains( c ) )
            return;

        if ( Word.Contains( c ) )
            Guessed.Add( c );
        else
            Wrong.Add( c );
    }

    public bool IsWin => Word.All( c => Guessed.Contains( c ) );
    public bool IsLose => Wrong.Count >= MaxErrors;
}

