using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugWindow : WindowComponent
{   
    private TextMeshProUGUI _tileCoordinatesTMP;

    private void Awake() {

        FindWindowObjects();
    }

    private void Update() {

        SetTileCoordinates();
    }

    private void FindWindowObjects() {

        _tileCoordinatesTMP = transform.Find("Tile Coordinates TMP").GetComponent<TextMeshProUGUI>();
    }

    private void SetTileCoordinates() {

        Vector2 currentCursorPosition = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridCoordinates = new Vector2Int( (int) (currentCursorPosition.x + 0.5f), (int) (currentCursorPosition.y + 0.5f) );

        _tileCoordinatesTMP.text = "Coordinates: (" + gridCoordinates.x + "; " + gridCoordinates.y + ")";
    }
}
