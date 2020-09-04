using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public bool isTraversable;
    public Vector2Int position;

    public int gCost, hCost;

    public PathNode parent = null;

    public Region region = null;

    public PathNode(Vector2Int position, bool isTraversable) {

        this.position = position;
        this.isTraversable = isTraversable;
        gCost = 0;
        hCost = 0;
    }

    public Tile GetTile() {
        return GameManager.Instance.world.GetTileAt(position);
    }

    public int fCost() {
        return gCost + hCost;
    }
}
