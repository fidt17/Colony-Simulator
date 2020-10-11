using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderRenderer : Singleton<PathfinderRenderer> {
    
    public bool drawRegions = false;
    public bool drawSubregions = false;

    private void Update() {
        if (drawRegions)
            StartCoroutine(DrawRegions(RegionSystem.regions));

        if (drawSubregions)
            StartCoroutine(DrawSubregions());
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
            foreach (Subregion subregion in region.subregions) {
                foreach (PathNode node in subregion.nodes) {
                    Tile t = Utils.TileAt(node.position);
                    StartCoroutine(ChangeTileColor(t, regionColor, 0.5f));
                }
            }
        }

        yield return new WaitForSeconds(0.5f);
        _isDrawingRegions = false;
    }

    private bool _isDrawingSubregions = false;
    public IEnumerator DrawSubregions() {
        if (_isDrawingSubregions)
            yield break;

        _isDrawingSubregions = true;
        Subregion subregion = Utils.NodeAt(Utils.CursorToCoordinates())?.subregion;
        if (subregion != null) {
            Color subregionColor = Color.blue;
            Color neighbourColor = Color.cyan;
            foreach (PathNode node in subregion.nodes) {
                StartCoroutine(ChangeTileColor(Utils.TileAt(node.position), subregionColor, 0.5f));
            }

            foreach (Subregion neighbouringSubregion in subregion.neighbouringSubregions) {
                foreach (PathNode node in neighbouringSubregion.nodes) {
                StartCoroutine(ChangeTileColor(Utils.TileAt(node.position), neighbourColor, 0.5f));
                }
            }
        }

        yield return new WaitForSeconds(0.5f);
        _isDrawingSubregions = false;
    }

    private IEnumerator ChangeTileColor(Tile tile, Color color, float time) {
        tile.SetColor(color);
        yield return new WaitForSeconds(time);
        tile.ResetColor();
    }
}