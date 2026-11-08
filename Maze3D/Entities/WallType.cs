namespace Maze3D.World;

public enum WallType
{
    None = 0,
    Brick = 1,
    Stone = 2,
    Wood = 3,
    Metal = 4,
    Grass = 5,
    Ice = 6,
}

public static class WallTypeExtensions
{
    public static string GetTexturePath(this WallType type) => type switch
    {
        WallType.Brick => "C:\\Users\\HONOR\\source\\repos\\Lab5CG\\Maze3D\\bin\\Debug\\net10.0\\Textures\\texture1.jpg",
        WallType.Stone => "C:\\Users\\HONOR\\source\\repos\\Lab5CG\\Maze3D\\bin\\Debug\\net10.0\\Textures\\texture2.jpg",
        WallType.Wood => "C:\\Users\\HONOR\\source\\repos\\Lab5CG\\Maze3D\\bin\\Debug\\net10.0\\Textures\\texture3.jpg",
        WallType.Metal => "C:\\Users\\HONOR\\source\\repos\\Lab5CG\\Maze3D\\bin\\Debug\\net10.0\\Textures\\texture4.jpg",
        WallType.Grass => "C:\\Users\\HONOR\\source\\repos\\Lab5CG\\Maze3D\\bin\\Debug\\net10.0\\Textures\\texture5.jpg",
        _ => "C:\\Users\\HONOR\\source\\repos\\Lab5CG\\Maze3D\\bin\\Debug\\net10.0\\Textures\\brick.jpg"
    };

    private static readonly Random _rand = new();
    public static WallType GetRandom()
    {
        var values = Enum.GetValues<WallType>();
        WallType result;
        do
        {
            result = values[_rand.Next(values.Length)];
        } while (result == WallType.None);
        return result;
    }
}