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

    public void GenerateTerrain() {

        WorldGenerator wg = new WorldGenerator();
        wg.GenerateTerrainWithPerlinNoise(dimensions, ref grid);
    }

    public void CharacterInit() {

        CharacterManager.Instance.CreateInitialCharacters();
    }

    public Tile GetTileAt(Vector2Int position) {

        if (!IsPositionViable(position))
            return null;

        return grid[position.x, position.y];
    }

    public bool IsPositionViable(Vector2Int position) {
        
        if (position.x < 0 || position.x >= dimensions.x || position.y < 0 || position.y >= dimensions.y)
            return false;

        return true;
    }
}
