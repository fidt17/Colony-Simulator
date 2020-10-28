using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {

    public static int MapSize => (int) GameManager.GetInstance().gameSettings.mapSize;

    public static Vector2 CursorToWorldPosition() => (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);

    public static Vector2Int WorldToGrid(Vector2 vec) => new Vector2Int( (int) (vec.x + 0.5f), (int) (vec.y + 0.5f));

    public static Vector2Int CursorToCoordinates() => WorldToGrid(CursorToWorldPosition());

    public static bool IsPositionViable(Vector2Int position) => !(position.x < 0 || position.x >= MapSize || position.y < 0 || position.y >= MapSize);
    public static bool IsPositionViable(int x, int y) => !(x < 0 || x >= MapSize || y < 0 || y >= MapSize);

    public static PathNode RandomNode() => NodeAt(new Vector2Int((int) Random.Range(0, MapSize), (int) Random.Range(0, MapSize)));
    public static PathNode NodeAt(Vector2Int position) => PathGrid.NodeAt(position);
    public static PathNode NodeAt(int x, int y) => PathGrid.NodeAt(x, y);

    public static Tile TileAt(Vector2Int position) => GameManager.GetInstance().world.GetTileAt(position);
    public static Tile TileAt(int x, int y) => GameManager.GetInstance().world.GetTileAt(x, y);
    public static Tile RandomTile() => TileAt(new Vector2Int((int) Random.Range(0, MapSize), (int) Random.Range(0, MapSize)));

    public static Vector3 ToVector3(Vector2Int vec) => new Vector3(vec.x, vec.y, 0);
    public static Vector2Int ToVector2Int(Vector2 vec) => new Vector2Int((int) vec.x, (int) vec.y);

    public static int SqrMaginute(int x1, int y1, int x2, int y2) => (x2 - x1)*(x2 - x1) + (y2 - y1)*(y2 - y1);

    public static Color GetRandomColor(float alpha) => new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), Mathf.Clamp(alpha, 0f, 1f));

    /* FUNCTION TIME TEST
        float startTime = Time.realtimeSinceStartup;
        //function
        Debug.Log("TIME TEST: " + (Time.realtimeSinceStartup - startTime) + " seconds.");
    */
}