namespace AlchemyGame.Models;

public class Recipe
{
    public string FirstRecipe { get; }
    public string SecondRecipe { get; }
    public List<string> Results { get; }

    public Recipe( string firstRecipe, string secondRecipe, params string[] results )
    {
        FirstRecipe = firstRecipe;
        SecondRecipe = secondRecipe;
        Results = results.ToList();
    }

    public bool Matches( string x, string y ) =>
        ( FirstRecipe == x && SecondRecipe == y ) || ( FirstRecipe == y && SecondRecipe == x );
}
