using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class MoveCommandInputState : CommandInputState {

    protected override void UnsubscribeFromEvents() {
        InputListener.GetInstance().OnMouse0_Down -= OnLeftMouseButtonDown;
        InputListener.GetInstance().OnMouse1_Down -= OnRightMouseButtonDown;
    }

    protected override void SubscribeToEvents() {
        InputListener.GetInstance().OnMouse0_Down += OnLeftMouseButtonDown;
        InputListener.GetInstance().OnMouse1_Down += OnRightMouseButtonDown;
    }

    protected override void SetupSelectionTracker() {
        SelectionSettings settings = new SelectionSettings();
        settings.selectionMask = new List<System.Type>() {
            typeof(Human)
        };
        settings.shouldDrawArea = true;
        SelectionTracker.GetInstance().SetSettings(settings);
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
        Node cursorNode = Utils.NodeAt(Utils.CursorToCoordinates());
        if (cursorNode is null) {
            return;
        }

        List<Human> colonists = SelectionTracker.GetInstance().GetSelected<Human>();
        List<Node> targetNodes = Dijkstra.DijkstraFor(colonists.Count, cursorNode);
        for (int i = 0; i < targetNodes.Count; i++) {
            Task moveTask = new Task();
            moveTask.AddCommand(new MoveCommand(colonists[i].MotionComponent, targetNodes[i].Position));
            colonists[i].AI.CommandProcessor.AddUrgentTask(moveTask);
        }
    }
}