using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void Start() {

    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.P))
            BuildWall();

        if (Input.GetKeyDown(KeyCode.G))
            SpawnVegetation();
    }

    private void SpawnVegetation() {

        for(int x = 0; x < GameManager.Instance.gameSettings.worldWidth; x++) {
            for(int y = 0; y < GameManager.Instance.gameSettings.worldHeight; y++) {
                
                Tile tile = GameManager.Instance.world.GetTileAt(new Vector2Int(x, y));

                if (tile.type != TileType.grass)
                    continue;

                float r = Random.Range(0f, 1f);

                if (r > 0.95f)
                    StaticObjectSpawnFactory.GetNewStaticObject("tree", "tall tree", new Vector2Int(x, y));
                else if(r > 0.6f)
                    StaticObjectSpawnFactory.GetNewStaticObject("grass", "grass", new Vector2Int(x, y));
			}
		}
    }

    private void BuildWall() {

        Vector3 currMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int mousePos2D = new Vector2Int( (int) (currMousePosition.x + 0.5f), (int) (currMousePosition.y + 0.5f) );

        Tile t = GameManager.Instance.world.GetTileAt(mousePos2D);
        t.TestFunc(!t.isTraversable);

        if (t.isTraversable)
            t.GameObject.GetComponent<SpriteRenderer>().color = Color.white;
        else
            t.GameObject.GetComponent<SpriteRenderer>().color = Color.red;

        GameManager.Instance.pathfinder.UpdateSystem();
    }
}
