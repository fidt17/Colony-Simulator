using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CutCommandInputState : CommandInputState {

    protected override void UnsubscribeFromEvents() {
        InputListener.GetInstance().OnMouse0_Down -= OnLeftClickDown;
        InputListener.GetInstance().OnMouse0_Up   -= OnLeftClickUp;
        InputListener.GetInstance().OnMouse1_Down -= SwitchToDefaultState;
        InputListener.GetInstance().OnEscape_Down -= SwitchToDefaultState;

        SelectionTracker.GetInstance().OnSelect -= CreateJobsOnSelected;
    }

    protected override void SubscribeToEvents() {
        InputListener.GetInstance().OnMouse0_Down += OnLeftClickDown;
        InputListener.GetInstance().OnMouse0_Up   += OnLeftClickUp;
        InputListener.GetInstance().OnMouse1_Down += SwitchToDefaultState;
        InputListener.GetInstance().OnEscape_Down += SwitchToDefaultState;

        SelectionTracker.GetInstance().OnSelect += CreateJobsOnSelected;
    }

    protected override void SetupSelectionTracker() {
        SelectionSettings settings;
        settings.selectionMask = new List<System.Type>() {
            typeof(Tree)
        };
        settings.shouldDrawArea = true;
        SelectionTracker.GetInstance().SetSettings(settings);
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