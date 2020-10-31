using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subregion {

    public List<PathNode> nodes { get; protected set; } = new List<PathNode>();
    public List<Subregion> neighbouringSubregions { get; protected set; } = new List<Subregion>();
    public RegionContent content { get; protected set; } = new RegionContent();

    public Region region { get; protected set; }

    public void SetRegion(Region region) => this.region = region;

    //Pathfinding
    public Subregion parent;
    public int       gCost, hCost;
    public int       fCost => gCost + hCost;
    //
    
    public void AddNode(PathNode node) {
        nodes.Add(node);
        node.subregion = this;
    }

    public void RemoveNode(PathNode node) {
        nodes.Remove(node);
        node.subregion = null;
    }

    public void AddNeighbour(Subregion neighbour) {
        if (neighbouringSubregions.Contains(neighbour)) {
            return;
        }
        neighbouringSubregions.Add(neighbour);
        neighbour.AddNeighbour(this);
    }

    public void RemoveNeighbour(Subregion neighbour) {
        neighbouringSubregions.Remove(neighbour);
        neighbour.neighbouringSubregions.Remove(this);
    }

    public void Reset() {
        content.Clear();

        for (int i = nodes.Count - 1; i >= 0; i--) {
            RemoveNode(nodes[i]);
        }

        for (int i = neighbouringSubregions.Count - 1; i >= 0; i--) {
            RemoveNeighbour(neighbouringSubregions[i]);
        }

        region?.RemoveSubregion(this);

        SubregionSystem.RemoveSubregion(this);
    }

    public void FindNeighbours() {
        foreach (PathNode node in nodes) {
            foreach (PathNode neighbour in node.GetNeighbours()) {
                if (neighbour.subregion != this && neighbour.subregion != null) {
                    AddNeighbour(neighbour.subregion);
                }
            }
        }
    }

    public void ScanForContent() {
        content.Clear();
        foreach (PathNode n in nodes) {
            Tile t = Utils.TileAt(n.x, n.y);
            t.content?.StaticObject?.AddToRegionContent();
            t.content?.Item?.AddToRegionContent();
        }
    }
}