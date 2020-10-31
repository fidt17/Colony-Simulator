using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableComponent : MonoBehaviour {

    public ISelectable selectable;
    private GameObject _selectionRim;

    public SelectableComponent(ISelectable selectable, GameObject selectionRim) {
        this.selectable = selectable;
        _selectionRim = selectionRim;
        Deselect();
    }
    
    public void Initialize(ISelectable selectable, GameObject selectionRim) {
        this.selectable = selectable;
        _selectionRim = selectionRim;
        Deselect();
    }

    public void Select() {
        _selectionRim?.SetActive(true);
        selectable.Select();
    }

    public void Deselect() {
        _selectionRim?.SetActive(false);
        selectable.Deselect();
    }

    public void DisableComponent() {
        //base.DisableComponent();
        Deselect();
    }

    #region Testing

    public bool CheckInitialization() {
        if (selectable is null) {
           return false;
        }

        if (_selectionRim is null) {
            return false;
        }

        if (_selectionRim.active == true) {
            return false;
        }

        return true;
    }

    #endregion
}