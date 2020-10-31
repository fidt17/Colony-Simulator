using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathfinderRenderer : Singleton<PathfinderRenderer> {
    
    public bool drawRegions = false;
    public bool drawSubregions = false;

    #region Region drawing
    
    private bool _isDrawingRegions = false;
    Region _selectedRegion = null;
    public IEnumerator DrawRegions(List<Region> regions) {
        if (_isDrawingRegions)
            yield break;
        _isDrawingRegions = true;

        PathNode node = Utils.NodeAt(Utils.CursorToCoordinates());
        Region region = node.Region;
        if (region == null) {
            yield break;
        }

        _selectedRegion = region;
        List<PathNode> list = region.GetNodes();
        List<GameObject> areas = MeshGenerator.GetInstance().GenerateOverlapAreaOverNodes(list, Utils.GetRandomColor(0.25f));

        while ((Utils.NodeAt(Utils.CursorToCoordinates()).Region == _selectedRegion || Utils.NodeAt(Utils.CursorToCoordinates()).Region == null) && drawRegions) {
            yield return null;
        }

        areas.ForEach(x => Destroy(x));
        _isDrawingRegions = false;
    }

    public void DrawSubregionsPath(List<Subregion> subregions) {
        StartCoroutine(DrawSubregions(subregions));
    }
    
    private bool _isDrawingSubregions = false;
    private IEnumerator DrawSubregions(List<Subregion> subregions) {
        if (_isDrawingSubregions) {
            yield break;
        }

        _isDrawingSubregions = true;

        List<PathNode> list = new List<PathNode>();
        foreach (Subregion s in subregions) {
            foreach (PathNode n in s.nodes) {
                list.Add(n);
            }
        }

        List<GameObject> areas = MeshGenerator.GetInstance()
                                              .GenerateOverlapAreaOverNodes(list, Utils.GetRandomColor(0.25f));

        yield return new WaitForSeconds(1f);
        areas.ForEach(x => Destroy(x));
        _isDrawingSubregions = false;
    }

    private void DrawSubregionUnderCursor() {
        PathNode node = Utils.NodeAt(Utils.CursorToCoordinates());
        List<PathNode> list = node.subregion.nodes;
        MeshGenerator.GetInstance().GenerateOverlapAreaOverNodes(list, Utils.GetRandomColor(0.1f)).ForEach(x => Destroy(x, 5));
    }

    #endregion

    private void Update() {
        if (drawRegions)
            StartCoroutine(DrawRegions(RegionSystem.regions));
    }

    public void DrawPath(List<PathNode> closedList) => MeshGenerator.GetInstance().GenerateOverlapAreaOverNodes(closedList, Utils.GetRandomColor(0.25f)).ForEach(x => Destroy(x, 2.5f));
}