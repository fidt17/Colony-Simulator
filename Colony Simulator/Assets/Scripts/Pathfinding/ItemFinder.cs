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
        foreach(IEdible edible in GameManager.GetInstance().natureManager.edibleList) {
            if (!canEat.Contains(edible.GetType())) {
                continue;
            }

            int sqrDistance = (edible.GetEdiblePosition() - sourcePosition).sqrMagnitude;
            if (sqrDistance < minDistance) {
                PathNode characterPosition = Utils.NodeAt(sourcePosition);
                PathNode foodPosition = Utils.NodeAt(edible.GetEdiblePosition());
                if (characterPosition.Region != foodPosition.Region) {
                    continue;
                }

                minDistance = sqrDistance;
                result = edible;
            }
        }

        if (result is null) {
            return null;
        }

        targetNode = Pathfinder.FindNodeNear(
                                Utils.NodeAt(result.GetEdiblePosition()), Utils.NodeAt(sourcePosition));

        return result;
    }
}