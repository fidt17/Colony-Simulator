using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable {
    SelectableComponent SelectableComponent { get; }
    void Select();
    void Deselect();
}
