using Maze3D.World;

public class Maze
{
    public int Size { get; }
    private readonly WallType[,] cells;

    public Maze(int size)
    {
        Size = size;
        cells = new WallType[size, size];
    }

    public WallType this[int x, int z] =>
        x < 0 || z < 0 || x >= Size || z >= Size ? WallType.Brick : cells[x, z];

    public bool IsWall(int x, int z) => this[x, z] != WallType.None;

    public void SetWall(int x, int z, WallType type)
    {
        if (x >= 0 && z >= 0 && x < Size && z < Size)
            cells[x, z] = type;
    }

    public void GenerateDFS()
    {
        for (int x = 0; x < Size; x++)
            for (int z = 0; z < Size; z++)
                cells[x, z] = WallType.Brick;

        var rand = new Random();
        Carve(1, 1, rand);

        AddWallVariety(rand);
    }

    private void Carve(int x, int z, Random rand)
    {
        int[] dirs = { 0, 1, 2, 3 };
        dirs = dirs.OrderBy(_ => rand.Next()).ToArray();

        foreach (int dir in dirs)
        {
            int dx = dir switch { 0 => 2, 1 => -2, _ => 0 };
            int dz = dir switch { 2 => 2, 3 => -2, _ => 0 };
            int nx = x + dx, nz = z + dz;

            if (nx > 0 && nz > 0 && nx < Size - 1 && nz < Size - 1)
            {
                if (cells[nx, nz] == WallType.Brick)
                {
                    cells[x + dx / 2, z + dz / 2] = WallType.None;
                    cells[nx, nz] = WallType.None;
                    Carve(nx, nz, rand);
                }
            }
        }
    }

    private void AddWallVariety(Random rand)
    {
        var types = new[] { WallType.Stone, WallType.Wood, WallType.Metal, WallType.Grass, WallType.Ice };

        for (int x = 0; x < Size; x++)
        {
            for (int z = 0; z < Size; z++)
            {
                if (IsWall(x, z) && rand.NextSingle() < 0.3f)
                {
                    cells[x, z] = types[rand.Next(types.Length)];
                }
            }
        }
    }
}