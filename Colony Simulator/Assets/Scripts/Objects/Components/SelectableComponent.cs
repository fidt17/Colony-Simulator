using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableComponent : MonoBehaviour {

    public ISelectable selectable;

    private GameObject _selectionRim;

    public void Initialize(ISelectable selectable, GameObject selectionRim) {
        this.selectable = selectable;
        _selectionRim = selectionRim;
        Deselect();
    }

    public void Select() {
        _selectionRim?.SetActive(true);
        selectable.OnSelect();
    }

    public void Deselect() {
        _selectionRim?.SetActive(false);
        selectable.OnDeselect();
    }
}