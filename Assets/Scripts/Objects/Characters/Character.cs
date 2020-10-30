using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : IPrefab, ISelectable {

    public CommandProcessor CommandProcessor => AI.CommandProcessor;
    public AIController AI { get; protected set; }

    #region Components

    protected List<CharacterComponent> _components = new List<CharacterComponent>();
    public SelectableComponent selectableComponent { get; protected set; }
    public MotionComponent     motionComponent     { get; protected set; }
    public HungerComponent     hungerComponent     { get; protected set; }

    #endregion

    public CharacterScriptableObject data { get; protected set; }
    public GameObject gameObject { get; protected set; }

    private Vector2Int _tempPosition;

    public virtual void Die() {
        RemoveFromTile();

        foreach (CharacterComponent component in _components) {
            component.DisableComponent();
        }
        
        Destroy();
    }

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

    public virtual void Destroy() {
        GameObject.Destroy(gameObject);
    } 

    #endregion

    protected virtual void InitializeMotionComponent(Vector2Int position) {
        motionComponent = new MotionComponent(data.movementSpeed, position, gameObject);
        _components.Add(motionComponent);

        motionComponent.OnGridPositionChange += HandleGridPosition;
    }

    protected virtual void InitializeHungerComponent() {
        hungerComponent = new HungerComponent(this);
        _components.Add(hungerComponent);
    }

    protected virtual void InitializeSelectableComponent() {
        selectableComponent = new SelectableComponent(this, gameObject.transform.Find("SelectionRim").gameObject);
        _components.Add(selectableComponent);
    }

    protected abstract void InitializeAI();

    private void HandleGridPosition(Vector2Int previousPosition, Vector2Int currentPosition) {
        Utils.TileAt(previousPosition).content.RemoveCharacter(this);
        Utils.TileAt(currentPosition).content.AddCharacter(this);
    }

    private void AddToTile() {
        Utils.TileAt(motionComponent.GridPosition).content.AddCharacter(this);
    } 

    private void RemoveFromTile() {
        Utils.TileAt(motionComponent.GridPosition).content.RemoveCharacter(this);
    }

    #region ISelectable

    public virtual void OnSelect() { }
    public virtual void OnDeselect() { }
        
    #endregion

    
}