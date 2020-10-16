using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable {
    SelectableComponent selectableComponent { get; }
    void InitializeSelectableComponent();
    void OnSelect();
    void OnDeselect();
}
