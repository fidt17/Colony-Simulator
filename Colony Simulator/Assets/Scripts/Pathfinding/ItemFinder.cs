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
            if (!canEat.Contains(edible.GetType())) {
                continue;
            }

            int sqrDistance = (edible.GetEdiblePosition() - sourcePosition).sqrMagnitude;
            if (sqrDistance < minDistance) {
                PathNode characterPosition = GameManager.Instance.pathfinder.grid.GetNodeAt(sourcePosition);
                PathNode foodPosition = GameManager.Instance.pathfinder.grid.GetNodeAt(edible.GetEdiblePosition());
                if (characterPosition.region != foodPosition.region) {
                    continue;
                }

                minDistance = sqrDistance;
                result = edible;
            }
        }

        if (result is null) {
            return null;
        }

        targetNode = GameManager.Instance.pathfinder.FindNodeNear(
                                GameManager.Instance.pathfinder.grid.GetNodeAt(result.GetEdiblePosition()), GameManager.Instance.pathfinder.grid.GetNodeAt(sourcePosition));

        return result;
    }
}