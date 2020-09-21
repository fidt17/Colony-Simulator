using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutCommandInputMode : CommandInputMode {

    public override void SubscribeToEvents() {

        InputController.Instance.OnMouse0_Down += OnLeftClickDown;
        InputController.Instance.OnEscape_Down += OnEscapeDown;
    }

    public override void UnsubscribeFromEvents() {

        InputController.Instance.OnMouse0_Down -= OnLeftClickDown;
        InputController.Instance.OnEscape_Down -= OnEscapeDown;
    }

    private void OnLeftClickDown() {

        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;

        RaycastHit2D hit = Physics2D.Raycast((Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider == null)
            return;

        VegetationComponent vegetation = hit.collider.gameObject.GetComponent<VegetationComponent>();

        if (vegetation == null || vegetation.Type != VegetationType.tree)
            return;
        
        Tree tree = vegetation.Vegetation as Tree;
        CutJob cutJob = new CutJob(tree, tree.position);
        JobSystem.Instance.AddJob(cutJob);
    }

    private void OnEscapeDown() {

        CommandInput.Instance.SwitchCommand(new EmptyCommandInputMode());
    }

    private Vector2Int CursorToTileCoordinates() {

        Vector2 currMousePosition = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2Int( (int) (currMousePosition.x + 0.5f), (int) (currMousePosition.y + 0.5f) );
    }
}
