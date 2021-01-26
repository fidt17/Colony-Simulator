using System.Collections.Generic;

namespace Pathfinding
{
	public class Region
	{
		public List<Subregion> Subregions { get; protected set; } = new List<Subregion>();

		public void AddSubregion(Subregion subregion)
		{
			Subregions.Add(subregion);
			subregion.SetRegion(this);
		}

		public void RemoveSubregion(Subregion subregion)
		{
			Subregions.Remove(subregion);
			subregion.SetRegion(null);
		}

		public void Reset()
		{
			for (int i = Subregions.Count - 1; i >= 0; i--)
			{
				RemoveSubregion(Subregions[i]);
			}
			RegionSystem.Regions.Remove(this);
		}

		public List<Node> GetNodes()
		{
			List<Node> nodes = new List<Node>();
			foreach (Subregion subregion in Subregions)
			{
				foreach (Node node in subregion.Nodes)
				{
					nodes.Add(node);
				}
			}
			
			return nodes;
		}
	}
}