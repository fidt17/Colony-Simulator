using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript : MonoBehaviour
{   
    private void Update() {

        if (Input.GetKey(KeyCode.I))
            SpawnItem("wood log");

        if (Input.GetKeyDown(KeyCode.P))
            SpawnHuman();

        if (Input.GetKeyDown(KeyCode.T))
            TestDijkstraSearch();
    }

    private void TestDijkstraSearch() {

        Func<Tile, bool> requirementsFunction = delegate(Tile tile) {
            if (tile == null) {
                return false;
            } else {
                if (tile.contents.staticObject != null) {
                    return tile.contents.staticObject.GetType().Equals(typeof(Tree));
                } else {
                    return false;
                }
            }
        };

        Tile t = DijkstraSearch.FindClosestTileWhere(Utils.CursorToCoordinates(), requirementsFunction, false);
        if (t == null) {
            Debug.Log("Tile was not found");
        } else {
            Debug.Log("Tile found at: " + t.position);
        }
    }

    private void DestroyObject() {

        Vector3 currMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int mousePos2D = new Vector2Int( (int) (currMousePosition.x + 0.5f), (int) (currMousePosition.y + 0.5f) );

        Tile t = Utils.TileAt(mousePos2D);

        if (t == null)
            return;

        t.contents.item?.Destroy();

        if (t.contents.staticObject != null) {

            if (t.contents.staticObject.GetType() == typeof(Tree))
                ( (IHarvestable) t.contents.staticObject).Harvest();
            else {
                t.contents.staticObject.Destroy();
            }
        }
    }

    private void SpawnItem(string name) {
        WoodLog w = Factory.Create<WoodLog>(name, Utils.CursorToCoordinates());
    }

    private void SpawnHuman() {
        Human human = Factory.Create<Human>("human", Utils.CursorToCoordinates());
        GameManager.GetInstance().characterManager.colonists.Add(human);
    }
}