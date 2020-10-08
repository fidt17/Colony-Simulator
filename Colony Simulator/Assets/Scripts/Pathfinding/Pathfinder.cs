using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder {

    public delegate void OnSystemUpdated();
    public delegate void OnPathFound(List<PathNode> closedSet);
    public static event OnPathFound PathHandler;
    public static event OnSystemUpdated UpdateHandler;

    public static PathGrid grid;
    public static PathfinderRegionSystem regionSystem;
    public static float systemUpdateCooldown = 0.5f;

    public static PathNode NodeAt(Vector2Int position) {
        if (Utils.IsPositionViable(position)) {
            PathNode n = grid?.nodes[position.x, position.y];
            return n;
        }
        return null;
    }
    
    private static Vector2Int _dimensions;
    private static bool _isUpdatingSystem = false;

    public static void Initialize(Vector2Int dimensions) {
        _dimensions = dimensions;
        float t1 = Time.realtimeSinceStartup;
        grid = new PathGrid(_dimensions);
        float gt = Time.realtimeSinceStartup - t1;
        regionSystem = new PathfinderRegionSystem(_dimensions);
        Debug.Log("Region Creation: " + (Time.realtimeSinceStartup - t1));
        systemUpdateCooldown = (Time.realtimeSinceStartup - t1) * 1.25f;
    }

    public static IEnumerator UpdateSystem() {
        if(_isUpdatingSystem) {
            yield break;
        }
        _isUpdatingSystem = true;

        yield return new WaitForSeconds(systemUpdateCooldown);

        regionSystem.UpdateSystem();
        _isUpdatingSystem = false;
        UpdateHandler?.Invoke();
    }

    public static List<PathNode> GetPath(PathNode startNode, PathNode targetNode) {
        List<PathNode> closedSet = new List<PathNode>();
        List<PathNode> path = AStarSearch.GetPath(startNode, targetNode, grid, ref closedSet);
        PathHandler?.Invoke(closedSet);
        return path;
    } 

    public static PathNode FindNodeNear(PathNode searchNode, PathNode sourceNode) {

        Vector2Int searchPosition = searchNode.position;

        Vector2Int checkN = new Vector2Int(searchPosition.x, searchPosition.y + 1);
        Vector2Int checkS = new Vector2Int(searchPosition.x, searchPosition.y - 1);
        Vector2Int checkW = new Vector2Int(searchPosition.x - 1, searchPosition.y);
        Vector2Int checkE = new Vector2Int(searchPosition.x + 1, searchPosition.y);

        List<PathNode> possiblePositions = new List<PathNode>();
        possiblePositions.Add(Pathfinder.NodeAt(checkN));
        possiblePositions.Add(Pathfinder.NodeAt(checkS));
        possiblePositions.Add(Pathfinder.NodeAt(checkW));
        possiblePositions.Add(Pathfinder.NodeAt(checkE));

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

        if (result is null) {
            return Pathfinder.NodeAt(searchPosition);
        }

        return result;
    }
}