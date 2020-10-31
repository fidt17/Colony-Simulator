using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStarSubregionSearch {

    public static List<Subregion> GetPath(Subregion startSubregion, Subregion targetSubregion) {
        
        if (startSubregion.region != targetSubregion.region) {
            return null;
        }

        if (startSubregion == targetSubregion) {
            List<Subregion> result = new List<Subregion>();
            result.Add(startSubregion);
            return result;
        }

        List<Subregion> openSet = new List<Subregion>();
        List<Subregion> closedSet = new List<Subregion>();
        openSet.Add(startSubregion);
        while (openSet.Count > 0) {
            Subregion currentSubregion = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {
                if ((openSet[i].fCost <= currentSubregion.fCost)
                    && (openSet[i].hCost < currentSubregion.hCost)) {
                    currentSubregion = openSet[i];
                }
            }

            openSet.Remove(currentSubregion);
            closedSet.Add(currentSubregion);
            if (currentSubregion == targetSubregion) {
                return RetracePath(startSubregion, targetSubregion);
            }

            foreach(Subregion neighbour in currentSubregion.neighbouringSubregions) {
                if(closedSet.Contains(neighbour)) {
                    continue;
                }

                int newMovementCostToNeighbour = currentSubregion.gCost + GetDistance(currentSubregion, neighbour);
                if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetSubregion);
                    neighbour.parent = currentSubregion;

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

    private static int GetDistance(Subregion subA, Subregion subB) {

        PathNode A = subA.nodes[subA.nodes.Count / 2];
        PathNode B = subB.nodes[subB.nodes.Count / 2];
        
        int distX = Mathf.Abs(A.position.x - B.position.x);
        int distY = Mathf.Abs(A.position.y - B.position.y);

        if(distX > distY) {
            return 14 * distY + 10 * (distX - distY);
        }

        return 14 * distX + 10 * (distY - distX);
    }

    private static List<Subregion> RetracePath(Subregion startSubregion, Subregion endSubregion) {
        List<Subregion> path = new List<Subregion>();
        Subregion currentSubregion = endSubregion;
        while (currentSubregion != startSubregion) {
            path.Add(currentSubregion);
            currentSubregion = currentSubregion.parent;
        }
        path.Add(startSubregion);

        path.Reverse();
        return path;
    }
}