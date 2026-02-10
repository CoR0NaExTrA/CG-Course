namespace Task3.Models;

public class Element
{
    public string Name { get; }
    public string ImagePath { get; }

    public Element( string name, string imagePath )
    {
        Name = name;
        ImagePath = imagePath;
    }

    public override string ToString() => Name;
}

