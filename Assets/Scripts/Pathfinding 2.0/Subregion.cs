using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	public class Subregion : IHeapItem<Subregion>
	{
		public  int AvergX, AvergY;

		public int       gCost, hCost;
		public int       fCost  => gCost + hCost;
		public Region    Region { get; protected set; }
		public Subregion ParentSubregion;
		public Subregion Child;	
		public int HeapIndex { get; set; }

		public HashSet<Node>   Nodes                  { get; private set; }   = new HashSet<Node>();
		public List<Subregion> NeighbouringSubregions { get; private set; }   = new List<Subregion>();
		public RegionContent   Content                { get; protected set; } = new RegionContent();
		
		public void SetRegion(Region region)
		{
			this.Region = region;
		}

		public void CalculateAverageCoordinates()
		{
			//Finding node with minimum distance to all other nodes
			int minSqrDistance = Int32.MaxValue;
			foreach (Node node in Nodes)
			{
				int currSqrDistance = 0;
				foreach (Node border in Nodes)
				{
					int dX = border.X >= node.X ? border.X - node.X : node.X - border.X;
					int dY = border.Y >= node.Y ? border.Y - node.Y : node.Y - border.Y;
					currSqrDistance += (int) (Mathf.Pow(dX, 2) + Mathf.Pow(dY, 2));
				}

				if (currSqrDistance < minSqrDistance)
				{
					minSqrDistance = currSqrDistance;
					AvergX = node.X;
					AvergY = node.Y;
				}
			}
		}

		public void AddNode(Node node)
		{
			Nodes.Add(node);
			node.Subregion = this;
		}

		public void AddNeighbour(Subregion neighbour)
		{
			if (NeighbouringSubregions.Contains(neighbour)) return;
			NeighbouringSubregions.Add(neighbour);
			neighbour.AddNeighbour(this);
		}

		public void RemoveNeighbour(Subregion neighbour)
		{
			NeighbouringSubregions.Remove(neighbour);
			neighbour.NeighbouringSubregions.Remove(this);
		}

		public void Reset()
		{
			Content.Clear();
			foreach (Node node in Nodes) node.Subregion = null;
			Nodes.Clear();

			for (int i = NeighbouringSubregions.Count - 1; i >= 0; i--)
			{
				RemoveNeighbour(NeighbouringSubregions[i]);
			}

			Region?.RemoveSubregion(this);

			SubregionSystem.RemoveSubregion(this);
		}

		public void FindNeighbours()
		{
			foreach (Node node in Nodes)
			{
				foreach (Node neighbour in node.GetNeighbours())
				{
					if (neighbour.Subregion != this && neighbour.Subregion != null)
					{
						AddNeighbour(neighbour.Subregion);
					}
				}
			}
		}

		public int CompareTo(Subregion other)
		{
			int compare = fCost.CompareTo(other.fCost);
			if (compare == 0)
			{
				compare = hCost.CompareTo(other.hCost);
			}
			return -compare;
		}
		
		public void ScanForContent()
		{
			Content.Clear();
			foreach (Node n in Nodes)
			{
				Tile t = Utils.TileAt(n.X, n.Y);
				t.Contents?.StaticObject?.AddToRegionContent();
				t.Contents?.Item?.AddToRegionContent();
			}
		}
	}
}