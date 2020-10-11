using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStarSearch {

    public static List<PathNode> GetPath(PathNode startNode, PathNode targetNode, ref List<PathNode> closedSet) {
        
        if (startNode.Region != targetNode.Region) {
            return null;
        }

        if (startNode == targetNode) {
            return new List<PathNode>();
        }

        List<PathNode> openSet   = new List<PathNode>();
        openSet.Add(startNode);
        while (openSet.Count > 0) {
            PathNode currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {
                if ((openSet[i].fCost <= currentNode.fCost)
                    && (openSet[i].hCost < currentNode.hCost)) {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            if (currentNode == targetNode) {
                return RetracePath(startNode, targetNode);
            }

            foreach(PathNode neighbour in GetNeighbours(currentNode)) {
                if(closedSet.Contains(neighbour)) {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour)) {
                        openSet.Add(neighbour);
                    }
                }
            }
        }

        return null;
    }

    private static List<PathNode> GetNeighbours(PathNode node) {
		List<PathNode> neighbours = new List<PathNode>();
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {

				if( x == 0 && y == 0 ) {
					continue;
                }

				int checkX = node.position.x + x;
				int checkY = node.position.y + y;
                PathNode n = Utils.NodeAt(new Vector2Int(checkX, checkY));
                if (n != null && n.isTraversable) {
					neighbours.Add(n);
                }
			}
		}
		return neighbours;
	}

    private static int GetDistance(PathNode A, PathNode B) {

        int distX = Mathf.Abs(A.position.x - B.position.x);
        int distY = Mathf.Abs(A.position.y - B.position.y);

        if(distX > distY) {
            return 14 * distY + 10 * (distX - distY);
        }

        return 14 * distX + 10 * (distY - distX);
    }

    private static List<PathNode> RetracePath(PathNode startNode, PathNode endNode) {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;
        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }
}