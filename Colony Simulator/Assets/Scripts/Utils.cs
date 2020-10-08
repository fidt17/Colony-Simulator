using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {

    public static Vector2 CursorToWorldPosition() => (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);

    public static Vector2Int CursorToCoordinates() {
        Vector2 currMousePosition = CursorToWorldPosition();
        return new Vector2Int( (int) (currMousePosition.x + 0.5f), (int) (currMousePosition.y + 0.5f) );
    }

    public static PathNode RandomNode() => NodeAt(new Vector2Int((int) Random.Range(0, GameManager.GetInstance().world.dimensions.x), (int) Random.Range(0, GameManager.GetInstance().world.dimensions.y)));
    public static Tile RandomTile() => TileAt(new Vector2Int((int) Random.Range(0, GameManager.GetInstance().world.dimensions.x), (int) Random.Range(0, GameManager.GetInstance().world.dimensions.y)));

    public static PathNode NodeAt(Vector2Int position) => Pathfinder.NodeAt(position);
    public static Tile TileAt(Vector2Int position) => GameManager.GetInstance().world.GetTileAt(position);

    public static bool IsPositionViable(Vector2Int position) {
        Vector2Int dimensions = GameManager.GetInstance().world.dimensions;
        return !(position.x < 0 || position.x >= dimensions.x || position.y < 0 || position.y >= dimensions.y);
    }

    public static Vector3 ToVector3(Vector2Int vec) => new Vector3(vec.x, vec.y, 0);
}