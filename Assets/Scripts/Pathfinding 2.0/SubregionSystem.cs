using System.Collections.Generic;

namespace Pathfinding
{
	public static class SubregionSystem
	{
		public static List<Subregion> Subregions { get; } = new List<Subregion>();

		private const int SubregionSize = 10;

		public static void Reset()
		{
			Subregions.Clear();
		}

		public static void CreateSubregions()
		{
			for (int x = 0; x < Pathfinder.MapWidth; x += 10)
			{
				for (int y = 0; y < Pathfinder.MapHeight; y += 10)
				{
					CreateSubregionAt(x, y);
				}
			}

			foreach (Subregion subregion in Subregions)
			{
				subregion.FindNeighbours();
			}
		}

		public static void UpdateSubregionAt(int x, int y)
		{
			foreach (Subregion newSubregion in CreateSubregionAt(x, y))
			{
				newSubregion.FindNeighbours();
			}
		}

		public static void RemoveSubregion(Subregion subregion)
		{
			Subregions.Remove(subregion);
		}

		private static List<Subregion> CreateSubregionAt(int X, int Y)
		{
			int subregionStartX = X / SubregionSize * SubregionSize;
			int subregionStartY = Y / SubregionSize * SubregionSize;

			//Deleting any subregions that existed on 10x10 chunk
			for (int x = subregionStartX; x < subregionStartX + SubregionSize; x++)
			{
				for (int y = subregionStartY; y < subregionStartY + SubregionSize; y++)
				{
					PathGrid.NodeAt(x, y)?.Subregion?.Reset();
				}
			}

			//Creating new subregions in this chunk
			List<Subregion> createdSubregions = new List<Subregion>();
			for (int x = subregionStartX; x < subregionStartX + SubregionSize; x++)
			{
				for (int y = subregionStartY; y < subregionStartY + SubregionSize; y++)
				{
					Subregion newSubregion = FillSubregionFrom(PathGrid.NodeAt(x, y));
					if (newSubregion == null) continue;
					newSubregion.CalculateAverageCoordinates();
					createdSubregions.Add(newSubregion);
				}
			}

			return createdSubregions;
		}

		private static Subregion FillSubregionFrom(Node node)
		{
			if (node is null || node.Subregion != null || !node.IsTraversable)
			{
				return null;
			}

			Subregion subregion = new Subregion();
			List<Node> openNodes = new List<Node>();
			openNodes.Add(node);

			while (FloodFill(openNodes, subregion))
			{ }

			Subregions.Add(subregion);
			return subregion;
		}

		private static bool FloodFill(List<Node> openSet, Subregion subregion)
		{
			for (int i = openSet.Count - 1; i >= 0; i--)
			{
				if (openSet[i].Subregion != null)
				{
					openSet.RemoveAt(i);
				}
			}

			if (openSet.Count == 0)
			{
				return false;
			}

			foreach (Node neighbour in openSet[0].GetNeighbours())
			{
				if (!neighbour.IsTraversable || neighbour.Subregion != null
				                             || !IsInsideArea(neighbour.X, neighbour.Y,
				                                              openSet[0].X / SubregionSize * SubregionSize,
				                                              openSet[0].Y / SubregionSize * SubregionSize))
					continue;
				openSet.Add(neighbour);
			}

			subregion.AddNode(openSet[0]);
			openSet.RemoveAt(0);

			return true;
		}

		private static bool IsInsideArea(int x, int y, int subX, int subY)
		{
			return x >= subX && y >= subY && x < subX + SubregionSize && y < subY + SubregionSize;
		}
	}
}