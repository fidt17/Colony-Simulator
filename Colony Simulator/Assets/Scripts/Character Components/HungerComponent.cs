using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HungerComponent : MonoBehaviour {   
    
    public float HungerLevel => _hunger;

    public List<Type> edibles = new List<Type>();// List of things character can eat.

    private Character _character;

    private float _hunger = 100;
    private float _hungerDecreasePerSecond = 1;
    private const float _foodSearchThreshold = 70;//FIX ME

    public void Initialize(Character character) {

        _character = character;
        _hungerDecreasePerSecond = _character.data.hungerDecreasePerSecond;
    }

    private void Start() => StartCoroutine(DecreaseHunger());

    private void OnDestroy() {

        if (getFoodTask != null)
            getFoodTask.TaskResultHandler -= HandleGetFoodTaskResult;
    }

    public void ChangeHunger(float value) {

        _hunger = Mathf.Clamp(_hunger + value, 0, 100);
        CheckHunger();
    }

    #region Food search

    //temporal solution untill I create more advanced AI controller
    private bool isSearchingForFood = false;
    private Task getFoodTask = null;

    private IEnumerator StartFoodSearch() {

        if (isSearchingForFood || edibles.Count == 0 || getFoodTask != null)
            yield break;
        
        isSearchingForFood = true;
        
        IEdible food = null;
        PathNode targetNode = null;
        while (food is null || targetNode is null) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 2f));
            food = ItemFinder.FindClosestFood(edibles, _character.motionComponent.GetGridPosition(), ref targetNode);
        }
        
        getFoodTask = new Task();
        getFoodTask.AddCommand(new MoveCommand(_character.motionComponent, GameManager.Instance.world.GetTileAt(targetNode.position)));
        getFoodTask.AddCommand(new RotateToCommand(_character.motionComponent, GameManager.Instance.world.GetTileAt(food.GetEdiblePosition())));
        getFoodTask.AddCommand(new EatCommand(this, food));
        getFoodTask.TaskResultHandler += HandleGetFoodTaskResult;

        _character.commandProcessor.AddTask(getFoodTask);
        _character.commandProcessor.StartExecution();

        while (getFoodTask != null)
            yield return null;
    }
    ///////////

    private void HandleGetFoodTaskResult(bool result) {

        getFoodTask = null;
        isSearchingForFood = false;
    }

    #endregion

    private IEnumerator DecreaseHunger() {

        while (true) {
            yield return new WaitForSeconds(1);
            ChangeHunger(-_hungerDecreasePerSecond);
            CheckHunger();
        }
    }

    private void CheckHunger() {
        
        if (_hunger < _foodSearchThreshold)
            StartCoroutine(StartFoodSearch());

        if (_hunger == 0)
            _character.Die();
    }
}