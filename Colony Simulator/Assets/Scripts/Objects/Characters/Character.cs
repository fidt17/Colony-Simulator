using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : ISelectable
{
    public GameObject gameObject { get; protected set; }

    public abstract string Name {

        get;
    }

    public SelectableComponent selectableComponent;

    public Character() { }

    public virtual void SetGameObject(GameObject gameObject) {

        this.gameObject = gameObject;
        InitSelectableComponent();
    }

    #region Selectable Component

    public void InitSelectableComponent() {

        selectableComponent = gameObject.AddComponent<SelectableComponent>();
        selectableComponent.entity = this;
        selectableComponent.selectionRim = gameObject.transform.Find("SelectionRim").gameObject;

        if (selectableComponent.selectionRim == null)
            Debug.LogError("No SelectionRim was found on this gameObject:", gameObject);

        selectableComponent.Deselect();
    }
    
    #endregion
}
