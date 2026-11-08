namespace MemoryTrainer.Core;

public enum GameState
{
    Playing,
    WaitingToHide,
    GameWon
}

public class SelectionState
{
    public Tile? First { get; set; }
    public Tile? Second { get; set; }
    public float HideTimer { get; set; } = 1f;

    public bool HasTwoSelected => First != null && Second != null;

    public void Reset()
    {
        First = null;
        Second = null;
        HideTimer = 1f;
    }
}