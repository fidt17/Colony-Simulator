using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pathfinding;
using UnityEngine;

public static class SearchEngine
{
	private static readonly Dictionary<string, Type> _cashedTypesByName = new Dictionary<string, Type>();

	public static Node FindNodeNear(Node searchNode, Node sourceNode)
	{
		Node result = null;
		int minDistance = int.MaxValue;
		List<Node> neighbours = searchNode.GetNeighbours();
		for (int i = neighbours.Count - 1; i >= 0; i--)
		{
			if (neighbours[i].Region != sourceNode.Region)
			{
				neighbours.RemoveAt(i);
				continue;
			}

			int sqrDistance = (neighbours[i].Position - sourceNode.Position).sqrMagnitude;
			if (sqrDistance < minDistance)
			{
				minDistance = sqrDistance;
				result = neighbours[i];
			}
		}

		return result;
	}

	public static Tile FindClosestTileWhere(Vector2Int startPosition, Func<Tile, bool> requirementsFunction,
	                                        bool checkEqualityOfRegions = true)
	{
		int checkIndex = 0;
		List<Node> closedSet = new List<Node>();
		List<Node> openSet = new List<Node>();
		openSet.Add(Utils.NodeAt(startPosition));

		Tile checkTile = null;
		do
		{
			Dijkstra.NextDijkstraIteration(ref openSet, ref closedSet, Utils.NodeAt(startPosition),
			                               checkEqualityOfRegions);
			if (checkIndex >= closedSet.Count) return null;
			checkTile = Utils.TileAt(closedSet[checkIndex].Position);
			checkIndex++;
		} while (requirementsFunction(checkTile) != true);

		return checkTile;
	}

	public static Tile FindClosestBySubregionTileWhere(Vector2Int sourcePosition, Func<Tile, bool> requirementsFunction,
	                                                   bool checkEqualityOfRegions = true)
	{
		List<Subregion> closedSet = new List<Subregion>();
		List<Subregion> openSet = new List<Subregion>();
		openSet.Add(Utils.NodeAt(sourcePosition.x, sourcePosition.y).Subregion);

		while (Dijkstra.NextDijkstraIteration(ref openSet, ref closedSet))
		{
			Subregion subregion = closedSet[0];
			foreach (Node node in subregion.Nodes)
			{
				Tile checkTile = Utils.TileAt(node.X, node.Y);
				if (requirementsFunction(checkTile)) return checkTile;
			}
		}

		return null;
	}

	//TODO: Remove later
	public static Grass FindClosestGrass(Vector2Int sourcePosition)
	{
		List<Subregion> closedSet = new List<Subregion>();
		List<Subregion> openSet = new List<Subregion>();
		openSet.Add(Utils.NodeAt(sourcePosition.x, sourcePosition.y).Subregion);

		Grass grass = null;
		while (grass == null && Dijkstra.NextDijkstraIteration(ref openSet, ref closedSet))
		{
			List<Grass> grassList = closedSet[0].Content.Get<Grass>();
			if (grassList != null && grassList.Count > 0) grass = grassList[0];
		}

		return grass;
	}

	public static Type GetTypeDerivativeOf<T>(string targetTypeName)
	{
		Type type = null;
		if (_cashedTypesByName.ContainsKey(targetTypeName))
		{
			type = _cashedTypesByName[targetTypeName];
		}
		else
		{
			List<Type> types = Assembly.GetAssembly(typeof(T)).GetTypes()
			                           .Where(myType => myType.IsSubclassOf(typeof(T))).ToList();
			foreach (Type checkType in types)
			{
				string typeName = checkType.ToString();
				if (typeName == targetTypeName)
				{
					type = checkType;
					_cashedTypesByName.Add(targetTypeName, type);
					break;
				}
			}
		}

		return type;
	}
}