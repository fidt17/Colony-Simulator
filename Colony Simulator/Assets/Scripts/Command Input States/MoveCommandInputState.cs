using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommandInputState : CommandInputState {

    protected override void UnsubscribeFromEvents() {
        InputController.GetInstance().OnMouse0_Down -= OnLeftMouseButtonDown;
        InputController.GetInstance().OnMouse1_Down -= OnRightMouseButtonDown;
    }

    protected override void SubscribeToEvents() {
        InputController.GetInstance().OnMouse0_Down += OnLeftMouseButtonDown;
        InputController.GetInstance().OnMouse1_Down += OnRightMouseButtonDown;
    }

    protected override void SetUpSelectionMask() {
        List<Type> selectionMask = new List<Type>();
        selectionMask.Add(typeof(Human));
        SelectionTracker.GetInstance().selectionMask = selectionMask;
    }

    protected override void UpdateCursorTexture() => CursorManager.Instance.SwitchTexture(CursorManager.Instance.moveStateTexture);

    private void OnLeftMouseButtonDown() {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        SwitchToDefaultState();
        (CommandInputStateMachine.currentCommandState as DefaultInputState).OnLeftMouseButtonDown();
    }

    private void OnRightMouseButtonDown() => MoveSelectedUnits();

    private void MoveSelectedUnits() {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        PathNode cursorNode = Pathfinder.NodeAt(Utils.CursorToCoordinates());
        if (cursorNode is null) {
            return;
        }

        List<Human> colonists = SelectionTracker.GetInstance().GetSelected<Human>();
        List<PathNode> targetNodes = DijkstraSearch.DijkstraFor(colonists.Count, cursorNode);
        for (int i = 0; i < targetNodes.Count; i++) {
            Task moveTask = new Task();
            moveTask.AddCommand(new MoveCommand(colonists[i].motionComponent, targetNodes[i]));
            colonists[i].AI.CommandProcessor.AddUrgentTask(moveTask);
        }
    }
}