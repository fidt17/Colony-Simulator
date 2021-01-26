using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public static class Dijkstra {

    public static List<Node> DijkstraFor(int steps, Node startNode) {
        List<Node> closedSet = new List<Node>();
        List<Node> openSet = new List<Node>();
        openSet.Add(startNode);

        while (closedSet.Count != steps) {
            int result = NextDijkstraIteration(ref openSet, ref closedSet, startNode, true);
            if (result == 0) {
                break;
            }
        }

        return closedSet;
    }

    public static int NextDijkstraIteration(ref List<Node> openSet, ref List<Node> closedSet, Node startNode, bool checkEqualityOfRegions) {
        if (openSet.Count == 0) {
            return 0;
        }

        Node initialNode = null;
        float minDistance = float.MaxValue;
        
        foreach (Node node in openSet) {
            float sqrMagnitude = (node.Position - startNode.Position).sqrMagnitude;
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
                Node checkNode = Utils.NodeAt(new Vector2Int(x, y));
                if (checkNode is null
                    || checkNode == initialNode
                    || openSet.Contains(checkNode)
                    || closedSet.Contains(checkNode)) {
                    continue;
                }
                if (checkEqualityOfRegions && checkNode.Region != startNode.Region) {
                    continue;
                }
                openSet.Insert(0, checkNode);
			}
		}

        closedSet.Add(initialNode);
        openSet.Remove(initialNode);
        return 1;
    }

    public static List<Subregion> DijkstraFor(int steps, Subregion startSubregion) {
        List<Subregion> closedSet = new List<Subregion>();
        List<Subregion> openSet = new List<Subregion>();
        openSet.Add(startSubregion);

        while (closedSet.Count != steps) {
            if (NextDijkstraIteration(ref openSet, ref closedSet) == false) {
                break;
            }
        }

        return closedSet;
    }

    public static bool NextDijkstraIteration(ref List<Subregion> openSet, ref List<Subregion> closedSet) {
        if (openSet.Count == 0) {
            return false;
        }

        Subregion subregion = openSet[0];
        openSet.Remove(subregion);
        closedSet.Insert(0, subregion);
        
        //Adding surrounding nodes to available list
        foreach (Subregion neighbour in subregion.NeighbouringSubregions) {
            if (openSet.Contains(neighbour) || closedSet.Contains(neighbour)) {
                continue;
            }
            openSet.Add(neighbour);
        }
        
        return true;
    }
}