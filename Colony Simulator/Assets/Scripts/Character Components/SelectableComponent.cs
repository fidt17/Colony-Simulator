using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableComponent : MonoBehaviour
{
    public ISelectable selectable;
    private GameObject _selectionRim;

    public bool isSelected { get; private set; }

    public void Initialize(ISelectable selectable, GameObject selectionRim) {

        this.selectable = selectable;
        _selectionRim = selectionRim;
        Deselect();
    }

    public void Select() {

        isSelected = true;
        _selectionRim?.SetActive(true);
    }

    public void Deselect() {

        isSelected = false;
        _selectionRim?.SetActive(false);
    }

    public void OnDestroy() {

        
    }
}
