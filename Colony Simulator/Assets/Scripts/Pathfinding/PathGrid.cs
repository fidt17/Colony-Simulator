﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGrid
{
    public PathNode[,] nodes;
    private Vector2Int dimensions;

    public PathGrid(Vector2Int dimensions) {

        this.dimensions = dimensions;

        CreateGrid();
    }

    public void CreateGrid() {

        nodes = new PathNode[dimensions.x, dimensions.y];

        for (int x = 0; x < dimensions.x; x++) {
            for (int y = 0; y < dimensions.y; y++) {
                
                Vector2Int position = new Vector2Int(x, y);
                bool isTraversable = GameManager.Instance.world.GetTileAt(position).isTraversable;

                PathNode node = new PathNode(position, isTraversable);
                nodes[x, y] = node;
            }
        }
    }

    public PathNode GetNodeAt(Vector2Int position) {

        if (GameManager.Instance.world.IsPositionViable(position)) {
            
            PathNode n = nodes[position.x, position.y];
            return n;
        }

        return null;
    }
}