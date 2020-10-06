using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGrid {

    public PathNode[,] nodes;
    
    private Vector2Int _dimensions;

    public PathGrid(Vector2Int dimensions) {
        _dimensions = dimensions;
        CreateGrid();
    }

    public void CreateGrid() {
        nodes = new PathNode[_dimensions.x, _dimensions.y];
        for (int x = 0; x < _dimensions.x; x++) {
            for (int y = 0; y < _dimensions.y; y++) {
                Vector2Int position = new Vector2Int(x, y);
                bool isTraversable = GameManager.GetInstance().world.GetTileAt(position).isTraversable;
                PathNode node = new PathNode(position, isTraversable);
                nodes[x, y] = node;
            }
        }
    }
}