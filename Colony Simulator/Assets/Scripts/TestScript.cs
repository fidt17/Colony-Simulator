using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript : MonoBehaviour
{   
    private void Update() {

        if (Input.GetKeyDown(KeyCode.I))
            SpawnItem("wood log");

        if (Input.GetKeyDown(KeyCode.O))
            CutTree();
    }

    private void CutTree() {

        Vector3 currMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int mousePos2D = new Vector2Int( (int) (currMousePosition.x + 0.5f), (int) (currMousePosition.y + 0.5f) );

        Tile t = GameManager.Instance.world.GetTileAt(mousePos2D);

        if (t == null || t.objectOnTile?.GetType() != typeof(Tree))
            return;

        Tree tree = (Tree) t.objectOnTile;
        Character human = GameManager.Instance.characterManager.colonists[0];
        
        PathNode targetNode = GameManager.Instance.pathfinder.FindNodeNear(GameManager.Instance.pathfinder.grid.GetNodeAt(tree.position),
                                                                           GameManager.Instance.pathfinder.grid.GetNodeAt(human.motionComponent.GridPosition));

        CutTask cutTask = new CutTask(human, targetNode, tree);
        human.AI.CommandProcessor.AddTask(cutTask);
    }

    private void DestroyObject() {

        Vector3 currMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int mousePos2D = new Vector2Int( (int) (currMousePosition.x + 0.5f), (int) (currMousePosition.y + 0.5f) );

        Tile t = GameManager.Instance.world.GetTileAt(mousePos2D);

        if (t == null)
            return;

        t.itemOnTile?.Destroy();

        if (t.objectOnTile != null) {

            if (t.objectOnTile.GetType() == typeof(Tree))
                ( (IHarvestable) t.objectOnTile).Harvest();
            else {
                t.objectOnTile.Destroy();
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