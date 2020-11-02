using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : IHeapItem<PathNode> {

    public Tile Tile => Utils.TileAt(position);

    public Vector2Int position;
    public int x;
    public int y;

    public Subregion subregion;
    public Region Region => subregion?.region;
    public PathNode parent;

    public bool isTraversable;
    public int  gCost, hCost;
    public int  heapIndex;
    public int  fCost => gCost + hCost;

    public PathNode(int x, int y, bool isTraversable) {
        position = new Vector2Int(x, y);
        this.x = position.x;
        this.y = position.y;
        this.isTraversable = isTraversable;
        gCost = 0;
        hCost = 0;
    }

    public List<PathNode> GetNeighbours() {
        List<PathNode> neighbours = new List<PathNode>() {
            Utils.NodeAt(x, y + 1),
            Utils.NodeAt(x + 1, y),
            Utils.NodeAt(x, y - 1),
            Utils.NodeAt(x - 1, y)
        };

        for (int i = neighbours.Count - 1; i >= 0; i--) {
            if (neighbours[i] == null) {
                neighbours.RemoveAt(i);
            }
        }

        return neighbours;
    }

    public int CompareTo(PathNode nodeToCompare) {
        int compare = hCost.CompareTo(nodeToCompare.hCost);
        if (compare == 0) {
            compare = fCost.CompareTo(nodeToCompare.fCost);
        }
        return -compare;
    }

    public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }
}