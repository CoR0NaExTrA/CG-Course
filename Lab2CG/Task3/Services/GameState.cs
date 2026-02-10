namespace Task3.Services;
public class GameState
{
    public HashSet<string> OpenedElements { get; } = new();

    public bool AddElement( string name )
    {
        return OpenedElements.Add( name );
    }

    public bool AllOpened( int totalCount )
        => OpenedElements.Count == totalCount;
}