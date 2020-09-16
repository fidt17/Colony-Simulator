using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//TODO verify that all selected objects are of one type

public class SelectionController : MonoBehaviour {

    private List<SelectableComponent> _selected = new List<SelectableComponent>();

    private Vector2 _currMousePosition;

    private void LateUpdate() {

        _currMousePosition = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);

        CheckRightClick();
        CheckLeftClick();
    }

    private void CheckLeftClick() {

        if (Input.GetMouseButtonDown(0)) {

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            DeselectEverything();

            RaycastHit2D hit = Physics2D.Raycast(_currMousePosition, Vector2.zero);

            if (hit.collider != null) {
                
                SelectableComponent selectableComponent = hit.collider.gameObject.GetComponent<SelectableComponent>();

                if (selectableComponent != null) {

                    selectableComponent.Select();
                    _selected.Add(selectableComponent);

                    if (selectableComponent.selectable is Character)
                        UIManager.Instance.OpenCharacterWindow(selectableComponent.selectable as Character);
                }
            }
        }
    }

    private void CheckRightClick() {

        if (Input.GetMouseButtonDown(1) && _selected.Count != 0 && !EventSystem.current.IsPointerOverGameObject()) {

            foreach (SelectableComponent selectableComponent in _selected) {
                
                if (!(selectableComponent.selectable is Character))
                    continue;

                Character character = (Character) selectableComponent.selectable;

                Vector2Int tileCoords = CursorToTileCoordinates();
                Tile t = GameManager.Instance.world.GetTileAt(tileCoords);

                Task moveTask = new Task();
                moveTask.AddCommand(new MoveCommand(character.motionComponent, t));
                character.commandProcessor.AddUrgentTask(moveTask);
            }
        }
    }

    private void DeselectEverything() {

        foreach (SelectableComponent s in _selected)
            s.Deselect();
        
        UIManager.Instance.CloseCharacterWindow();
        _selected = new List<SelectableComponent>();
    }

    private Vector2Int CursorToTileCoordinates() => new Vector2Int( (int) (_currMousePosition.x + 0.5f), (int) (_currMousePosition.y + 0.5f) );
}
