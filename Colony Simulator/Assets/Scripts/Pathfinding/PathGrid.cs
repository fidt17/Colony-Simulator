using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathGrid {

    public static PathNode[,] nodes;
    private static bool _isInitialized = false;

    public static PathNode NodeAt(Vector2Int position) {
        if (_isInitialized == false) {
            return null;
        }

        if (Utils.IsPositionViable(position)) {
            return nodes[position.x, position.y];
        }
        return null;
    }

    public static PathNode NodeAt(int x, int y) {
        if (_isInitialized == false) {
            return null;
        }
        
        if (Utils.IsPositionViable(x, y)) {
            return nodes[x, y];
        }
        return null;
    }

    public static void CreateGrid() {
        _isInitialized = true;
        nodes = new PathNode[Utils.MapSize, Utils.MapSize];
        for (int x = 0; x < Utils.MapSize; x++) {
            for (int y = 0; y < Utils.MapSize; y++) {
                nodes[x, y] = new PathNode(x, y, Utils.TileAt(x, y).isTraversable);
            }
        }
    }
}