using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Interfaces;

public class CancelJobInputState : CommandInputState
{
    private readonly List<StaticJob> _staticJobs = new List<StaticJob>();
    private readonly List<ConstructionPlan> _constructionPlans = new List<ConstructionPlan>();
    private fidt17.Utils.IntRectangle _selectionArea;

    protected override void SubscribeToEvents()
    {
        InputListener.GetInstance().OnMouse0_Down += OnLeftClickDown;
        InputListener.GetInstance().OnMouse0_Up   += OnLeftClickUp;
        InputListener.GetInstance().OnMouse1_Down += SwitchToDefaultState;
        InputListener.GetInstance().OnEscape_Down += SwitchToDefaultState;

        SelectionTracker.GetInstance().OnAreaChange += OnAreaChange;
    }

    protected override void UnsubscribeFromEvents()
    {
        InputListener.GetInstance().OnMouse0_Down -= OnLeftClickDown;
        InputListener.GetInstance().OnMouse0_Up   -= OnLeftClickUp;
        InputListener.GetInstance().OnMouse1_Down -= SwitchToDefaultState;
        InputListener.GetInstance().OnEscape_Down -= SwitchToDefaultState;

        SelectionTracker.GetInstance().OnAreaChange -= OnAreaChange;
    }

    protected override void UpdateCursorTexture() => CursorManager.Instance.SwitchTexture(CursorManager.Instance.cutStateTexture);

    private void OnLeftClickDown() => SelectionTracker.GetInstance().OnLeftMouseButtonDown();
    private void OnLeftClickUp()   => SelectionTracker.GetInstance().OnLeftMouseButtonUp();

    protected virtual void OnAreaChange(SelectionTracker.OnAreaChangeArgs args)
    {
        if (_selectionArea?.CompareTo(args.rectangle) == false || _selectionArea == null)
        {
            _selectionArea = args.rectangle;
            GetNewStaticJobs();
        }

        if (args.dragEnded)
        {
            CancelJobs();
        }
    }

    private void GetNewStaticJobs()
    {
        FilterTilesBySelectionArea<StaticJob>(_staticJobs);
        FilterTilesBySelectionArea<ConstructionPlan>(_constructionPlans);
    
        foreach (Vector2Int position in _selectionArea.GetPositions())
        {
            Tile t = Utils.TileAt(position.x, position.y);
            if (t?.Contents is null || t.Contents.StaticJobs.Count == 0 && t.Contents.ConstructionPlan is null)
            {
                continue;
            }
            
            foreach (StaticJob sj in t.Contents.StaticJobs)
            { 
                _staticJobs.Add(sj);
            }

            if (t.Contents.ConstructionPlan != null)
            {
                _constructionPlans.Add(t.Contents.ConstructionPlan);
            }
        }
    }

    private void FilterTilesBySelectionArea<T>(IList<T> origin) where T : IPosition
    {
        for (int i = origin.Count - 1; i >= 0; i--)
        {
            if (_selectionArea.Contains(origin[i].Position) == false)
            {
                origin.Remove(origin[i]);
            }
        }
    }   

    private void CancelJobs()
    {
        _staticJobs.ForEach(s => s.DeleteJob());
        _staticJobs.Clear();

        _constructionPlans.ForEach(c => c.CancelPlan());
        _constructionPlans.Clear();
    }
}