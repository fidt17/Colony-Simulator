using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{   
    public PathGrid grid;
    public PathfinderRegionSystem regionSystem;
    private Vector2Int dimensions;

    public delegate void OnPathFound(List<PathNode> closedSet);
    public event OnPathFound PathHandler;

    public Pathfinder(Vector2Int dimensions) {

        this.dimensions = dimensions;

        grid = new PathGrid(dimensions);
    }

    public void CreateRegionSystem() {
        regionSystem = new PathfinderRegionSystem(dimensions);
    }

    public void UpdateSystem() {

        grid = new PathGrid(dimensions);
        regionSystem.UpdateSystem();
    }

    public List<PathNode> GetPath(Vector2Int start, Vector2Int target) {

        PathNode startNode  = grid.GetNodeAt(start);
        PathNode targetNode = grid.GetNodeAt(target);

        List<PathNode> openSet   = new List<PathNode>();
        List<PathNode> closedSet = new List<PathNode>();

        openSet.Add(startNode);

        while (openSet.Count > 0) {

            PathNode currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++) {

                if ((openSet[i].fCost() < currentNode.fCost() || openSet[i].fCost() == currentNode.fCost())
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

                if(closedSet.Contains(neighbour) || !neighbour.isTraversable)
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

    private List<PathNode> GetNeighbours2(PathNode node)
    {
        List<PathNode> neighbours = new List<PathNode>();
        int X = node.position.x;
        int Y = node.position.y;

        int GRID_SIZE = 100;

        int checkX = X;
        int checkY = Y-1;

        if((checkX >= 0 && checkX < GRID_SIZE) &&
            (checkY >= 0 && checkY < GRID_SIZE))
            neighbours.Add(grid.GetNodeAt(new Vector2Int(checkX, checkY)));

        checkX = X;
        checkY = Y+1;

        if((checkX >= 0 && checkX < GRID_SIZE) &&
            (checkY >= 0 && checkY < GRID_SIZE))
            neighbours.Add(grid.GetNodeAt(new Vector2Int(checkX, checkY)));

        checkX = X-1;
        checkY = Y;

        if((checkX >= 0 && checkX < GRID_SIZE) &&
            (checkY >= 0 && checkY < GRID_SIZE))
            neighbours.Add(grid.GetNodeAt(new Vector2Int(checkX, checkY)));

        checkX = X+1;
        checkY = Y;

        if((checkX >= 0 && checkX < GRID_SIZE) &&
            (checkY >= 0 && checkY < GRID_SIZE))
            neighbours.Add(grid.GetNodeAt(new Vector2Int(checkX, checkY)));

        return neighbours;
    }

    private int GetDistance(PathNode A, PathNode B) {

        int distX = Mathf.Abs(A.position.x - B.position.x);
        int distY = Mathf.Abs(A.position.y - B.position.y);

        if(distX > distY)
            return 14 * distY + 10 * (distX - distY);

        return 14 * distX + 10 * (distY - distX);
    }
}
