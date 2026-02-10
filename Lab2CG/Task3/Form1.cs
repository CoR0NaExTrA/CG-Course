using System.Media;
using Task3.Models;
using Task3.Services;
using Task3.UI;

namespace Task3;

public partial class Form1 : Form
{
    private readonly List<Element> allElements = new();
    private readonly GameState gameState = new();
    private readonly RecipeService recipeService = new();

    private FlowLayoutPanel panelOpened;
    private Panel panelLab;
    private Button btnSort;

    public Form1()
    {
        InitializeComponent();
        InitializeGameUI();
        LoadBaseElements();
    }

    private void InitializeGameUI()
    {
        panelOpened = new FlowLayoutPanel
        {
            Dock = DockStyle.Left,
            Width = 200,
            AutoScroll = true
        };
        Controls.Add( panelOpened );

        panelLab = new Panel
        {
            Dock = DockStyle.Fill,
            AllowDrop = true,
            BorderStyle = BorderStyle.FixedSingle
        };
        panelLab.DragEnter += PanelLab_DragEnter;
        panelLab.DragDrop += PanelLab_DragDrop;
        Controls.Add( panelLab );

        btnSort = new Button { Text = "Сортировать", Dock = DockStyle.Top };
        btnSort.Click += ( s, e ) => SortElements();
        panelOpened.Controls.Add( btnSort );
    }

    private void LoadBaseElements()
    {
        allElements.Add( new Element( "Огонь", "C:\\Users\\Zzenam\\source\\repos\\Lab2CG\\Task3\\Assets\\Images\\fire.jpg" ) );
        allElements.Add( new Element( "Вода", "C:\\Users\\Zzenam\\source\\repos\\Lab2CG\\Task3\\Assets\\Images\\water.jpg" ) );
        allElements.Add( new Element( "Земля", "C:\\Users\\Zzenam\\source\\repos\\Lab2CG\\Task3\\Assets\\Images\\earth.jpg" ) );
        allElements.Add( new Element( "Воздух", "C:\\Users\\Zzenam\\source\\repos\\Lab2CG\\Task3\\Assets\\Images\\air.jpg" ) );

        foreach ( var e in allElements )
            AddOpenedElement( e );
    }

    private void AddOpenedElement( Element element )
    {
        if ( !gameState.OpenedElements.Contains( element.Name ) )
        {
            gameState.AddElement( element.Name );
            var control = new ElementControl( element );
            panelOpened.Controls.Add( control );
        }
    }

    private void PanelLab_DragEnter( object sender, DragEventArgs e )
    {
        if ( e.Data.GetDataPresent( typeof( ElementControl ) ) )
            e.Effect = DragDropEffects.Copy;
    }

    private void PanelLab_DragDrop( object sender, DragEventArgs e )
    {
        if ( e.Data.GetData( typeof( ElementControl ) ) is ElementControl elControl )
        {
            var copy = new ElementControl( elControl.Element )
            {
                Location = panelLab.PointToClient( new Point( e.X, e.Y ) )
            };
            panelLab.Controls.Add( copy );
            copy.BringToFront();
            copy.MouseDown += ( s, args ) => DoDragDrop( copy, DragDropEffects.Move );

        }
    }

    private void SortElements()
    {
        var sorted = panelOpened.Controls.OfType<ElementControl>().OrderBy( c => c.Element.Name ).ToList();
        panelOpened.Controls.Clear();
        panelOpened.Controls.Add( btnSort );
        panelOpened.Controls.AddRange( sorted.ToArray() );
    }
}