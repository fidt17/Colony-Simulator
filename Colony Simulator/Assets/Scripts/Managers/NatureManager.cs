using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NatureManager : MonoBehaviour
{
    public List<Grass> grass = new List<Grass>();

    public Grass FindClosestFood(List<Type> edibles, Vector2Int sourcePosition) {

        //temporal solution. I should change this to wave search later on.

        int minDistance = 100 * 100;
        Grass result = null;

        foreach (Grass e in grass) {

            int sqrDistance = (e.position - sourcePosition).sqrMagnitude;

            if (sqrDistance < minDistance) {

                PathNode characterPosition = GameManager.Instance.pathfinder.grid.GetNodeAt(sourcePosition);
                PathNode foodPosition = GameManager.Instance.pathfinder.grid.GetNodeAt(e.position);

                if ( characterPosition.region != foodPosition.region )
                    continue;

                minDistance = sqrDistance;
                result = e;
            }
        }

        return result;
    }
}
