using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode {

    public int X => position.x;
    public int Y => position.y;
    public Tile Tile => Utils.TileAt(position);

    public Vector2Int position;

    public Region region;
    public PathNode parent;

    public bool isTraversable;
    public int gCost, hCost;
    public int fCost => gCost + hCost;

    public PathNode(Vector2Int position, bool isTraversable) {
        this.position = position;
        this.isTraversable = isTraversable;
        gCost = 0;
        hCost = 0;
    }
}