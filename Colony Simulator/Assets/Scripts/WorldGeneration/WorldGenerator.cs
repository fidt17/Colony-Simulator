using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator
{
    public void GenerateEmptyWorld(Vector2Int dimensions, ref Tile[,] grid) {

        grid = new Tile[dimensions.x, dimensions.y];

        GameObject tileParent = GameObject.Find("World_Tiles");

        if (tileParent == null) {
            Debug.LogError("World_Tiles gameobject is missing!");
            return;
        }

        for (int x = 0; x < dimensions.x; x++) {
            for (int y = 0; y < dimensions.y; y++) {

                GameObject newTile = GameObject.Instantiate(PrefabStorage.Instance.protoTile);
                newTile.transform.position = new Vector3(x, y, 0);
                newTile.transform.parent = tileParent.transform;
                newTile.name = "Tile (" + x + ";" + y + ")";

                newTile.GetComponent<SpriteRenderer>().color = ((x + y) % 2 == 0) ? Color.white : Color.black;

                Tile t = new Tile(new Vector2Int(x, y), newTile);

                grid[x, y] = t;
            }
        }
    }
}
