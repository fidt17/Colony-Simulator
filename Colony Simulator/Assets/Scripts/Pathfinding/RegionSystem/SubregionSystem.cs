using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SubregionSystem {

    public static List<Subregion> subregions = new List<Subregion>();
    private const int _subregionSize = 10;

    public static void CreateSubregions() {
        for (int x = 0; x < Utils.WorldDimensions().x; x += 10) {
            for (int y = 0; y < Utils.WorldDimensions().y; y += 10) {
                CreateSubregionAt(x, y);
            }
        }
        foreach(Subregion subregion in subregions) {
            FindNeighboursFor(subregion);
        }
    }

    public static void UpdateSubregionAt(int X, int Y) {
        Utils.NodeAt(X, Y).subregion.Reset();
        foreach (Subregion newSubregion in CreateSubregionAt(X, Y)) {
            FindNeighboursFor(newSubregion);
        }
    }

    private static void FindNeighboursFor(Subregion subregion) {
        foreach (PathNode checkNode in subregion.nodes) {
            foreach (PathNode neighbour in checkNode.GetNeighbours()) {
                if (neighbour.subregion != subregion && neighbour.subregion != null) {
                    subregion.AddNeighbour(neighbour.subregion);
                    
                }
            }
        }
    }

    private static List<Subregion> CreateSubregionAt(int X, int Y) {
        int subregionStartX = ((int) (X / _subregionSize)) * _subregionSize;
        int subregionStartY = ((int) (Y / _subregionSize)) * _subregionSize;

        List<Subregion> createdSubregions = new List<Subregion>();
        for (int x = subregionStartX; x < subregionStartX + _subregionSize; x++) {
            for (int y = subregionStartY; y < subregionStartY + _subregionSize; y++) {
                Subregion newSubregion = FillSubregionFrom(Utils.NodeAt(x, y));
                if (newSubregion != null) {
                    createdSubregions.Add(newSubregion);
                }
            }
        }

        return createdSubregions;
    }

    private static Subregion FillSubregionFrom(PathNode node) {
        if (node.subregion != null || node.isTraversable == false) {
            return null;
        }
        
        Subregion subregion = new Subregion();
        List<PathNode> openNodes = new List<PathNode>();
        openNodes.Add(node);
        do {
        } while(NextWaveIteration(ref openNodes, ref subregion) != 0);
        subregions.Add(subregion);
        return subregion;
    }

    private static int NextWaveIteration(ref List<PathNode> openSet, ref Subregion subregion) {
        for (int i = openSet.Count - 1; i >= 0; i--) {
            if (openSet[i].subregion != null) {
                openSet.RemoveAt(i);
                if (openSet.Count == 0) {
                    return 0;
                }
            }
        }

        PathNode node = openSet[0];
        subregion.AddNode(node);
        
        //Adding surrounding cells to available list
        foreach (PathNode neighbour in node.GetNeighbours()) {
            if (neighbour.isTraversable == false || neighbour.subregion != null
                || IsInsideArea(neighbour.x, neighbour.y, ((int) (node.x / _subregionSize)) * _subregionSize, ((int) (node.y / _subregionSize)) * _subregionSize) == false) {
                continue;
            }
            openSet.Insert(0, neighbour);
        }
        //////

        return 1;
	}

    private static bool IsInsideArea(int x, int y, int subX, int subY) => (x >= subX && y >= subY && x < subX + _subregionSize && y < subY + _subregionSize);
}