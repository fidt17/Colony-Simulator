using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommandInputState : CommandInputState {

    private List<Human> _colonists;

    public MoveCommandInputState() {
        _colonists = SelectionTracker.Colonists;
        if (_colonists is null)
            CommandInputStateMachine.SwitchCommandState(new DefaultInputState());
    }

    public override void UpdateCursorTexture() => CursorManager.Instance.SwitchTexture(CursorManager.Instance.moveStateTexture);

    public override void SubscribeToEvents() {
        InputController.Instance.OnMouse0_Down += OnLeftMouseButtonDown;
        InputController.Instance.OnMouse1_Down += OnRightMouseButtonDown;
    } 

    public override void UnsubscribeFromEvents() {
        InputController.Instance.OnMouse0_Down -= OnLeftMouseButtonDown;
        InputController.Instance.OnMouse1_Down -= OnRightMouseButtonDown;
    }

    private void OnLeftMouseButtonDown() {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        DefaultInputState state = new DefaultInputState();
        CommandInputStateMachine.SwitchCommandState(state);
        state.OnLeftMouseButtonDown();
    }

    private void OnRightMouseButtonDown() {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()
            || _colonists.Count == 0) {
            return;
        }
        
        Human colonist = _colonists[0];

        Vector2Int nodeCoordinates = CursorToTileCoordinates();
        PathNode node = GameManager.Instance.pathfinder.grid.GetNodeAt(nodeCoordinates);
        if (node is PathNode) {
            Task moveTask = new Task();
            moveTask.AddCommand(new MoveCommand(colonist.motionComponent, node));

            colonist.AI.commandProcessor.AddUrgentTask(moveTask);
        }
    }

    private Vector2Int CursorToTileCoordinates() {
        Vector2 currMousePosition = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2Int( (int) (currMousePosition.x + 0.5f), (int) (currMousePosition.y + 0.5f) );
    } 
}