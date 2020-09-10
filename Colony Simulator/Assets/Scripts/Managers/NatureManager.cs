using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NatureManager : MonoBehaviour
{
    #region Food

    public List<IEdible> edibleList = new List<IEdible>();

    #endregion

    #region Vegetation
    
    public List<Grass> grassList = new List<Grass>();

    #endregion

    public IEdible FindClosestFood(List<Type> canEat, Vector2Int sourcePosition) {

        //temporal solution. I should change this to wave search later on.

        int minDistance = 100 * 100;
        IEdible result = null;

        foreach(IEdible edible in edibleList) {
            
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

        return result;
    }
}
