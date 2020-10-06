using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommandInputState : CommandInputState {

    private List<Human> _colonists;

    public MoveCommandInputState() {
        _colonists = SelectionTracker.GetInstance().Colonists;
        if (_colonists is null) {
            CommandInputStateMachine.SwitchCommandState(new DefaultInputState());
        }
    }

    public override void UnsubscribeFromEvents() {
        InputController.GetInstance().OnMouse0_Down -= OnLeftMouseButtonDown;
        InputController.GetInstance().OnMouse1_Down -= OnRightMouseButtonDown;
    }

    protected override void SubscribeToEvents() {
        InputController.GetInstance().OnMouse0_Down += OnLeftMouseButtonDown;
        InputController.GetInstance().OnMouse1_Down += OnRightMouseButtonDown;
    }

    protected override void UpdateCursorTexture() => CursorManager.Instance.SwitchTexture(CursorManager.Instance.moveStateTexture);

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
        MoveSelectedUnits();
    }

    private void MoveSelectedUnits() {
        PathNode cursorNode = Pathfinder.NodeAt(Utils.CursorToCoordinates());
        if (cursorNode is null) {
            return;
        }

        List<PathNode> targetNodes = DijkstraSearch.DijkstraFor(_colonists.Count, cursorNode);
        for (int i = 0; i < targetNodes.Count; i++) {
            Task moveTask = new Task();
            moveTask.AddCommand(new MoveCommand(_colonists[i].motionComponent, targetNodes[i]));
            _colonists[i].AI.CommandProcessor.AddUrgentTask(moveTask);
        }
    }
}