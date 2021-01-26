using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Pathfinding
{
	public class Node : IHeapItem<Node>
	{
		public Vector2Int Position => new Vector2Int(X, Y);
		public readonly int        X, Y;
		public Region     Region => Subregion?.Region;
		public bool       IsTraversable;

		public int  gCost, hCost, rCost;
		public int  fCost => gCost + hCost + rCost;
		public Node Parent;
		
		public Subregion Subregion;
		
		public int HeapIndex { get; set; }

		public Node(int x, int y, ITraversable tile)
		{
			X = x;
			Y = y;
			IsTraversable = tile.IsTraversable;
			tile.OnTraversabilityChange += HandleTraversabilityChange;
		}

		public int CompareTo(Node nodeToCompare)
		{
			int compare = fCost.CompareTo(nodeToCompare.fCost);
			if (compare == 0)
			{
				compare = hCost.CompareTo(nodeToCompare.hCost);
			}
			return -compare;
		}

		public void HandleTraversabilityChange(object source, EventArgs e)
		{
			if (e is TraversabilityArgs args)
			{
				IsTraversable = args.IsTraversable;
				RegionSystem.UpdateSystemAt(X, Y);
			}
		}
		
		///<summary>Returns neighbours to the right/left/up/down of the node.</summary>
		public List<Node> GetNeighbours()
		{
			List<Node> neighbours = new List<Node>
			{
				PathGrid.NodeAt(X, Y + 1),
				PathGrid.NodeAt(X    + 1, Y),
				PathGrid.NodeAt(X, Y - 1),
				PathGrid.NodeAt(X    - 1, Y)
			};

			for (int i = neighbours.Count - 1; i >= 0; i--)
			{
				if (neighbours[i] is null)
				{
					neighbours.RemoveAt(i);
				}
			}

			return neighbours;
		}
		
		public List<Node> GetAllNeighbours()
		{
			List<Node> neighbours = new List<Node>();
			for (int x = -1; x <= 1; x++)
			for (int y = -1; y <= 1; y++)
			{
				if (x == 0 && y == 0)
				{
					continue;
				}

				Node n = PathGrid.NodeAt(this.X + x, this.Y + y);
				
				if (n != null)
				{
					neighbours.Add(n);
				}
			}

			return neighbours;
		}
	}
}