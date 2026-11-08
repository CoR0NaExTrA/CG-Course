using Battleship3D.Models;
using System.Numerics;

namespace Battleship3D.Services;

public static class ShipSpawner
{
    public static Ship Spawn(
        Random random,
        Mesh ship1Mesh,
        Mesh ship2Mesh,
        Mesh ship3Mesh,
        uint ship1Tex,
        uint ship2Tex,
        uint ship3Tex )
    {
        int type = random.Next( 3 );

        Mesh mesh;
        uint tex;

        switch ( type )
        {
            case 0:
                mesh = ship1Mesh;
                tex = ship1Tex;
                break;

            case 1:
                mesh = ship2Mesh;
                tex = ship2Tex;
                break;

            default:
                mesh = ship3Mesh;
                tex = ship3Tex;
                break;
        }

        return new Ship(
            mesh,
            tex,
            new Vector3(
                -120,
                1.5f,
                -30 - random.Next( 100 ) ),
            4 + random.Next( 5 ) );
    }
}