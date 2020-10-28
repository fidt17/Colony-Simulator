using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugWindow : WindowComponent {

    [SerializeField] private TextMeshProUGUI _tileCoordinatesTMP = null;

    private void Update() => SetTileCoordinates();

    private void SetTileCoordinates() {
        Vector2 currentCursorPosition = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridCoordinates = new Vector2Int( (int) (currentCursorPosition.x + 0.5f), (int) (currentCursorPosition.y + 0.5f) );

        Tile t = Utils.TileAt(new Vector2Int(gridCoordinates.x, gridCoordinates.y));
        if (t != null) {
            //_tileCoordinatesTMP.text = "Coordinates: (" + t.position.x + "; " + t.position.y + "), Object: " + t.content.staticObject + ", " + t.content.item;
            _tileCoordinatesTMP.text = "Coordinates: (" + t.position.x + "; " + t.position.y + "), Character: " + t.content.characters.Count;
        } else {
            _tileCoordinatesTMP.text = "Coordinates: (" + gridCoordinates.x + "; " + gridCoordinates.y + ")";
        }
    }
}