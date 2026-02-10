namespace Task3.Models;
public class Recipe
{
    public string ElementA { get; }
    public string ElementB { get; }
    public List<string> Results { get; }

    public Recipe( string a, string b, params string[] results )
    {
        ElementA = a;
        ElementB = b;
        Results = results.ToList();
    }

    public bool Matches( string x, string y )
    {
        return ( x == ElementA && y == ElementB ) ||
               ( x == ElementB && y == ElementA );
    }
}
