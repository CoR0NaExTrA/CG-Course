using Task4.Game;
using Task4.Rendering;

namespace Task4;

public partial class Form1 : Form
{
    GameState game;
    WordProvider provider;
    IHangmanRenderer renderer;
    bool useClassic = true;

    public Form1()
    {
        InitializeComponent();
        provider = new WordProvider( "C:\\Users\\Zzenam\\source\\repos\\Lab1CG\\Task4\\words.txt" );
        StartNewGame();
    }

    protected override void OnPaint( PaintEventArgs e )
    {
        base.OnPaint( e );

        if ( game == null || renderer == null )
            return;

        renderer.Draw( e.Graphics, game );

        DrawWord( e.Graphics );
    }

    void StartNewGame()
    {
        var (word, hint) = provider.GetRandom();
        game = new GameState( word, hint );

        renderer = useClassic
            ? new ClassicRenderer()
            : new ProgressRenderer();

        Controls.Clear();

        CreateViewSwitchButton();
        CreateLetterButtons();

        Invalidate();
    }

    void CreateViewSwitchButton()
    {
        var btn = new Button
        {
            Text = "Сменить вид",
            Width = 120,
            Height = 30,
            Top = 10,
            Left = 250
        };

        btn.Click += ( s, e ) =>
        {
            useClassic = !useClassic;
            renderer = useClassic
                ? new ClassicRenderer()
                : new ProgressRenderer();

            Invalidate();
        };

        Controls.Add( btn );
    }

    void DrawWord( Graphics g )
    {
        var display = string.Join( " ",
            game.Word.Select( c => game.Guessed.Contains( c ) ? c : '_' ) );

        using var font = new Font( "Arial", 24 );
        g.DrawString( display, font, Brushes.Black, 250, 50 );

        using var hintFont = new Font( "Arial", 12 );
        g.DrawString( $"Подсказка: {game.Hint}", hintFont, Brushes.Gray, 250, 90 );
    }

    void CreateLetterButtons()
    {
        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 120
        };

        for ( char c = 'А'; c <= 'Я'; c++ )
        {
            char letter = c;

            var btn = new Button
            {
                Text = letter.ToString(),
                Width = 35,
                Height = 35
            };

            btn.Click += ( s, e ) => OnLetterClicked( btn, letter );
            panel.Controls.Add( btn );
        }

        Controls.Add( panel );
    }

    void OnLetterClicked( Button btn, char c )
    {
        game.Guess( char.ToUpperInvariant( c ) );

        if ( game.Word.Contains( char.ToUpperInvariant( c ) ) )
            btn.BackColor = Color.LightGreen;
        else
            btn.BackColor = Color.LightCoral;

        btn.Enabled = false;

        Invalidate();

        CheckGameEnd();
    }

    void CheckGameEnd()
    {
        if ( game.IsWin )
        {
            if ( MessageBox.Show(
                "Вы выиграли! Начать новую игру?",
                "Победа",
                MessageBoxButtons.YesNo ) == DialogResult.Yes )
                StartNewGame();
            else
                Close();
        }
        else if ( game.IsLose )
        {
            if ( MessageBox.Show(
                $"Вы проиграли! Слово: {game.Word}\nНачать заново?",
                "Поражение",
                MessageBoxButtons.YesNo ) == DialogResult.Yes )
                StartNewGame();
            else
                Close();
        }
    }

}
