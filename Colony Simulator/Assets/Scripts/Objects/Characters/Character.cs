using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : IPrefab, ISelectable, IMovable, IHunger {

    public CharacterScriptableObject Data => _data;
    public GameObject GameObject => _gameObject;

    public CommandProcessor CommandProcessor => AI.CommandProcessor;

    #region Components

    public SelectableComponent selectableComponent { get; protected set; }
    public MotionComponent     motionComponent     { get; protected set; }
    public HungerComponent     hungerComponent     { get; protected set; }
    public AIController        AI                  { get; protected set; }

    #endregion

    protected CharacterScriptableObject _data;
    protected GameObject _gameObject;

    public Character() { }

    public virtual void Die() {
        _gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        Destroy();
    }

    #region IPrefab

    public virtual void SetData(PrefabScriptableObject data) => _data = data as CharacterScriptableObject;

    public virtual void SetGameObject(GameObject obj, Vector2Int position) {
        _gameObject = obj;
        InitializeSelectableComponent();
        InitializeMotionComponent(position);
        InitializeHungerComponent();
        InitializeAI();
    }

    public virtual void Destroy() {
        GameObject.Destroy(_gameObject, 1);
    }

    #endregion

    #region Motion Component

    public virtual void InitializeMotionComponent(Vector2Int position) {
        motionComponent = _gameObject.AddComponent<MotionComponent>();
        motionComponent.Initialize(_data.movementSpeed, position);
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