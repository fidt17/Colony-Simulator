using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subregion {

    public List<PathNode> nodes = new List<PathNode>();
    public List<Subregion> neighbouringSubregions = new List<Subregion>();

    public Region region { get; protected set; }

    public void SetRegion(Region region) => this.region = region;

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
        for (int i = nodes.Count - 1; i >= 0; i--) {
            RemoveNode(nodes[i]);
        }

        for (int i = neighbouringSubregions.Count - 1; i >= 0; i--) {
            RemoveNeighbour(neighbouringSubregions[i]);
        }

        region?.RemoveSubregion(this);

        SubregionSystem.subregions.Remove(this);
    }
}