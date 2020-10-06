using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CutCommandInputState : CommandInputState {

    private List<Tree> _selectedTrees;

    public CutCommandInputState() {
        _selectedTrees = SelectionTracker.Trees;
        if (_selectedTrees is null) {
            SelectionTracker.DeselectEverything();
        }
    }

    public override void UnsubscribeFromEvents() {
        InputController.Instance.OnMouse0_Down -= OnLeftClickDown;
        InputController.Instance.OnMouse1_Down -= ExitState;
        InputController.Instance.OnEscape_Down -= ExitState;
    }

    protected override void SubscribeToEvents() {
        InputController.Instance.OnMouse0_Down += OnLeftClickDown;
        InputController.Instance.OnMouse1_Down += ExitState;
        InputController.Instance.OnEscape_Down += ExitState;
    }

    protected override void UpdateCursorTexture() => CursorManager.Instance.SwitchTexture(CursorManager.Instance.cutStateTexture);

    private void OnLeftClickDown() {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast((Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        VegetationComponent vegetation = hit.collider?.gameObject.GetComponent<VegetationComponent>();

        if (vegetation?.Type == VegetationType.tree) {
            Tree tree = vegetation.Vegetation as Tree;
            if (JobExists(tree) == false) {
                CutJob cutJob = new CutJob(tree, tree.position);
                JobSystem.GetInstance().AddJob(cutJob);
            }
        }
    }

    private bool JobExists(Tree tree) {
        CutJob cutJob = JobSystem.GetInstance().AllJobs.Find(x => x.GetType() == typeof(CutJob) && (x as CutJob).Vegetation == tree) as CutJob;
        return cutJob != null;
    }

    private void ExitState() => CommandInputStateMachine.SwitchCommandState(new DefaultInputState());
}