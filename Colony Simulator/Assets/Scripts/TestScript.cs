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
            Test2();
    }

    private IEnumerator spawnGrass() {

        while(true) {
            
            Tile t = null;

            while (t == null || t.type != TileType.grass)
             t = GameManager.Instance.world.GetTileAt(new Vector2Int((int) Random.Range(0, 50), (int) Random.Range(0, 50)));
            
            StaticObjectSpawnFactory.GetNewStaticObject("grass", "grass", t.position);

            yield return new WaitForSeconds(1f);
        }
    }

    private void Test2() {

        for(int x = 0; x < 50; x++) {
            for(int y = 0; y < 50; y++) {
                
                Tile tile = GameManager.Instance.world.GetTileAt(new Vector2Int(x, y));

                if (tile.type != TileType.grass)
                    continue;

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
