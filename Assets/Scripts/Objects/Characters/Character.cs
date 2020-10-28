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

    private Vector2Int _tempPosition;

    public virtual void Die() {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        RemoveFromTile();
        Destroy();
    }

    private void HandleGridPosition(Vector2Int previousPosition, Vector2Int currentPosition) {
        Utils.TileAt(previousPosition).content.RemoveCharacter(this);
        Utils.TileAt(currentPosition).content.AddCharacter(this);
    }

    private void AddToTile() => Utils.TileAt(motionComponent.GridPosition).content.AddCharacter(this);
    private void RemoveFromTile() => Utils.TileAt(motionComponent.GridPosition).content.RemoveCharacter(this);

    #region IPrefab

    public virtual void SetData(PrefabScriptableObject data, Vector2Int position) {
        this.data = data as CharacterScriptableObject;
        _tempPosition = position;
    } 

    public virtual void SetGameObject(GameObject obj) {
        gameObject = obj;
        InitializeSelectableComponent();
        InitializeMotionComponent(_tempPosition);
        InitializeHungerComponent();
        InitializeAI();

        AddToTile();
    }

    public virtual void Destroy() => GameObject.Destroy(gameObject, 1);

    #endregion

    #region Motion Component

    public virtual void InitializeMotionComponent(Vector2Int position) {
        motionComponent = gameObject.AddComponent<MotionComponent>();
        motionComponent.Initialize(data.movementSpeed, position);
        motionComponent.OnGridPositionChange += HandleGridPosition;
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