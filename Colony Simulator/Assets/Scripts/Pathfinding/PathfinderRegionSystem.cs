using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderRegionSystem
{   
    private List<Region> _regions;
    private List<Subregion> _subregions = new List<Subregion>();

    private const int subregionSize = 10;

    private Vector2Int dimensions;
    public PathfinderRegionSystem(Vector2Int dimensions) {

        this.dimensions = dimensions;
        CreateRegions();
    }

    public void UpdateSystem() {

        ResetRegions();
        CreateRegions();
    }

    private void ResetRegions() {

        foreach (Region r in _regions)
            r.ResetRegion();
    }

    private void CreateRegions() {

        _regions = new List<Region>();

        for (int x = 0; x < dimensions.x; x++) {
            for (int y = 0; y < dimensions.y; y++) {

                PathNode node = GameManager.Instance.pathfinder.grid.GetNodeAt(new Vector2Int(x, y));

                if (node.region == null && node.isTraversable) {

                    Region newRegion = CreateRegionAt(node);
                    _regions.Add(newRegion);
                }
            }
        }
    }

    #region Debug Functions

    private bool drawingRegions = false;
    public IEnumerator DrawRegions() {

        if (drawingRegions)
            yield break;

        drawingRegions = true;

        foreach (Region r in _regions) {
            
            Color regionColor = new Color(Random.Range(0, 255)/255f, Random.Range(0, 255)/255f, Random.Range(0, 255)/255f, 1f);

            foreach (PathNode node in r.nodes) {

                Tile t = GameManager.Instance.world.GetTileAt(node.position);
                t.gameObject.GetComponent<SpriteRenderer>().color = regionColor;
            }
        }

        yield return new WaitForSeconds(2f);

        foreach (Region r in _regions) {
            
            Color regionColor = Color.white;

            foreach (PathNode node in r.nodes) {

                Tile t = GameManager.Instance.world.GetTileAt(node.position);
                t.gameObject.GetComponent<SpriteRenderer>().color = regionColor;
            }
        }

        drawingRegions = false;
    }

    #endregion

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

    private int NextWaveIteration(ref List<PathNode> openSet, ref Region region, Vector2Int startCell)
    {
        if(openSet.Count == 0)
            return 0;
        
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

                if (n == null)
                    continue;

                if (!n.isTraversable)
                    continue;

                if (n.region != null)
                    continue;
                
                if (openSet.Contains(n))
                    continue;

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

    private List<Subregion> _subregions = new List<Subregion>();
    public List<PathNode> nodes = new List<PathNode>();

    public List<Subregion> GetSubregions() {

        return _subregions;
    }

    public void ResetRegion() {

        foreach (PathNode n in nodes)
            n.region = null;
    }
}

public class Subregion {

    public Region region;

    public List<PathNode> nodes = new List<PathNode>();
}
