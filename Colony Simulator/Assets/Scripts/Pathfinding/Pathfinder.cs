using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder {

    public delegate void OnPathFound(List<PathNode> closedSet);
    public event OnPathFound PathHandler;

    public PathGrid grid;
    public PathfinderRegionSystem regionSystem;
    
    private Vector2Int _dimensions;

    public Pathfinder(Vector2Int dimensions) {

        _dimensions = dimensions;
        grid = new PathGrid(_dimensions);
    }

    public void CreateRegionSystem() => regionSystem = new PathfinderRegionSystem(_dimensions);

    private bool _isUpdatingSystem = false;
    public IEnumerator UpdateSystem() {

        if(_isUpdatingSystem)
            yield break;

        _isUpdatingSystem = true;

        yield return new WaitForSeconds(0.25f);

        grid = new PathGrid(_dimensions);
        regionSystem.UpdateSystem();

        _isUpdatingSystem = false;
    }

    public List<PathNode> GetPath(Vector2Int start, Vector2Int target) {
        
        PathNode startNode  = grid.GetNodeAt(start);
        PathNode targetNode = grid.GetNodeAt(target);

        if (startNode.region != targetNode.region)
            return null;

        if (startNode == targetNode)
            return new List<PathNode>();

        List<PathNode> openSet   = new List<PathNode>();
        List<PathNode> closedSet = new List<PathNode>();

        openSet.Add(startNode);

        while (openSet.Count > 0) {

            PathNode currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {

                if ((openSet[i].fCost() <= currentNode.fCost())
                    && (openSet[i].hCost < currentNode.hCost))
                        currentNode = openSet[i];
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode) {
                List<PathNode> path = RetracePath(startNode, targetNode);
                
                PathHandler?.Invoke(closedSet);
                return path;
            }

            foreach(PathNode neighbour in GetNeighbours(currentNode)) {

                if(closedSet.Contains(neighbour))
                    continue;

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {

                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return null;
    }

    private List<PathNode> RetracePath(PathNode startNode, PathNode endNode) {

        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    private List<PathNode> GetNeighbours(PathNode node) {

		List<PathNode> neighbours = new List<PathNode>();
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {

				if( x == 0 && y == 0 )
					continue;

				int checkX = node.position.x + x;
				int checkY = node.position.y + y;

                PathNode n = grid.GetNodeAt(new Vector2Int(checkX, checkY));

                if (n != null && n.isTraversable)
					neighbours.Add(n);
			}
		}

		return neighbours;
	}

    private int GetDistance(PathNode A, PathNode B) {

        int distX = Mathf.Abs(A.position.x - B.position.x);
        int distY = Mathf.Abs(A.position.y - B.position.y);

        if(distX > distY)
            return 14 * distY + 10 * (distX - distY);

        return 14 * distX + 10 * (distY - distX);
    }

    public PathNode FindNodeNear(PathNode searchNode, PathNode sourceNode) {

        Vector2Int searchPosition = searchNode.position;

        Vector2Int checkN = new Vector2Int(searchPosition.x, searchPosition.y + 1);
        Vector2Int checkS = new Vector2Int(searchPosition.x, searchPosition.y - 1);
        Vector2Int checkW = new Vector2Int(searchPosition.x - 1, searchPosition.y);
        Vector2Int checkE = new Vector2Int(searchPosition.x + 1, searchPosition.y);

        List<PathNode> possiblePositions = new List<PathNode>();
        possiblePositions.Add(GameManager.Instance.pathfinder.grid.GetNodeAt(checkN));
        possiblePositions.Add(GameManager.Instance.pathfinder.grid.GetNodeAt(checkS));
        possiblePositions.Add(GameManager.Instance.pathfinder.grid.GetNodeAt(checkW));
        possiblePositions.Add(GameManager.Instance.pathfinder.grid.GetNodeAt(checkE));

        possiblePositions.ToList().ForEach(x => {
            if (x == null || x.region != sourceNode.region)
                possiblePositions.Remove(x);
        });

        int minDistance = Int32.MaxValue;
        PathNode result = null;

        foreach(PathNode node in possiblePositions) {

            int sqrDistance = (node.position - sourceNode.position).sqrMagnitude;
            if (sqrDistance < minDistance) {

                minDistance = sqrDistance;
                result = node;
            }
        }

        if (result == null)
            return GameManager.Instance.pathfinder.grid.GetNodeAt(searchPosition);

        return result;
    }
}
