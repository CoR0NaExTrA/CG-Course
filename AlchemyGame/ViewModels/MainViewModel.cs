using AlchemyGame.Models;
using AlchemyGame.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AlchemyGame.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<ElementModel> OpenedElements { get; } = new();
    public ObservableCollection<FieldElementViewModel> FieldElements { get; } = new();
    public ObservableCollection<string> Log { get; } = new();

    private readonly RecipeService _recipes = new();

    public MainViewModel()
    {
        AddBase( "Earth" );
        AddBase( "Fire" );
        AddBase( "Water" );
        AddBase( "Air" );
    }

    private void AddBase( string name )
    {
        OpenedElements.Add(
            new ElementModel(
                name,
                $"D:\\Lab2CG\\AlchemyGame\\Resources\\Images\\{name.ToLower()}.jpg"
            )
        );
    }

    public void TryCombine( FieldElementViewModel a, FieldElementViewModel b )
    {
        var results = _recipes.TryCombine( a.Name, b.Name );

        if ( results.Count == 0 )
        {
            Log.Add( $"{a.Name} + {b.Name} = ничего" );
            return;
        }

        FieldElements.Remove( a );
        FieldElements.Remove( b );

        foreach ( var result in results )
        {
            if ( !OpenedElements.Any( e => e.Name == result ) )
            {
                OpenedElements.Add(
                    new ElementModel(
                        result,
                        $"D:\\Lab2CG\\AlchemyGame\\Resources\\Images\\{result}.jpg"
                    )
                );
                Log.Add( $"Открыт новый элемент: {result}" );
            }

            FieldElements.Add(
                new FieldElementViewModel(
                    new ElementModel(
                        result,
                        $"D:\\Lab2CG\\AlchemyGame\\Resources\\Images\\{result}.jpg"
                    ),
                    a.X,
                    a.Y
                )
            );
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}

