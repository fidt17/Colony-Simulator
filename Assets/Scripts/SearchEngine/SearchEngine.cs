using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SearchEngine {

    private static Dictionary<string, Type> _cashedTypesByName = new Dictionary<string, Type>();

    public static Tile FindClosestTileWhere(Vector2Int startPosition, Func<Tile, bool> requirementsFunction, bool checkEqualityOfRegions = true) {
        int checkIndex = 0;
        List<PathNode> closedSet = new List<PathNode>();
        List<PathNode> openSet = new List<PathNode>();
        openSet.Add(Utils.NodeAt(startPosition));

        Tile checkTile = null;
        do {
            Dijkstra.NextDijkstraIteration(ref openSet, ref closedSet, Utils.NodeAt(startPosition), checkEqualityOfRegions);
            if (checkIndex >= closedSet.Count) {
                return null;
            }
            checkTile = Utils.TileAt(closedSet[checkIndex].position);
            checkIndex++;
        } while (requirementsFunction(checkTile) != true);

        return checkTile;
    }

    public static Tile FindClosestBySubregionTileWhere(Vector2Int sourcePosition, Func<Tile, bool> requirementsFunction, bool checkEqualityOfRegions = true) {
        
        List<Subregion> closedSet = new List<Subregion>();
        List<Subregion> openSet = new List<Subregion>();
        openSet.Add(Utils.NodeAt(sourcePosition.x, sourcePosition.y).subregion);

        while (Dijkstra.NextDijkstraIteration(ref openSet, ref closedSet)) {
            Subregion subregion = closedSet[0];
            foreach (PathNode node in subregion.nodes) {
                Tile checkTile = Utils.TileAt(node.x, node.y);
                if (requirementsFunction(checkTile) == true) {
                    return checkTile;
                }
            }
        }
        return null;
    }

    public static Grass FindClosestGrass(Vector2Int sourcePosition) {
        List<Subregion> closedSet = new List<Subregion>();
        List<Subregion> openSet = new List<Subregion>();
        openSet.Add(Utils.NodeAt(sourcePosition.x, sourcePosition.y).subregion);

        Grass grass = null;
        while (grass == null && Dijkstra.NextDijkstraIteration(ref openSet, ref closedSet)) {
            List<Grass> grassList = closedSet[0].content.Get<Grass>();
            if (grassList != null && grassList.Count > 0) {
                grass = grassList[0];
            }
        }

        return grass;
    }

    public static Type GetTypeDerivativeOf<T>(string targetTypeName) {
        Type type = null;
        if (_cashedTypesByName.ContainsKey(targetTypeName)) {
            type = _cashedTypesByName[targetTypeName];
        } else {
            List<Type> types = System.Reflection.Assembly.GetAssembly(typeof(T)).GetTypes().Where(myType => myType.IsSubclassOf(typeof(T))).ToList();
            foreach(Type checkType in types) {
                string typeName = checkType.ToString();
                if (typeName == targetTypeName) {
                    type = checkType;
                    _cashedTypesByName.Add(targetTypeName, type);
                    break;
                }
            }
        }
        return type;
    }
}