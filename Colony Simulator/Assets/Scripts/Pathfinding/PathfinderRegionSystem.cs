using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderRegionSystem {

    public List<Region> regions { get; private set; }

    private Vector2Int _dimensions;

    public PathfinderRegionSystem(Vector2Int dimensions) {
        _dimensions = dimensions;
        CreateRegions();
    }

    public void UpdateSystem() {
        ResetRegions();
        CreateRegions();
    }

    private void ResetRegions() {
        foreach (Region r in regions) {
            r.ResetRegion();
        }
    }

    private void CreateRegions() {
        regions = new List<Region>();
        for (int x = 0; x < _dimensions.x; x++) {
            for (int y = 0; y < _dimensions.y; y++) {
                PathNode node = GameManager.Instance.pathfinder.grid.GetNodeAt(new Vector2Int(x, y));
                if (node.region == null && node.isTraversable) {
                    Region newRegion = CreateRegionAt(node);
                    regions.Add(newRegion);
                }
            }
        }
    }

    #region Wave generation of regions

    public Region CreateRegionAt(PathNode startNode) {
        Region region = new Region();
        List<PathNode> openNodes = new List<PathNode>();
        openNodes.Add(startNode);
        int result = 0;
        do {
            result = NextWaveIteration(ref openNodes, ref region, startNode.position);
        } while(result != 0);

        return region;
	}

    private int NextWaveIteration(ref List<PathNode> openSet, ref Region region, Vector2Int startCell) {
        if(openSet.Count == 0) {
            return 0;
        }
        
        //Choosing initial cell
        PathNode initialCell = null;

        float minDistance = 100000;
        int indexToClosest = -1;

        for (int i = openSet.Count - 1; i >= 0; i--) {
            PathNode n = openSet[i];
            Vector2Int dist = n.position - startCell;
            float d = dist.magnitude;

            if(d < minDistance && n.region == null) {
                minDistance = d;
                indexToClosest = i;
			}
		}

        if (indexToClosest == -1)
            return 0;

        initialCell = openSet[indexToClosest];

        for (int i = openSet.Count - 1; i >= 0; i--) {
            PathNode n = openSet[i];
            if(n.region != null || n == initialCell) {
                openSet.RemoveAt(i);
                continue;
			}
		}
        //////

        //Adding surrounding cells to available list
        for (int x = initialCell.position.x - 1; x <= initialCell.position.x + 1; x++) {
            for(int y = initialCell.position.y - 1; y <= initialCell.position.y + 1; y++) {
                Vector2Int checkPosition = new Vector2Int(x, y);
                PathNode n = GameManager.Instance.pathfinder.grid.GetNodeAt(checkPosition);
                if (n == null || !n.isTraversable || n.region != null || openSet.Contains(n)) {
                    continue;
                }
                openSet.Insert(0, n);
			}
		}
        //////

        initialCell.region = region;
        region.nodes.Add(initialCell);

        return 1;
	}
    #endregion
}

public class Region {

    public List<PathNode> nodes = new List<PathNode>();

    public void ResetRegion() {
        foreach (PathNode n in nodes) {
            n.region = null;
        }
    }
}