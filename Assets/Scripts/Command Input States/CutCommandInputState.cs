using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CutCommandInputState : CommandInputState {

    private Dictionary<Tree, GameObject> _trees = new Dictionary<Tree, GameObject>();
    private fidt17.Utils.IntRectangle _selectionArea;
    private Stack<GameObject> _jobIconsPool = new Stack<GameObject>();

    public override void ExitState() {
        base.ExitState();

        //Removing non-valid trees from existing dictionary
        Dictionary<Tree, GameObject> temp = new Dictionary<Tree, GameObject>(_trees);
        foreach (KeyValuePair<Tree, GameObject> pair in temp) {
            _jobIconsPool.Push(pair.Value);
            _trees.Remove(pair.Key);
        }
        temp.Clear();

        while (_jobIconsPool.Count != 0) {
            GameObject.Destroy(_jobIconsPool.Pop());
        }
    }

    protected override void SubscribeToEvents() {
        InputListener.GetInstance().OnMouse0_Down += OnLeftClickDown;
        InputListener.GetInstance().OnMouse0_Up   += OnLeftClickUp;
        InputListener.GetInstance().OnMouse1_Down += SwitchToDefaultState;
        InputListener.GetInstance().OnEscape_Down += SwitchToDefaultState;

        SelectionTracker.GetInstance().OnAreaChange += OnAreaChange;
    }

    protected override void UnsubscribeFromEvents() {
        InputListener.GetInstance().OnMouse0_Down -= OnLeftClickDown;
        InputListener.GetInstance().OnMouse0_Up   -= OnLeftClickUp;
        InputListener.GetInstance().OnMouse1_Down -= SwitchToDefaultState;
        InputListener.GetInstance().OnEscape_Down -= SwitchToDefaultState;

        SelectionTracker.GetInstance().OnAreaChange -= OnAreaChange;
    }

    protected override void UpdateCursorTexture() => CursorManager.Instance.SwitchTexture(CursorManager.Instance.cutStateTexture);

    private void OnLeftClickDown() => SelectionTracker.GetInstance().OnLeftMouseButtonDown();
    private void OnLeftClickUp()   => SelectionTracker.GetInstance().OnLeftMouseButtonUp();

    protected virtual void OnAreaChange(object source, EventArgs e) {
        if (source is SelectionTracker) {
            SelectionTracker.OnAreaChangeArgs args = e as SelectionTracker.OnAreaChangeArgs;

            if (_selectionArea?.CompareTo(args.rectangle) == false || _selectionArea == null) {
                _selectionArea = args.rectangle;
                GetNewTrees();
            }

            if (args.dragEnded) {
                CreateJobs();
            }
        }
    }

    private void GetNewTrees() {
        //Removing non-valid trees from existing dictionary
        Dictionary<Tree, GameObject> temp = new Dictionary<Tree, GameObject>(_trees);
        foreach (KeyValuePair<Tree, GameObject> pair in temp) {
            if (_selectionArea.Contains(pair.Key.position) == false) {
                pair.Value.SetActive(false);
                _jobIconsPool.Push(pair.Value);
                _trees.Remove(pair.Key);
            }
        }
        temp.Clear();

        //Detecting new trees.
        foreach (Vector2Int position in _selectionArea.GetPositions()) {
            Tile t = Utils.TileAt(position.x, position.y);
            if (t is null || t.content is null || t.content.staticObject is null) {
                continue;
            }

            StaticObject staticObject = t.content.staticObject;

            if (staticObject.GetType() != typeof(Tree) || JobExists(staticObject as Tree)) {
                continue;
            }

            if (_trees.ContainsKey(staticObject as Tree) == false) {
                _trees.Add(staticObject as Tree, CreateJobIcon(staticObject as Tree));
            }
        }
    }

    private GameObject CreateJobIcon(Tree tree) {
       if (_jobIconsPool.Count != 0) {
           GameObject jobIcon = _jobIconsPool.Pop();
           jobIcon.transform.position = Utils.ToVector3(tree.position);
           jobIcon.SetActive(true);
           return jobIcon;
       }
       return Factory.Create("cut job", tree.position);
    }

    private void CreateJobs() {
        Dictionary<Tree, GameObject> temp = new Dictionary<Tree, GameObject>(_trees);
        foreach(KeyValuePair<Tree, GameObject> pair in temp) {
            if (JobExists(pair.Key) == false) {
                JobSystem.GetInstance().AddJob(new CutJob(pair.Key, pair.Key.position, pair.Value));
                _trees.Remove(pair.Key);
            }
        }
    }

    private bool JobExists(Tree tree) {
        CutJob cutJob = JobSystem.GetInstance().AllJobs.Find(x => x.GetType() == typeof(CutJob) && (x as CutJob).Vegetation == tree) as CutJob;
        return cutJob != null;
    }
}