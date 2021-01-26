using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	public static class RPathfinding
	{
		public delegate void OnAddToClosedSet(Node node);

		public delegate void OnAddToPath(Node node);

		public static event OnAddToPath      HandleAddToPath;
		public static event OnAddToClosedSet HandleAddToClosedSet;

		public static List<Node> GetPath(Node startNode, Node targetNode)
		{
			if (startNode == targetNode)
			{
				return new List<Node>();
			}

			if (startNode.Region != targetNode.Region)
			{
				return null;
			}
			
			//Swap source with target for optimization of path retracing.
			Node tmp = startNode;
			startNode = targetNode;
			targetNode = tmp;
			
			List<Subregion> subregionPath = ASubregionSearch.GetPath(startNode.Subregion, targetNode.Subregion);
			Stack<Subregion> corridor = new Stack<Subregion>(subregionPath);

			MinHeap<Node> openSet = new MinHeap<Node>(Pathfinder.MapWidth * Pathfinder.MapHeight);
			HashSet<Node> closedSet = new HashSet<Node>();
			openSet.Add(startNode);

			while (openSet.Count > 0)
			{
				Node currentNode;
				do
				{
					currentNode = openSet.RemoveFirst();
					if (corridor.Count == 0 || currentNode.Subregion == corridor.Peek()) break;
				} while (true);

				closedSet.Add(currentNode);
				HandleAddToClosedSet?.Invoke(currentNode);//visualization

				if (currentNode == targetNode)
				{
					List<Node> path = RetracePath(startNode, targetNode);

					foreach (Subregion subregion in subregionPath) subregion.Child = null;

					return path;
				}

				foreach (Node neighbour in currentNode.GetAllNeighbours())
				{
					if (!neighbour.IsTraversable || closedSet.Contains(neighbour))
					{
						continue;
					}

					if (corridor.Count != 0 && corridor.Peek().Child == neighbour.Subregion)
					{
						corridor.Pop();
					}

					if (corridor.Count != 0 && !corridor.Peek().Nodes.Contains(neighbour))
					{
						continue;
					}

					int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
					bool isInOpenSet = openSet.Contains(neighbour);
					if (newCostToNeighbour < neighbour.gCost || !isInOpenSet)
					{
						neighbour.gCost = newCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						
						if (neighbour.Subregion.Child != null)
						{
							neighbour.rCost = GetDistance(neighbour,
							                              PathGrid.NodeAt(neighbour.Subregion.Child.AvergX,
							                                              neighbour.Subregion.Child.AvergY));
						}
						else
						{
							neighbour.rCost = 0;
						}

						neighbour.Parent = currentNode;

						if (!isInOpenSet)
						{
							openSet.Add(neighbour);
						}
					}
				}
			}

			foreach (Subregion subregion in subregionPath)
			{
				subregion.Child = null;
			}

			return null;
		}

		public static int GetDistance(Node A, Node B)
		{
			int dx = Mathf.Abs(A.X - B.X);
			int dy = Mathf.Abs(A.Y - B.Y);
			return 10 * (dx + dy) + (14 - 2 * 10) * Mathf.Min(dx, dy);
		}

		private static List<Node> RetracePath(Node startNode, Node currentNode)
		{
			List<Node> path = new List<Node>();
			do
			{
				path.Add(currentNode);
				currentNode = currentNode.Parent;
				HandleAddToPath?.Invoke(currentNode);//visualization
			} while (currentNode != startNode);

			path.Add(startNode);
			return path;
		}
	}
}