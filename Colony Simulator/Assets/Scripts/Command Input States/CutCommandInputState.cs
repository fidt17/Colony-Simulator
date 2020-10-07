using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CutCommandInputState : CommandInputState {

    protected override void UnsubscribeFromEvents() {
        InputController.GetInstance().OnMouse0_Down -= OnLeftClickDown;
        InputController.GetInstance().OnMouse0_Up   -= OnLeftClickUp;
        InputController.GetInstance().OnMouse1_Down -= SwitchToDefaultState;
        InputController.GetInstance().OnEscape_Down -= SwitchToDefaultState;

        SelectionTracker.GetInstance().OnSelect -= CreateJobsOnSelected;
    }

    protected override void SubscribeToEvents() {
        InputController.GetInstance().OnMouse0_Down += OnLeftClickDown;
        InputController.GetInstance().OnMouse0_Up   += OnLeftClickUp;
        InputController.GetInstance().OnMouse1_Down += SwitchToDefaultState;
        InputController.GetInstance().OnEscape_Down += SwitchToDefaultState;

        SelectionTracker.GetInstance().OnSelect += CreateJobsOnSelected;
    }

    protected override void SetUpSelectionMask() {
        List<Type> selectionMask = new List<Type>();
        selectionMask.Add(typeof(Tree));
        SelectionTracker.GetInstance().selectionMask = selectionMask;
    }

    protected override void UpdateCursorTexture() => CursorManager.Instance.SwitchTexture(CursorManager.Instance.cutStateTexture);

    private void OnLeftClickDown() => SelectionTracker.GetInstance().OnLeftMouseButtonDown();
    private void OnLeftClickUp() => SelectionTracker.GetInstance().OnLeftMouseButtonUp();

    private void CreateJobsOnSelected() {
        foreach (Tree tree in SelectionTracker.GetInstance().GetSelected<Tree>()) {
            if (JobExists(tree) == false) {
                JobSystem.GetInstance().AddJob(new CutJob(tree, tree.position));
            }
        }
    }

    private bool JobExists(Tree tree) {
        CutJob cutJob = JobSystem.GetInstance().AllJobs.Find(x => x.GetType() == typeof(CutJob) && (x as CutJob).Vegetation == tree) as CutJob;
        return cutJob != null;
    }
}