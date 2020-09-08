﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : IGameObject, ISelectable, IMovable
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

    public MotionComponent motionComponent { get; protected set; }
    public SelectableComponent selectableComponent;
    public CommandProcessor commandProcessor;

    #endregion

    public Character() { }

    public virtual void SetData(CharacterScriptableObject data) {

        this.data = data;
    }

    public virtual void SetGameObject(GameObject gameObject, Vector2Int position) {

        _gameObject = gameObject;

        InitializeSelectableComponent();
        InitializeMotionComponent();
        InitializeCommandProcessor();

        motionComponent.SetPosition(position);
    }

    #region Motion Component

    public void InitializeMotionComponent() {

        motionComponent = _gameObject.AddComponent<MotionComponent>();
        motionComponent.SetSpeed(data.movementSpeed);
    }

    #endregion

    #region Selectable Component

    public void InitializeSelectableComponent() {

        selectableComponent = _gameObject.AddComponent<SelectableComponent>();
        selectableComponent.entity = this;
        selectableComponent.selectionRim = _gameObject.transform.Find("SelectionRim").gameObject;

        if (selectableComponent.selectionRim == null)
            Debug.LogError("No SelectionRim was found on this gameObject:", _gameObject);

        selectableComponent.Deselect();
    }
    
    #endregion

    #region Command Processor

    public void InitializeCommandProcessor() {

        commandProcessor = _gameObject.AddComponent<CommandProcessor>();
    }

    public void AddCommand(Command command) {

        commandProcessor.AddCommand(command);
    }

    public void AddUrgentCommand(Command command) {
        
        commandProcessor.ResetCommands();
        AddCommand(command);
    }

    public void StartCommandExecution() {

        commandProcessor.ExecuteNextCommand();
    }

    #endregion
}
