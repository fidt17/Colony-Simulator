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