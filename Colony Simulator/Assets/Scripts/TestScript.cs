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
            DrawSubregions();

        if (Input.GetKeyDown(KeyCode.T))
            TestT();

        if (Input.GetKeyDown(KeyCode.Y))
            Factory.Create<Construction>("wall", Utils.CursorToCoordinates());

        if (Input.GetKeyDown(KeyCode.U))
            SearchEngine.GetTypeDerivativeOf<Item>("WoodLog");
    }

    private void DrawSubregions() {
        PathfinderRenderer.GetInstance().drawSubregions = !PathfinderRenderer.GetInstance().drawSubregions;
    }

    private void TestT() {
        Func<int> funcA = delegate() {
            
            return 1;
		};

        Func<int> funcB = delegate() {
            
            return 1;
		};

        CompareExecutionTime(funcA, funcB, 1000);
    }

    private void CompareExecutionTime(Func<int> funcA, Func<int> funcB, int iterationCount) {
        float aTime = 0;
        float bTime = 0;

        for (int i = 0; i < iterationCount; i++) {
            aTime += FunctionExecutionTime(funcA) / iterationCount;
            bTime += FunctionExecutionTime(funcB) / iterationCount;
        }

        string result = (aTime >= bTime) ? "BFunction is " + (1 - bTime/aTime)*100 + "% faster than AFunction."
                                         : "AFunction is " + (1 - aTime/bTime)*100 + "% faster than BFunction."; 
        Debug.Log("AFunction: " + aTime + "; BFunction: " + bTime + " => " + result);
    }

    private float FunctionExecutionTime(Func<int> func) {
        float startTime = Time.realtimeSinceStartup;
        func();
        return Time.realtimeSinceStartup - startTime;
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

        Tile t = SearchEngine.FindClosestTileWhere(Utils.CursorToCoordinates(), requirementsFunction, false);
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