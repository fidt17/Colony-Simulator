using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode {

    public Vector2Int position;

    public Region region;
    public PathNode parent;

    public bool isTraversable;
    public int gCost, hCost;

    public PathNode(Vector2Int position, bool isTraversable) {

        this.position = position;
        this.isTraversable = isTraversable;
        gCost = 0;
        hCost = 0;
    }

    public Tile GetTile() => GameManager.Instance.world.GetTileAt(position);

    public int fCost() => gCost + hCost;
}
