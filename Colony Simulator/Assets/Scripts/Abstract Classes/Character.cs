using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : ISelectable
{
    public GameObject gameObject { get; protected set; }

    public SelectableComponent selectableComponent;

    public Character(GameObject gameObject) {

        this.gameObject = gameObject;
        InitSelectableComponent();
    }

    #region Selectable Component

    public void InitSelectableComponent() {

        selectableComponent = gameObject.AddComponent<SelectableComponent>();
        selectableComponent.entity = this;

        foreach(Transform child in gameObject.transform) {
            if (child.name == "SelectionRim") {
                selectableComponent.selectionRim = child.gameObject;
                return;
            }
        }

        if (selectableComponent.selectionRim == null)
            Debug.LogError("No SelectionRim was found on this gameObject:", gameObject);

        selectableComponent.Deselect();
    }
    
    #endregion
}
