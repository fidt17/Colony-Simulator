using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript : MonoBehaviour
{   
    private void Update() {

        if (Input.GetKeyDown(KeyCode.I))
            SpawnItem("wood log");
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

        Vector3 currMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int mousePos2D = new Vector2Int( (int) (currMousePosition.x + 0.5f), (int) (currMousePosition.y + 0.5f) );

        WoodLog w = Factory.Create<WoodLog>(name, mousePos2D);
        //ItemSpawnFactory.GetNewItem(name, name, mousePos2D);
    }
}


/*
TODO

1. CommandInputStateMachine has currentCommandState variable set a public(selection controller uses it). Fix Selection Controller and make currentCommandState private.


*. Add controls over multiple characters



*/