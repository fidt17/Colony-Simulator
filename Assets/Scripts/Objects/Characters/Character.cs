using System.Collections.Generic;
using UnityEngine;

public abstract class Character : IPrefab, ISelectable {

    public CommandProcessor CommandProcessor => AI.CommandProcessor;

    public AIController        AI                  { get; protected set; }
    
    public SelectableComponent SelectableComponent { get; private set; }
    public MotionComponent     MotionComponent     { get; private set; }
    public HungerComponent     HungerComponent     { get; private set; }

    public CharacterScriptableObject Data       { get; private set; }
    public GameObject                gameObject { get; private set; }
    
    protected readonly List<CharacterComponent> Components = new List<CharacterComponent>();
    
    private Vector2Int _tempPosition;

    public virtual void Die() {
        RemoveFromTile();
        Components.ForEach(x => x.DisableComponent());
        Destroy();
    }

    public virtual void SetData(PrefabScriptableObject data, Vector2Int position) {
        Data = data as CharacterScriptableObject;
        _tempPosition = position;
    } 

    public virtual void SetGameObject(GameObject obj) {
        gameObject = obj;

        //Components initialization
        InitializeSelectableComponent();
        InitializeMotionComponent(_tempPosition);
        InitializeHungerComponent();
        InitializeAI();

        AddToTile();
    }

    public virtual void Destroy() {
        Object.Destroy(gameObject);
    } 
    
    protected abstract void InitializeAI();
    
    protected virtual void InitializeMotionComponent(Vector2Int position) {
        MotionComponent = new MotionComponent(Data.movementSpeed, position, gameObject);
        MotionComponent.OnGridPositionChange += HandleGridPosition;
        Components.Add(MotionComponent);
    }

    protected virtual void InitializeHungerComponent() {
        HungerComponent = new HungerComponent(this);
        Components.Add(HungerComponent);
    }

    protected virtual void InitializeSelectableComponent() {
        SelectableComponent = gameObject.AddComponent<SelectableComponent>();
        SelectableComponent.Initialize(this, gameObject.transform.Find("SelectionRim").gameObject);
    }

    private void HandleGridPosition(Vector2Int previousPosition, Vector2Int currentPosition) {
        Utils.TileAt(previousPosition).content.RemoveCharacter(this);
        Utils.TileAt(currentPosition).content.AddCharacter(this);
    }

    private void AddToTile() {
        Utils.TileAt(MotionComponent.GridPosition).content.AddCharacter(this);
    } 

    private void RemoveFromTile() {
        Utils.TileAt(MotionComponent.GridPosition).content.RemoveCharacter(this);
    }

    public virtual void Select()   { }
    public virtual void Deselect() { }
}