using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : IGameObject, ISelectable, IMovable, IHunger {

    public abstract string Name { get; }

    public CharacterScriptableObject data;
    public GameObject GameObject => _gameObject;

    public CommandProcessor CommandProcessor => AI.commandProcessor;

    #region Components

    public SelectableComponent selectableComponent { get; protected set; }
    public MotionComponent     motionComponent     { get; protected set; }
    public HungerComponent     hungerComponent     { get; protected set; }
    public AIController        AI                  { get; protected set; }

    #endregion

    protected GameObject _gameObject;

    public Character() { }

    public virtual void SetData(CharacterScriptableObject data) => this.data = data;

    public virtual void SetGameObject(GameObject gameObject, Vector2Int position) {
        _gameObject = gameObject;
        InitializeSelectableComponent();
        InitializeMotionComponent(position);
        InitializeHungerComponent();
        InitializeAI();
    }

    public virtual void Die() {
        _gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        GameObject.Destroy(_gameObject, 1);
    }

    #region Motion Component

    public virtual void InitializeMotionComponent(Vector2Int position) {
        motionComponent = _gameObject.AddComponent<MotionComponent>();
        motionComponent.Initialize(data.movementSpeed, position);
    }

    #endregion

    #region Hunger Component

    public virtual void InitializeHungerComponent() {
        hungerComponent = _gameObject.AddComponent<HungerComponent>();
        hungerComponent.Initialize(this);
    }

    #endregion

    #region Selectable Component

    public virtual void InitializeSelectableComponent() {
        selectableComponent = _gameObject.AddComponent<SelectableComponent>();
        selectableComponent.Initialize(this, _gameObject.transform.Find("SelectionRim").gameObject);
    }

    public virtual void OnSelect() => UIManager.Instance.OpenCharacterWindow(this);
    public virtual void OnDeselect() => UIManager.Instance.CloseCharacterWindow();
    
    #endregion

    #region Command Processor

    protected abstract void InitializeAI();

    #endregion
}