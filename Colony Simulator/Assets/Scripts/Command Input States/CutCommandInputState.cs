﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CutCommandInputState : CommandInputState {

    private List<Tree> _treesInArea = new List<Tree>();

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

    protected override void SetupSelectionTracker() {
        SelectionSettings settings;
        settings.selectionMask = new List<System.Type>();
        settings.shouldDrawArea = true;
        SelectionTracker.GetInstance().SetSettings(settings);
    }

    protected override void UpdateCursorTexture() => CursorManager.Instance.SwitchTexture(CursorManager.Instance.cutStateTexture);

    private void OnLeftClickDown() => SelectionTracker.GetInstance().OnLeftMouseButtonDown();
    private void OnLeftClickUp() => SelectionTracker.GetInstance().OnLeftMouseButtonUp();

    protected virtual void OnAreaChange(object source, EventArgs e) {
        SelectionTracker.OnAreaChangeArgs args = e as SelectionTracker.OnAreaChangeArgs;
        _treesInArea = GetTreesOnTiles(Utils.GetTilesInArea(args.start, args.end));
        if (args.dragEnded) {
            CreateJobs(_treesInArea);
        }
    }

    private List<Tree> GetTreesOnTiles(List<Tile> tiles) {
        List<Tree> trees = new List<Tree>();
        for (int i = tiles.Count - 1; i >= 0; i--) {
            StaticObject staticObject = tiles[i].content.staticObject;
            if (staticObject != null && staticObject.GetType() == typeof(Tree) && JobExists(staticObject as Tree) == false) {
                trees.Add(staticObject as Tree);
            }
        }
        return trees;
    }

    private void CreateJobs(List<Tree> trees) {
        foreach (Tree tree in trees) {
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