using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HungerComponent : MonoBehaviour
{   
    private Character _character;

    private float _hunger = 100;
    public float HungerLevel {
        get {
            return _hunger;
        }
    }

    private float _hungerDecreasePerSecond = 1;
    private const float _foodSearchThreshold = 70;//if hunger level drops below this value, character will try to find food nearby.

    public List<Type> edibles = new List<Type>();// List of things character can eat.

    private void Start() {

        StartCoroutine(DecreaseHunger());
    }

    public void Initialize(Character character, float hungerDecreasePerSecond) {

        _character = character;
        _hungerDecreasePerSecond = hungerDecreasePerSecond;
    }

    private void Update() {

        CheckHunger();
    }

    public void ChangeHunger(float value) { _hunger = Mathf.Clamp(_hunger + value, 0, 100); }

    #region Food searching region

    private bool isSearchingForFood = false;
    private Task getFoodTask = null;
    private IEnumerator StartFoodSearch() {

        if (isSearchingForFood || edibles.Count == 0 || getFoodTask != null)
            yield break;
        
        isSearchingForFood = true;
        
        IEdible food = null;
        while (food == null) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 2f));
            food = GameManager.Instance.natureManager.FindClosestFood(edibles, _character.motionComponent.GetGridPosition());
        }

        getFoodTask = new Task();
        getFoodTask.AddCommand(new MoveCommand(_character.motionComponent, GameManager.Instance.world.GetTileAt(food.GetEdiblePosition())));
        getFoodTask.AddCommand(new EatCommand(this, food));
        getFoodTask.TaskResultHandler += HandleGetFoodTaskResult;

        _character.commandProcessor.AddTask(getFoodTask);
        _character.commandProcessor.StartExecution();

        while(getFoodTask != null)
            yield return null;
    }

    private void HandleGetFoodTaskResult(bool result) {

        getFoodTask = null;
        isSearchingForFood = false;
    }

    #endregion

    private IEnumerator DecreaseHunger() {

        while (true) {
            yield return new WaitForSeconds(1);
            ChangeHunger(-_hungerDecreasePerSecond);
        }
    }

    private void CheckHunger() {

        if (_hunger < _foodSearchThreshold)
            StartCoroutine(StartFoodSearch());

        if (_hunger == 0)
            _character.Die();
    }

    private void OnDestroy() {

        if (getFoodTask != null)
            getFoodTask.TaskResultHandler -= HandleGetFoodTaskResult;
    }
}
