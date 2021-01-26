using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	public static class ASubregionSearch
	{
		public static List<Subregion> GetPath(Subregion startSubregion, Subregion targetSubregion)
		{
			if (startSubregion.Region != targetSubregion.Region)
			{
				return null;
			}

			if (startSubregion == targetSubregion)
			{
				List<Subregion> result = new List<Subregion>();
				result.Add(startSubregion);
				return result;
			}

			MinHeap<Subregion> openSet = new MinHeap<Subregion>(startSubregion.Region.Subregions.Count);
			HashSet<Subregion> closedSet = new HashSet<Subregion>();
			openSet.Add(startSubregion);
			while (openSet.Count > 0)
			{
				Subregion currentSubregion = openSet.RemoveFirst();

				closedSet.Add(currentSubregion);

				if (currentSubregion == targetSubregion)
				{
					return RetracePath(startSubregion, targetSubregion);
				}

				foreach (Subregion neighbour in currentSubregion.NeighbouringSubregions)
				{
					if (closedSet.Contains(neighbour))
					{
						continue;
					}

					int newMovementCostToNeighbour = currentSubregion.gCost + GetDistance(currentSubregion, neighbour);
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
					{
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetSubregion);
						neighbour.ParentSubregion = currentSubregion;

						if (!openSet.Contains(neighbour))
						{
							openSet.Add(neighbour);
						}
					}
				}
			}

			return null;
		}

		private static int GetDistance(Subregion subA, Subregion subB)
		{
			Node A = PathGrid.NodeAt(subA.AvergX, subA.AvergY);
			Node B = PathGrid.NodeAt(subB.AvergX, subB.AvergY);

			return RPathfinding.GetDistance(A, B);
		}

		private static List<Subregion> RetracePath(Subregion startSubregion, Subregion endSubregion)
		{
			List<Subregion> path = new List<Subregion>();
			Subregion currentSubregion = endSubregion;
			while (currentSubregion != startSubregion)
			{
				path.Add(currentSubregion);
				currentSubregion.ParentSubregion.Child = currentSubregion;
				currentSubregion = currentSubregion.ParentSubregion;
			}
			path.Add(startSubregion);
			return path;
		}
	}
}