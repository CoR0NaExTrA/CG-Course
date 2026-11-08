using AlchemyGame.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AlchemyGame.ViewModels;
public class FieldElementViewModel : INotifyPropertyChanged
{
    public ElementModel Model { get; }

    public string Name => Model.Name;
    public string ImagePath => Model.ImagePath;

    private double _x;
    public double X
    {
        get => _x;
        set { _x = value; OnPropertyChanged(); }
    }

    private double _y;
    public double Y
    {
        get => _y;
        set { _y = value; OnPropertyChanged(); }
    }

    public FieldElementViewModel( ElementModel model, double x, double y )
    {
        Model = model;
        X = x;
        Y = y;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged( [CallerMemberName] string? name = null )
        => PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );
}
