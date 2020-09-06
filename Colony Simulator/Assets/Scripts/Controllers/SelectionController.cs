using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    List<SelectableComponent> selected = new List<SelectableComponent>();

    private Vector2 currMousePosition;

    private void LateUpdate() {

        currMousePosition = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);

        CheckRightClick();
        CheckLeftClick();
    }

    private void CheckLeftClick() {

        if (Input.GetMouseButtonDown(0)) {

            DeselectEverything();

            RaycastHit2D hit = Physics2D.Raycast(currMousePosition, Vector2.zero);

            if (hit.collider != null) {
                
                SelectableComponent selectable = hit.collider.gameObject.GetComponent<SelectableComponent>();

                if (selectable != null) {

                    //TODO verify that all selected objects are of one type

                    selectable.Select();
                    selected.Add(selectable);
                }
            }
        }
    }

    private void CheckRightClick() {

        if (Input.GetMouseButtonDown(1) && selected.Count != 0) {
            
            //TODO
            //Add check on what types of objects are selected

            foreach (SelectableComponent sc in selected) {

                Human h = (Human) sc.entity;

                Vector2Int tileCoords = CursorToTileCoordinates();
                Tile t = GameManager.Instance.world.GetTileAt(tileCoords);

                h.SetDestination(t);
            }
        }
    }

    private void DeselectEverything() {

        foreach (SelectableComponent s in selected)
            s.Deselect();
    }

    private Vector2Int CursorToTileCoordinates() {
        return new Vector2Int( (int) (currMousePosition.x + 0.5f), (int) (currMousePosition.y + 0.5f) );
    }
}
