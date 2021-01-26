using UnityEngine;

namespace Pathfinding
{
	public static class PathGrid
	{
		private static Node[,] _nodes;
		private static bool    _isInitialized;

		public static Node NodeAt(int x, int y)
		{
			if (!_isInitialized || !Pathfinder.IsPositionViable(x, y))
			{
				return null;
			}

			return _nodes[x, y];
		}

		public static void CreateGrid(ref Tile[,] tileGrid)
		{
			_isInitialized = true;
			_nodes = new Node[Pathfinder.MapWidth, Pathfinder.MapHeight];
			for (int x = 0; x < Pathfinder.MapWidth; x++)
			{
				for (int y = 0; y < Pathfinder.MapHeight; y++)
				{
					_nodes[x, y] = new Node(x, y, tileGrid[x, y]);
				}
			}
		}
	}
}