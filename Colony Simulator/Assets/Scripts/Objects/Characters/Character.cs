using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : IPrefab, ISelectable, IMovable, IHunger {

    public CommandProcessor CommandProcessor => AI.CommandProcessor;

    #region Components

    public SelectableComponent selectableComponent { get; protected set; }
    public MotionComponent     motionComponent     { get; protected set; }
    public HungerComponent     hungerComponent     { get; protected set; }
    public AIController        AI                  { get; protected set; }

    #endregion

    public CharacterScriptableObject data { get; protected set; }
    public GameObject gameObject { get; protected set; }

    public virtual void Die() {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        Destroy();
    }

    #region IPrefab

    public virtual void SetData(PrefabScriptableObject data) => this.data = data as CharacterScriptableObject;

    public virtual void SetGameObject(GameObject obj, Vector2Int position) {
        gameObject = obj;
        InitializeSelectableComponent();
        InitializeMotionComponent(position);
        InitializeHungerComponent();
        InitializeAI();
    }

    public virtual void Destroy() => GameObject.Destroy(gameObject, 1);

    #endregion

    #region Motion Component

    public virtual void InitializeMotionComponent(Vector2Int position) {
        motionComponent = gameObject.AddComponent<MotionComponent>();
        motionComponent.Initialize(data.movementSpeed, position);
    }

    #endregion

    #region Hunger Component

    public virtual void InitializeHungerComponent() {
        hungerComponent = gameObject.AddComponent<HungerComponent>();
        hungerComponent.Initialize(this);
    }

    #endregion

    #region Selectable Component

    public virtual void InitializeSelectableComponent() {
        selectableComponent = gameObject.AddComponent<SelectableComponent>();
        selectableComponent.Initialize(this, gameObject.transform.Find("SelectionRim").gameObject);
    }

    public virtual void OnSelect() { }
    public virtual void OnDeselect() { }
        
    #endregion

    #region Command Processor

    protected abstract void InitializeAI();

    #endregion
}