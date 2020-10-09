using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DijkstraSearch {

    public static List<PathNode> DijkstraFor(int steps, PathNode startNode) {

        List<PathNode> closedSet = new List<PathNode>();
        List<PathNode> openSet = new List<PathNode>();
        openSet.Add(startNode);

        while (closedSet.Count != steps) {
            int result = NextDijkstraIteration(ref openSet, ref closedSet, startNode, true);
            if (result == 0) {
                break;
            }
        }

        return closedSet;
    }

    public static Tile FindClosestTileWhere(Vector2Int startPosition, Func<Tile, bool> requirementsFunction, bool checkEqualityOfRegions = true) {
        int checkIndex = 0;
        List<PathNode> closedSet = new List<PathNode>();
        List<PathNode> openSet = new List<PathNode>();
        openSet.Add(Utils.NodeAt(startPosition));

        Tile checkTile = null;
        do {
            NextDijkstraIteration(ref openSet, ref closedSet, Utils.NodeAt(startPosition), checkEqualityOfRegions);
            if (checkIndex >= closedSet.Count) {
                return null;
            }
            checkTile = Utils.TileAt(closedSet[checkIndex].position);
            checkIndex++;
        } while (requirementsFunction(checkTile) != true);

        return checkTile;
    }

    private static int NextDijkstraIteration(ref List<PathNode> openSet, ref List<PathNode> closedSet, PathNode startNode, bool checkEqualityOfRegions) {
        if (openSet.Count == 0) {
            return 0;
        }

        PathNode initialNode = null;
        float minDistance = float.MaxValue;
        
        foreach (PathNode node in openSet) {
            float sqrMagnitude = (node.position - startNode.position).sqrMagnitude;
            if (sqrMagnitude < minDistance) {
                minDistance = sqrMagnitude;
                initialNode = node;
            }
        }

        if (initialNode is null) {
            return 0;
        }
        
        //Adding surrounding nodes to available list
        for (int x = initialNode.X - 1; x <= initialNode.X + 1; x++) {
            for(int y = initialNode.Y - 1; y <= initialNode.Y + 1; y++) {
                PathNode checkNode = Pathfinder.NodeAt(new Vector2Int(x, y));
                if (checkNode is null
                    || checkNode == initialNode
                    || openSet.Contains(checkNode)
                    || closedSet.Contains(checkNode)) {
                    continue;
                }
                if (checkEqualityOfRegions && checkNode.region != startNode.region) {
                    continue;
                }
                openSet.Insert(0, checkNode);
			}
		}

        closedSet.Add(initialNode);
        openSet.Remove(initialNode);
        return 1;
    }
}