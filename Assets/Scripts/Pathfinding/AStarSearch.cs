using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStarSearch {

    public delegate void OnAddToOpenSet(PathNode node);
    public static event OnAddToOpenSet HandleAddToOpenSet;
    
    public delegate void OnAddToClosedSet(PathNode node);
    public static event OnAddToClosedSet HandleAddToClosedSet;
    
    public static List<PathNode> GetPath2(PathNode startNode, PathNode targetNode, ref List<PathNode> closedSet) {

        if (startNode.Region != targetNode.Region) {
            return null;
        }

        if (startNode == targetNode) {
            return new List<PathNode>();
        }

        List<Subregion> subregions = AStarSubregionSearch.GetPath(startNode.subregion, targetNode.subregion);
        PathNode[] possibleNodes = new PathNode[Utils.MapSize  * Utils.MapSize];
        foreach (Subregion s in subregions) {
            foreach (PathNode n in s.nodes) {
                possibleNodes[n.x + n.y * Utils.MapSize] = n;
            }
        }
        
        List<PathNode> openSet = new List<PathNode>();
        openSet.Add(startNode);
        
        //FOR COLORING
        HandleAddToOpenSet?.Invoke(startNode);
        
        while (openSet.Count > 0) {
            PathNode currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].fCost <= currentNode.fCost) {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            
            //FOR COLORING
            HandleAddToClosedSet?.Invoke(currentNode);
            
            if (currentNode == targetNode) {
                return RetracePath(startNode, targetNode);
            }

            foreach(PathNode neighbour in GetNeighbours(currentNode)) {
                if(closedSet.Contains(neighbour) || possibleNodes[neighbour.x + neighbour.y * Utils.MapSize] == null) {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + Heuristic(currentNode, neighbour);
                if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = Heuristic(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour)) {
                        openSet.Add(neighbour);
                        
                        //FOR COLORING
                        HandleAddToOpenSet?.Invoke(neighbour);
                    }
                }
            }
        }

        return null;
    }
    
    public static List<PathNode> GetPath(PathNode startNode, PathNode targetNode) {
        if (startNode == targetNode) {
            return new List<PathNode>();
        }
        
        if (startNode.Region != targetNode.Region) {
            return null;
        }

        Heap<PathNode> openSet = new Heap<PathNode>(Utils.MapSize * Utils.MapSize);
        openSet.Add(startNode);
        HandleAddToOpenSet?.Invoke(startNode);
        HashSet<PathNode> closedSet = new HashSet<PathNode>();

        while (openSet.Count > 0) {
            PathNode currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);
            HandleAddToClosedSet?.Invoke(currentNode);
            
            if (currentNode == targetNode) {
                var path = RetracePath(startNode, targetNode);
                return path;
            }

            foreach(var neighbour in GetNeighbours(currentNode)) {
                if (closedSet.Contains(neighbour)) {
                    continue;
                }
                
                int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (openSet.Contains(neighbour)) {
                    if (newCostToNeighbour < neighbour.gCost) {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = Heuristic(neighbour, targetNode);
                        neighbour.parent = currentNode;
                        openSet.UpdateItem(neighbour);
                    }
                } else {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = Heuristic(neighbour, targetNode);
                    neighbour.parent = currentNode;
                    
                    openSet.Add(neighbour);
                    HandleAddToOpenSet?.Invoke(neighbour);
                }
            }
        }

        return null;
    }

    private static List<PathNode> GetNeighbours(PathNode node) {
		List<PathNode> neighbours = new List<PathNode>();
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {

				if( x == 0 && y == 0 ) {
					continue;
                }

				int checkX = node.position.x + x;
				int checkY = node.position.y + y;
                PathNode n = Utils.NodeAt(new Vector2Int(checkX, checkY));
                if (n != null && n.isTraversable) {
					neighbours.Add(n);
                }
			}
		}
		return neighbours;
	}

    private static int GetDistance(PathNode A, PathNode B) {
        int distX = Mathf.Abs(A.x - B.x);
        int distY = Mathf.Abs(A.y - B.y);
        if(distX > distY) {
            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distX + 10 * (distY - distX);
    }

    private static int Heuristic(PathNode A, PathNode B) {
        
        
        // Octile
        int dx = Mathf.Abs(A.x - B.x);
        int dy = Mathf.Abs(A.y - B.y);
        return 10 * (dx + dy) + (14 - 2 * 10) * Mathf.Min(dx, dy);
        
        //Manhattan
        return Mathf.Abs(A.x - B.x) + Mathf.Abs(A.y - B.y);
        
        int D = 1;
        int D2 = 1;
        
        if (dx > dy) {
            return (D * (dx - dy) + D2 * dy);
        } else {
            return (D * (dy - dx) + D2 * dx);
        }
    }

    private static List<PathNode> RetracePath(PathNode startNode, PathNode currentNode) {
        List<PathNode> path = new List<PathNode>();
        path.Add(currentNode);
        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }
}

public class Heap<T> where T : IHeapItem<T> {
	
    T[] items;
    int currentItemCount;
	
    public Heap(int maxHeapSize) {
        items = new T[maxHeapSize];
    }
	
    public void Add(T item) {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst() {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item) {
        SortUp(item);
    }

    public int Count {
        get {
            return currentItemCount;
        }
    }

    public bool Contains(T item) {
        return Equals(items[item.HeapIndex], item);
    }

    void SortDown(T item) {
        while (true) {
            int childIndexLeft = item.HeapIndex  * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentItemCount) {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount) {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) {
                        swapIndex = childIndexRight;
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0) {
                    Swap (item,items[swapIndex]);
                }
                else {
                    return;
                }

            }
            else {
                return;
            }

        }
    }
	
    void SortUp(T item) {
        int parentIndex = (item.HeapIndex -1) /2;
		
        while (true) {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0) {
                Swap (item,parentItem);
            }
            else {
                break;
            }

            parentIndex = (item.HeapIndex -1) /2;
        }
    }
	
    void Swap(T itemA, T itemB) {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T> {
    int HeapIndex {
        get;
        set;
    }
}
