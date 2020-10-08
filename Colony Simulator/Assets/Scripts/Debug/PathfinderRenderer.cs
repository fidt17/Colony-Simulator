﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderRenderer : MonoBehaviour {
    
    public bool drawRegions = false;

    private void Update() {
        if (drawRegions)
            StartCoroutine(DrawRegions(Pathfinder.regionSystem.regions));
    }

    public void DrawPath(List<PathNode> closedList) {
        foreach (PathNode node in closedList) {
            Color pathColor = new Color(230f/255f, 100f/255f, 240f/255f, 1f);
            Tile t = Utils.TileAt(node.position);
            StartCoroutine(ChangeTileColor(t, pathColor, 2f));
        }
    }

    private bool _isDrawingRegions = false;
    public IEnumerator DrawRegions(List<Region> regions) {
        if (_isDrawingRegions)
            yield break;

        _isDrawingRegions = true;
        foreach (Region region in regions) {
            Color regionColor = new Color(Random.Range(0, 255)/255f, Random.Range(0, 255)/255f, Random.Range(0, 255)/255f, 1f);
            foreach (PathNode node in region.nodes) {
                Tile t = Utils.TileAt(node.position);
                StartCoroutine(ChangeTileColor(t, regionColor, 2f));
            }
        }

        yield return new WaitForSeconds(2f);
        _isDrawingRegions = false;
    }

    private IEnumerator ChangeTileColor(Tile tile, Color color, float time) {
        tile.SetColor(color);
        yield return new WaitForSeconds(time);
        tile.ResetColor();
    }
}