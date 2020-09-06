using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void Update() {

        if (Input.GetKeyDown(KeyCode.P))
            BuildWall();
    }

    private void BuildWall() {

        Vector3 currMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int mousePos2D = new Vector2Int( (int) (currMousePosition.x + 0.5f), (int) (currMousePosition.y + 0.5f) );

        Tile t = GameManager.Instance.world.GetTileAt(mousePos2D);
        t.TestFunc(!t.isTraversable);

        if (t.isTraversable)
            t.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        else
            t.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        GameManager.Instance.pathfinder.UpdateSystem();
    }
}
