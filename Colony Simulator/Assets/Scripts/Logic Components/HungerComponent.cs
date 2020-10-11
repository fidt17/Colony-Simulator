using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HungerComponent : MonoBehaviour {   
    
    public float HungerLevel => _hunger;

    public List<Type> edibles = new List<Type>();// List of things character can eat.

    private Character _character;
    private EatFoodTask _eatFoodTask;

    private float _hunger = 100;
    private float _hungerDecreasePerSecond = 1;

    private bool _isSearchingForFood = false;

    public void Initialize(Character character) {
        _character = character;
        _hungerDecreasePerSecond = _character.data.hungerDecreasePerSecond;
    }

    private void Start() => StartCoroutine(DecreaseHunger());

    public void Eat(IEdible food) {
        ChangeHunger(food.NutritionValue);
        (food as IPrefab).Destroy();
    }

    public void ChangeHunger(float value) {
        _hunger = Mathf.Clamp(_hunger + value, 0, 100);
        CheckHunger();
    }

    private IEnumerator DecreaseHunger() {
        while (true) {
            yield return new WaitForSeconds(1);
            ChangeHunger(-_hungerDecreasePerSecond);
            CheckHunger();
        }
    }

    private void CheckHunger() {
        if (_hunger < _character.data.hungerSearchThreshold) {
            _character.CommandProcessor.StartCoroutine(StartFoodSearch());
        }

        if (_hunger == 0) {
            _character.Die();
        }
    }

    private IEnumerator StartFoodSearch() {
        if (_isSearchingForFood || _character.hungerComponent.edibles.Count == 0) {
            yield break;
        }
        _isSearchingForFood = true;

        IEdible food = null;
        PathNode targetNode = null;

        //FIX THIS. dijkstra search?
        while (food is null || targetNode is null) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 2f));
            food = ItemFinder.FindClosestFood(_character.hungerComponent.edibles, _character.motionComponent.GridPosition, ref targetNode);
        }
        
        _eatFoodTask = new EatFoodTask(_character, targetNode, food);
        _eatFoodTask.TaskResultHandler += HandleGetFoodTaskResult;
        _character.AI.CommandProcessor.AddTask(_eatFoodTask);

        while (_eatFoodTask != null) {
            yield return null;
        }
        _isSearchingForFood = false;
    }

    private void HandleGetFoodTaskResult(object source, EventArgs e) {
        _eatFoodTask.TaskResultHandler -= HandleGetFoodTaskResult;
        _eatFoodTask = null;
    } 
}