using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder {

    public delegate void OnPathFound(List<PathNode> closedSet);
    public static event OnPathFound PathHandler;

    public static void Initialize() {
        float t1 = Time.realtimeSinceStartup;
        PathGrid.CreateGrid();
        RegionSystem.Initialize();
        //Debug.Log("Region System was created in: " + (Time.realtimeSinceStartup - t1) + " sec.");
    }

    public static void UpdateSystemAt(int x, int y) {
        float t1 = Time.realtimeSinceStartup;
        RegionSystem.UpdateSystemAt(x, y);
        //Debug.Log("Region System was updated in: " + (Time.realtimeSinceStartup - t1) + " sec.");
    }

    public static List<PathNode> GetPath(PathNode startNode, PathNode targetNode) {
        List<PathNode> closedSet = new List<PathNode>();
        List<PathNode> path = AStarSearch.GetPath(startNode, targetNode, ref closedSet);
        PathHandler?.Invoke(closedSet);
        return path;
    } 

    public static PathNode FindNodeNear(PathNode searchNode, PathNode sourceNode) {
        PathNode result = null;
        int minDistance = Int32.MaxValue;
        List<PathNode> neighbours = searchNode.GetNeighbours();
        for (int i = neighbours.Count - 1; i >= 0; i--) {
            if (neighbours[i].Region != sourceNode.Region) {
                neighbours.RemoveAt(i);
                continue;
            }
            int sqrDistance = (neighbours[i].position - sourceNode.position).sqrMagnitude;
            if (sqrDistance < minDistance) {
                minDistance = sqrDistance;
                result = neighbours[i];
            }
        }
        return result;
    }

    public static bool CompareCharacterRegionWith(Character character, Region r) {
        PathNode characterNode = character.MotionComponent.PathNode;

        if (characterNode.isTraversable == false) {
            character.MotionComponent.MoveCharacterToTraversableTile();
        }

        return characterNode.Region == r;
    }
}