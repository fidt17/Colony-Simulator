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
        Vector2Int dimensions = Utils.WorldDimensions();
        nodes = new PathNode[dimensions.x, dimensions.y];
        for (int x = 0; x < dimensions.x; x++) {
            for (int y = 0; y < dimensions.y; y++) {
                nodes[x, y] = new PathNode(x, y, Utils.TileAt(x, y).isTraversable);
            }
        }
    }
}