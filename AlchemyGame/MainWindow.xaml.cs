using AlchemyGame.Models;
using AlchemyGame.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AlchemyGame;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel = new();

    private const double ElementSize = 64;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _viewModel;
    }

    private void Element_MouseDown( object sender, MouseButtonEventArgs e )
    {
        if ( sender is FrameworkElement fe &&
            fe.DataContext is ElementModel model )
        {
            DragDrop.DoDragDrop( fe, model, DragDropEffects.Copy );
        }
    }

    private void Canvas_Drop( object sender, DragEventArgs e )
    {
        if ( !e.Data.GetDataPresent( typeof( ElementModel ) ) )
            return;

        var model = ( ElementModel )e.Data.GetData( typeof( ElementModel ) )!;

        var canvas = FindVisualChild<Canvas>( FieldItemsControl );
        if ( canvas == null )
            return;

        var pos = e.GetPosition( canvas );

        var dropped = new FieldElementViewModel(
            model,
            pos.X - ElementSize / 2,
            pos.Y - ElementSize / 2
        );

        _viewModel.FieldElements.Add( dropped );

        var intersect = _viewModel.FieldElements
            .FirstOrDefault( el => el != dropped && Intersects( el, dropped ) );

        if ( intersect != null )
        {
            _viewModel.TryCombine( dropped, intersect );
        }
    }

    private bool Intersects( FieldElementViewModel a, FieldElementViewModel b )
    {
        var r1 = new Rect( a.X, a.Y, ElementSize, ElementSize );
        var r2 = new Rect( b.X, b.Y, ElementSize, ElementSize );
        return r1.IntersectsWith( r2 );
    }

    private static T? FindVisualChild<T>( DependencyObject parent )
        where T : DependencyObject
    {
        for ( int i = 0; i < VisualTreeHelper.GetChildrenCount( parent ); i++ )
        {
            var child = VisualTreeHelper.GetChild( parent, i );

            if ( child is T found )
                return found;

            var result = FindVisualChild<T>( child );
            if ( result != null )
                return result;
        }
        return null;
    }
}