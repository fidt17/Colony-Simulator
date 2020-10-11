using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StaticObject : IPrefab, IDestroyable, ISelectable {

    public event EventHandler OnDestroyed;

    public StaticScriptableObject data { get; protected set; }
    public GameObject gameObject       { get; protected set; }

    public Vector2Int position   { get; protected set; }
    public Vector2Int dimensions { get; protected set; }
    public bool isTraversable    { get; protected set; }

    public SelectableComponent selectableComponent { get; protected set; }

    #region IPrefab

    public virtual void SetData(PrefabScriptableObject data) {
        this.data = data as StaticScriptableObject;
        dimensions = this.data.dimensions;
        isTraversable = this.data.isTraversable;
    }

    public virtual void SetGameObject(GameObject obj, Vector2Int position) {
        gameObject = obj;
        gameObject.transform.position = new Vector3(position.x, position.y, 0);
        this.position = position;

        InitializeSelectableComponent();
    }

    public virtual void Destroy() {
        GameObject.Destroy(gameObject);
        OnDestroyed?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    #region Selectable Component

    public virtual void InitializeSelectableComponent() {
        selectableComponent = gameObject.AddComponent<SelectableComponent>();
        selectableComponent.Initialize(this, gameObject.transform.Find("SelectionRim")?.gameObject);
    }

    public virtual void OnSelect() { }
    public virtual void OnDeselect() { }
        
    #endregion
}
