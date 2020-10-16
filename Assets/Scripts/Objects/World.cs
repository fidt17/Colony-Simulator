using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World {

    public Tile[,] grid;

    public Tile GetTileAt(Vector2Int position) {
        if (!Utils.IsPositionViable(position)) {
            return null;
        }
        return grid[position.x, position.y];
    }

    public Tile GetTileAt(int x, int y) {
        if (!Utils.IsPositionViable(x, y)) {
            return null;
        }
        return grid[x, y];
    }
}