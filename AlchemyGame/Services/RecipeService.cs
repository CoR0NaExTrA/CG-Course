using AlchemyGame.Models;

namespace AlchemyGame.Services;

public class RecipeService
{
    private readonly List<Recipe> _recipes = new()
    {
        new("Fire", "Water", "Steam"),
        new("Fire", "Earth", "Lava"),
        new("Air", "Earth", "Dust"),
        new("Fire", "Dust", "Gunpowder"),
        new("Gunpowder", "Fire", "Explosion", "Smoke"),
        new("Air", "Fire", "Energy"),
        new("Lava", "Water", "Steam", "Stone"),
        new("Metal", "Energy", "Electricity"),
        new("Electricity", "Water", "Hydrogen", "Oxygen"),
    };

    public List<string> TryCombine( string a, string b )
    {
        return _recipes
            .FirstOrDefault( r => r.Matches( a, b ) )
            ?.Results ?? new();
    }
}

