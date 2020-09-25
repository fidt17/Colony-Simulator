using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World {

    public Tile[,] grid;
    public Vector2Int dimensions;

    public World(Vector2Int mapDimensions) => dimensions = mapDimensions;

    public Tile GetTileAt(Vector2Int position) {
        if (!IsPositionViable(position)) {
            return null;
        }
        return grid[position.x, position.y];
    }

    public bool IsPositionViable(Vector2Int position) => !(position.x < 0 || position.x >= dimensions.x || position.y < 0 || position.y >= dimensions.y);
}