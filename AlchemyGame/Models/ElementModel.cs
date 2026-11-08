namespace AlchemyGame.Models;

public class ElementModel
{
    public string Name { get; }
    public string ImagePath { get; }

    public ElementModel( string name, string imagePath )
    {
        Name = name;
        ImagePath = imagePath;
    }
}
