using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CancelJobInputState : CommandInputState {

    private Dictionary<Tile, StaticJob> _staticJobs = new Dictionary<Tile, StaticJob>();
    private Dictionary<Tile, ConstructionPlan> _constructionPlans = new Dictionary<Tile, ConstructionPlan>();
    private fidt17.Utils.IntRectangle _selectionArea;

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
                GetNewStaticJobs();
            }

            if (args.dragEnded) {
                CancelJobs();
            }
        }
    }

    private void GetNewStaticJobs() {
        FilterTilesBySelectionArea<StaticJob>(_staticJobs);
        FilterTilesBySelectionArea<ConstructionPlan>(_constructionPlans);
    
        foreach (Vector2Int position in _selectionArea.GetPositions()) {
            Tile t = Utils.TileAt(position.x, position.y);
            if (t is null || t.content is null || (t.content.StaticJobs.Count == 0 && t.content.constructionPlan is null)) {
                continue;
            }

            if (t.content.StaticJobs.Count > 0) {
                if (_staticJobs.ContainsKey(t) == false) {
                    foreach (StaticJob sj in t.content.StaticJobs) {
                        _staticJobs.Add(t, sj);
                    }
                }
            }

            if (t.content.constructionPlan != null && _constructionPlans.ContainsKey(t) == false) {
                _constructionPlans.Add(t, t.content.constructionPlan);
            }
        }
    }

    private void FilterTilesBySelectionArea<T>(Dictionary<Tile, T> dic) {
        Dictionary<Tile, T> temp = new Dictionary<Tile, T>(dic);
        foreach (KeyValuePair<Tile, T> pair in temp) {
            if (_selectionArea.Contains(pair.Key.position) == false) {
                dic.Remove(pair.Key);
            }
        }
    }   

    private void CancelJobs() {
        Dictionary<Tile, StaticJob> temp = new Dictionary<Tile, StaticJob>(_staticJobs);
        foreach (KeyValuePair<Tile, StaticJob> pair in temp) {
            pair.Value.DeleteJob();
            _staticJobs.Remove(pair.Key);
        }

        Dictionary<Tile, ConstructionPlan> tempPlan = new Dictionary<Tile, ConstructionPlan>(_constructionPlans);
        foreach (KeyValuePair<Tile, ConstructionPlan> pair in tempPlan) {
            pair.Value.CancelConstruction();
            _constructionPlans.Remove(pair.Key);
        }
    }
}