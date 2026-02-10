using Task3.Models;

namespace Task3.Services;
public class RecipeService
{
    private readonly List<Recipe> recipes = new();

    public RecipeService()
    {
        recipes.Add( new Recipe( "Огонь", "Вода", "Пар" ) );
        recipes.Add( new Recipe( "Огонь", "Земля", "Лава" ) );
        recipes.Add( new Recipe( "Воздух", "Земля", "Пыль" ) );
        recipes.Add( new Recipe( "Огонь", "Пыль", "Порох" ) );
        recipes.Add( new Recipe( "Порох", "Огонь", "Взрыв", "Дым" ) );
        recipes.Add( new Recipe( "Воздух", "Огонь", "Энергия" ) );
        recipes.Add( new Recipe( "Лава", "Вода", "Пар", "Камень" ) );
        recipes.Add( new Recipe( "Воздух", "Энергия", "Буря" ) );
        recipes.Add( new Recipe( "Огонь", "Камень", "Металл" ) );
        recipes.Add( new Recipe( "Металл", "Энергия", "Электричество" ) );
        recipes.Add( new Recipe( "Электричество", "Вода", "Водород", "Кислород" ) );
        recipes.Add( new Recipe( "Электричество", "Кислород", "Озон" ) );
        recipes.Add( new Recipe( "Пыль", "Вода", "Грязь" ) );
        recipes.Add( new Recipe( "Пар", "Земля", "Гейзер" ) );
        recipes.Add( new Recipe( "Пар", "Металл", "Паровой котел" ) );
        recipes.Add( new Recipe( "Паровой котел", "Пар", "Давление" ) );
        recipes.Add( new Recipe( "Лава", "Давление", "Вулкан" ) );
        recipes.Add( new Recipe( "Водород", "Кислород", "Гремучий газ" ) );
        recipes.Add( new Recipe( "Вода", "Земля", "Болото" ) );
        recipes.Add( new Recipe( "Огонь", "Вода", "Спирт" ) );
        recipes.Add( new Recipe( "Спирт", "Огонь", "Коктейль Молотова" ) );
        recipes.Add( new Recipe( "Болото", "Энергия", "Жизнь" ) );
        recipes.Add( new Recipe( "Жизнь", "Болото", "Бактерии" ) );
        recipes.Add( new Recipe( "Спирт", "Вода", "Водка" ) );
    }

    public List<string> TryCombine( string a, string b )
    {
        return recipes
            .FirstOrDefault( r => r.Matches( a, b ) )
            ?.Results
            ?? [];
    }
}
