namespace Maze3D.World;

public class Maze
{
    public int Size { get; }
    private readonly int[,] cells;

    public Maze(int size)
    {
        Size = size;
        cells = new int[size, size];
    }

    public int this[int x, int z] => cells[x, z];

    public bool IsWall(int x, int z) =>
        x < 0 || z < 0 || x >= Size || z >= Size || cells[x, z] == 1;

    public void GenerateDFS()
    {
        for (int x = 0; x < Size; x++)
            for (int z = 0; z < Size; z++)
                cells[x, z] = 1;

        var rand = new Random();
        Carve(1, 1, rand);
    }

    private void Carve(int x, int z, Random rand)
    {
        int[] dirs = { 0, 1, 2, 3 };
        dirs = dirs.OrderBy(_ => rand.Next()).ToArray();

        foreach (int dir in dirs)
        {
            int dx = dir switch
            {
                0 => 2,
                1 => -2,
                _ => 0
            };

            int dz = dir switch
            {
                2 => 2,
                3 => -2,
                _ => 0
            };

            int nx = x + dx;
            int nz = z + dz;

            if (nx > 0 && nz > 0 && nx < Size - 1 && nz < Size - 1)
            {
                if (cells[nx, nz] == 1)
                {
                    cells[x + dx / 2, z + dz / 2] = 0;
                    cells[nx, nz] = 0;
                    Carve(nx, nz, rand);
                }
            }
        }
    }
}