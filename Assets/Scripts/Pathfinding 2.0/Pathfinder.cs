using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	public static class Pathfinder
	{
		public delegate void OnPathFound(List<Node> closedSet);
		public static event OnPathFound PathHandler;
		
		public static int MapWidth, MapHeight;
		
		public static bool IsPositionViable(int x, int y)
		{
			return x >= 0 && y >= 0 && x < MapWidth && y < MapHeight;
		}

		public static void Initialize(int mapWidth, int mapHeight, ref Tile[,] tileGrid)
		{
			MapWidth = mapWidth;
			MapHeight = mapHeight;

			PathGrid.CreateGrid(ref tileGrid);
			RegionSystem.Initialize();
		}

		public static void Reset()
		{
			RegionSystem.Reset();
		}

		public static List<Node> GetPath(Vector2Int startPosition, Vector2Int targetPosition)
		{
			var startNode = PathGrid.NodeAt(startPosition.x, startPosition.y);
			var targetNode = PathGrid.NodeAt(targetPosition.x, targetPosition.y);
			var path = RPathfinding.GetPath(startNode, targetNode);
			PathHandler?.Invoke(path);
			return path;
		}
		
		public static bool CompareCharacterRegionWith(Character character, Region r)
		{
			Node characterNode = character.MotionComponent.Node;
			if (characterNode.IsTraversable == false)
			{
				character.MotionComponent.MoveCharacterToTraversableTile();
			}
			return characterNode.Region == r;
		}
	}
}