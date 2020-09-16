using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class ItemFinder {

    //Temporal solution. I should change this to wave search later on.
    public static IEdible FindClosestFood(List<Type> canEat, Vector2Int sourcePosition, ref PathNode targetNode) {

        int minDistance = Int32.MaxValue;
        IEdible result = null;

        foreach(IEdible edible in GameManager.Instance.natureManager.edibleList) {
            
            if (!canEat.Contains(edible.GetType()))
                continue;

            int sqrDistance = (edible.GetEdiblePosition() - sourcePosition).sqrMagnitude;
            if (sqrDistance < minDistance) {

                PathNode characterPosition = GameManager.Instance.pathfinder.grid.GetNodeAt(sourcePosition);
                PathNode foodPosition = GameManager.Instance.pathfinder.grid.GetNodeAt(edible.GetEdiblePosition());

                if ( characterPosition.region != foodPosition.region )
                    continue;

                minDistance = sqrDistance;
                result = edible;
            }
        }

        if (result == null)
            return null;

        targetNode = FindNodeNear(GameManager.Instance.pathfinder.grid.GetNodeAt(result.GetEdiblePosition()), GameManager.Instance.pathfinder.grid.GetNodeAt(sourcePosition));

        return result;
    }

    public static PathNode FindNodeNear(PathNode searchNode, PathNode sourceNode) {

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