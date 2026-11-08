using System.Numerics;

namespace MemoryTrainer.Core;

public class Game
{
    private readonly int _rows, _cols;
    private readonly float _spacing;
    private readonly Tile[,] _tiles;
    private readonly SelectionState _selection = new();

    public GameState State { get; private set; } = GameState.Playing;
    public int MatchedCount { get; private set; }
    public int TotalPairs => ( _rows * _cols ) / 2;

    public Game( int rows, int cols, float spacing )
    {
        _rows = rows;
        _cols = cols;
        _spacing = spacing;
        _tiles = new Tile[ rows, cols ];
    }

    public void Initialize()
    {
        GenerateTiles();
    }

    public void Update( double deltaTime )
    {
        var dt = ( float )deltaTime;

        foreach ( var tile in GetAllTiles() )
            tile.Update( dt );

        if ( State == GameState.WaitingToHide )
            UpdateHideTimer( dt );
    }

    public bool TrySelectTile( Tile tile )
    {
        if ( tile.IsMatched || tile.IsAnimating || State != GameState.Playing )
            return false;

        if ( tile == _selection.First )
        {
            _selection.First.IsRevealed = false;
            _selection.First = null;
            return true;
        }

        if ( _selection.First == null )
        {
            _selection.First = tile;
            tile.IsRevealed = true;
            return true;
        }

        if ( _selection.Second == null )
        {
            _selection.Second = tile;
            tile.IsRevealed = true;
            ProcessSelection();
            return true;
        }

        return false;
    }

    private void ProcessSelection()
    {
        var first = _selection.First;
        var second = _selection.Second;

        if ( first!.Id == second!.Id )
        {
            first.IsMatched = true;
            second.IsMatched = true;
            MatchedCount++;

            if ( MatchedCount >= TotalPairs )
                State = GameState.GameWon;

            _selection.Reset();
        }
        else
        {
            State = GameState.WaitingToHide;
            _selection.HideTimer = 1.0f;

            _selection.First = first;
            _selection.Second = second;
        }
    }

    private void UpdateHideTimer( float dt )
    {
        _selection.HideTimer -= dt;

        if ( _selection.HideTimer <= 0f )
        {
            if ( _selection.First != null )
                _selection.First.IsRevealed = false;
            if ( _selection.Second != null )
                _selection.Second.IsRevealed = false;

            _selection.Reset();
            State = GameState.Playing;
        }
    }


    private void GenerateTiles()
    {
        var ids = Enumerable.Range( 0, TotalPairs )
                           .SelectMany( i => new[] { i, i } )
                           .OrderBy( _ => Random.Shared.Next() )
                           .ToList();

        int idx = 0;
        for ( int r = 0; r < _rows; r++ )
            for ( int c = 0; c < _cols; c++ )
                _tiles[ r, c ] = new Tile
                {
                    Id = ids[ idx++ ],
                    Position = new Vector3( c * _spacing, 0, r * _spacing )
                };
    }

    public IEnumerable<Tile> GetAllTiles()
    {
        foreach ( var tile in _tiles )
            yield return tile;
    }

    public Tile? GetTileAtPosition( Vector3 worldPos )
    {
        Tile? closest = null;
        float minDist = float.MaxValue;

        foreach ( var tile in GetAllTiles() )
        {
            if ( tile.IsMatched )
                continue;

            var center = tile.Position + new Vector3( 0.5f, 0.1f, 0.5f );
            var dist = Vector3.Distance( worldPos, center );

            if ( tile.ContainsPoint( worldPos ) && dist < minDist )
            {
                minDist = dist;
                closest = tile;
            }
        }
        return closest;
    }
}