using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	public static class RegionSystem
	{
		public static List<Region> Regions { get; private set; }

		public static void Initialize()
		{
			SubregionSystem.CreateSubregions();
			CreateRegions();
		}

		public static void Reset()
		{
			Regions?.Clear();
			SubregionSystem.Reset();
		}

		public static void UpdateSystemAt(int x, int y)
		{
			SubregionSystem.UpdateSubregionAt(x, y);
			ResetRegions();
			CreateRegions();
		}

		private static void CreateRegions()
		{
			Regions = new List<Region>();
			foreach (Subregion subregion in SubregionSystem.Subregions)
			{
				if (subregion.Region is null)
				{
					Regions.Add(CreateRegionAt(subregion));
				}
			}
		}

		private static void ResetRegions()
		{
			if (Regions is null)
			{
				return;
			}
			
			for (int i = Regions.Count - 1; i >= 0; i--)
			{
				Regions[i].Reset();
			}
		}

		private static Region CreateRegionAt(Subregion subregion)
		{
			Region region = new Region();
			List<Subregion> openSet = new List<Subregion>();
			openSet.Add(subregion);

			while (FloodFill(openSet, region))
			{ }

			return region;
		}


		private static bool FloodFill(List<Subregion> openSet, Region region)
		{
			if (openSet.Count == 0)
			{
				return false;
			}

			for (int i = openSet.Count - 1; i >= 0; i--)
			{
				if (openSet[i].Region != region && openSet[i].Region is null)
				{
					region.AddSubregion(openSet[i]);
				}
			}
				

			foreach (Subregion neighbour in openSet[0].NeighbouringSubregions)
			{
				if (neighbour.Region != region && !openSet.Contains(neighbour))
				{
					openSet.Add(neighbour);
				}
			}

			openSet.RemoveAt(0);
			return true;
		}
	}
}