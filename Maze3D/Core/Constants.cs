namespace Maze3D.Core;

public static class Constants
{
    // Лабиринт
    public const int MAZE_SIZE = 17;

    // Игрок
    public const float PLAYER_RADIUS = 0.3f;
    public const float PLAYER_HEIGHT = 1.8f;
    public const float PLAYER_Y = 1.0f;

    // Движение
    public const float MOVE_SPEED = 3f;
    public const float MAX_STEP_FACTOR = 0.5f;
    public const float COLLISION_MARGIN = 0.05f;

    // Камера
    public const float MOUSE_SENSITIVITY = 0.1f;
    public const float PITCH_MIN = -89f;
    public const float PITCH_MAX = 89f;
    public const float FOV = MathF.PI / 4;
    public const float NEAR_PLANE = 0.1f;
    public const float FAR_PLANE = 100f;

    // Окно
    public const int WINDOW_WIDTH = 1000;
    public const int WINDOW_HEIGHT = 800;
    public const string WINDOW_TITLE = "3D Maze";

    // Миникарта
    public const int MINIMAP_SIZE = 200;
    public const float MINIMAP_PLAYER_SCALE = 0.25f;
}