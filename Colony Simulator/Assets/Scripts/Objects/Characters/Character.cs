using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : IGameObject, ISelectable, IMovable, IHunger
{
    public abstract string Name { get; }
    public CharacterScriptableObject data;

    protected GameObject _gameObject;
    public GameObject GameObject {
        get {
            return _gameObject;
        }
    }

    #region Components

    public SelectableComponent selectableComponent { get; protected set; }
    public MotionComponent     motionComponent     { get; protected set; }
    public HungerComponent     hungerComponent     { get; protected set; }
    public CommandProcessor    commandProcessor    { get; protected set; }

    #endregion

    public Character() { }

    public virtual void SetData(CharacterScriptableObject data) {

        this.data = data;
    }

    public virtual void SetGameObject(GameObject gameObject, Vector2Int position) {

        _gameObject = gameObject;

        InitializeSelectableComponent();
        InitializeMotionComponent(position);
        InitializeHungerComponent();
        InitializeCommandProcessor();
    }

    public virtual void Die() {

        _gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        GameObject.Destroy(_gameObject, 1);
    }

    #region Motion Component

    public virtual void InitializeMotionComponent(Vector2Int position) {

        motionComponent = _gameObject.AddComponent<MotionComponent>();
        motionComponent.speed = data.movementSpeed;
        motionComponent.SetPosition(position);
    }

    #endregion

    #region Hunger Component

    public virtual void InitializeHungerComponent() {

        hungerComponent = _gameObject.AddComponent<HungerComponent>();
        hungerComponent.character = this;
    }

    #endregion

    #region Selectable Component

    public virtual void InitializeSelectableComponent() {

        selectableComponent = _gameObject.AddComponent<SelectableComponent>();
        selectableComponent.entity = this;
        selectableComponent.selectionRim = _gameObject.transform.Find("SelectionRim").gameObject;

        if (selectableComponent.selectionRim == null)
            Debug.LogError("No SelectionRim was found on this gameObject:", _gameObject);

        selectableComponent.Deselect();
    }
    
    #endregion

    #region Command Processor

    public virtual void InitializeCommandProcessor() {

        commandProcessor = _gameObject.AddComponent<CommandProcessor>();
    }

    #endregion
}
