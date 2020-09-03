using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    public static SelectionController Instance;

    List<SelectableController> selected = new List<SelectableController>();

    private void Awake() {

        if (Instance != null) {

            Debug.LogError("Only one SelectionController can exist at a time!");
            Destroy(gameObject);
        }

        Instance = this;
    }

    private Vector3 currMousePosition;

    private void LateUpdate() {

        currMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        CheckRightClick();
        CheckLeftClick();
        UpdateTileCoordsTMP();
    }

    private void CheckLeftClick() {

        if (Input.GetMouseButtonDown(0)) {

            DeselectEverything();

            Vector2 mousePos2D = new Vector2(currMousePosition.x, currMousePosition.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null) {
                
                SelectableController selectable = hit.collider.gameObject.GetComponent<SelectableController>();

                if (selectable != null) {

                    selectable?.Select();
                    selected.Add(selectable);
                }
            }
        }
    }

    private void CheckRightClick() {

        if (Input.GetMouseButtonDown(1) && selected.Count != 0) {
            
            //TODO
            //Add check on what types of objects are selected

            foreach (SelectableController sc in selected) {

                Human h = (Human) sc.entity;

                Vector2Int tileCoords = new Vector2Int( (int) (currMousePosition.x + 0.5f), (int) (currMousePosition.y + 0.5f) );
                Tile t = GameManager.Instance.world.GetTileAt(tileCoords);

                h.motionController.SetDestination(t);
            }
        }
    }

    private void DeselectEverything() {

        foreach (SelectableController s in selected) {
            s.Deselect();
        }
    }

    private void UpdateTileCoordsTMP() {

        Vector2Int tileCoords = new Vector2Int( (int) (currMousePosition.x + 0.5f), (int) (currMousePosition.y + 0.5f) );
        UIManager.Instance.SetTileCoords(tileCoords);
    }
}
