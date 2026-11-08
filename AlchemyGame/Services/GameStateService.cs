using AlchemyGame.Models;
using AlchemyGame.ViewModels;
using System.Collections.ObjectModel;

namespace AlchemyGame.Services;

public class GameStateService
{
    public ObservableCollection<ElementModel> Discovered { get; } = new();
    public ObservableCollection<FieldElementViewModel> Field { get; } = new();

    public void Discover( ElementModel element )
    {
        if ( !Discovered.Any( e => e.Name == element.Name ) )
            Discovered.Add( element );
    }
}

