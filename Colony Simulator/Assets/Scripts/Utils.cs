using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {

    public static Vector2Int WorldDimensions() => new Vector2Int(GameManager.GetInstance().gameSettings.worldWidth, GameManager.GetInstance().gameSettings.worldHeight);

    public static Vector2 CursorToWorldPosition() => (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);

    public static Vector2Int CursorToCoordinates() {
        Vector2 currMousePosition = CursorToWorldPosition();
        return new Vector2Int( (int) (currMousePosition.x + 0.5f), (int) (currMousePosition.y + 0.5f) );
    }

    public static bool IsPositionViable(Vector2Int position) => !(position.x < 0 || position.x >= WorldDimensions().x || position.y < 0 || position.y >= WorldDimensions().y);
    public static bool IsPositionViable(int x, int y) => !(x < 0 || x >= WorldDimensions().x || y < 0 || y >= WorldDimensions().y);

    public static PathNode RandomNode() => NodeAt(new Vector2Int((int) Random.Range(0, WorldDimensions().x), (int) Random.Range(0, WorldDimensions().y)));
    public static PathNode NodeAt(Vector2Int position) => PathGrid.NodeAt(position);
    public static PathNode NodeAt(int x, int y) => PathGrid.NodeAt(x, y);

    public static Tile TileAt(Vector2Int position) => GameManager.GetInstance().world.GetTileAt(position);
    public static Tile TileAt(int x, int y) => GameManager.GetInstance().world.GetTileAt(x, y);
    public static Tile RandomTile() => TileAt(new Vector2Int((int) Random.Range(0, WorldDimensions().x), (int) Random.Range(0, WorldDimensions().y)));

    public static Vector3 ToVector3(Vector2Int vec) => new Vector3(vec.x, vec.y, 0);

    public static int SqrMaginute(int x1, int y1, int x2, int y2) => (x2 - x1)*(x2 - x1) + (y2 - y1)*(y2 - y1);

    /* FUNCTION TIME TEST
        float startTime = Time.realtimeSinceStartup;
        //function
        Debug.Log("TIME TEST: " + (Time.realtimeSinceStartup - startTime) + " seconds.");
    */
}