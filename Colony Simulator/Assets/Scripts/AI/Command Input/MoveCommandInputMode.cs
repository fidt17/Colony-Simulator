using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommandInputMode : CommandInputMode {

    private SelectionController _selectionController;

    public MoveCommandInputMode() {

        _selectionController = SelectionController.Instance;
    }

    public override void SubscribeToEvents() => InputController.Instance.OnMouse1_Down += OnRightMouseButtonDown;

    public override void UnsubscribeFromEvents() => InputController.Instance.OnMouse1_Down -= OnRightMouseButtonDown;

    private void OnRightMouseButtonDown() {

        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;

        List<SelectableComponent> selectedCharacters = _selectionController.GetSelectedColonists();
        if (selectedCharacters == null)
            return;

        //TODO add controls over multiple characters
        Human colonist = selectedCharacters[0].selectable as Human;

        Vector2Int nodeCoordinates = CursorToTileCoordinates();
        PathNode node = GameManager.Instance.pathfinder.grid.GetNodeAt(nodeCoordinates);

        if (node == null)
            return;

        Task moveTask = new Task();
        moveTask.AddCommand(new MoveCommand(colonist.motionComponent, node));
        colonist.AI.commandProcessor.AddUrgentTask(moveTask);
    }

    private Vector2Int CursorToTileCoordinates() {

        Vector2 currMousePosition = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2Int( (int) (currMousePosition.x + 0.5f), (int) (currMousePosition.y + 0.5f) );
    } 
}