using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    private Tile[,] grid;
    public Vector2Int dimensions { get; private set; }

    public World(Vector2Int mapDimensions) {

        dimensions = mapDimensions;
    }

    public void GenerateEmptyWorld() {

        WorldGenerator wg = new WorldGenerator();
        wg.GenerateEmptyWorld(dimensions, ref grid);
    }
}
