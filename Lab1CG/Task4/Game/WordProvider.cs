namespace Task4.Game;
public class WordProvider
{
    private readonly List<(string, string)> words;

    public WordProvider( string path )
    {
        words = File.ReadAllLines( path )
            .Select( l => l.Split( ';' ) )
            .Select( p => (p[ 0 ], p[ 1 ]) )
            .ToList();
    }

    public (string, string) GetRandom()
    {
        var rnd = new Random();
        return words[ rnd.Next( words.Count ) ];
    }
}

