using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class SelectionTracker {

    public static List<Human> Colonists {
        get {
            if (_selected.Count == 0 || _selected[0].selectable.GetType() != typeof(Human)) {
                return null;
            }

            List<Human> colonists = new List<Human>();
            _selected.ForEach(x => colonists.Add(x.selectable as Human));
            return colonists;
        }
    }

    public static List<Tree> Trees {
        get {
            return null;
        }
    }

    private static List<SelectableComponent> _selected = new List<SelectableComponent>();
    
    public static void Select(SelectableComponent selectableComponent) {
        DeselectEverything();
        _selected.Add(selectableComponent);
        selectableComponent.Select();
    }

    public static void DeselectEverything() {
        foreach (SelectableComponent s in _selected) {
            s.Deselect();
        }
        _selected.Clear();
    }
}