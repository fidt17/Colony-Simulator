using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerAIComponent {

    private Character _character;
    private EatFoodTask _eatFoodTask;

    public HungerAIComponent(Character character) {
        _character = character;
        AssignListeners();
    }

    public void AssignListeners() => _character.hungerComponent.HungerLevelHandler += HandleHungerLevel;

    public void UnassignListeners() {
        _character.hungerComponent.HungerLevelHandler -= HandleHungerLevel;

        if (_eatFoodTask is EatFoodTask) {
            _eatFoodTask.TaskResultHandler -= HandleGetFoodTaskResult;
        }
    }

    private void HandleHungerLevel(float hungerLevel) {
        if (hungerLevel < _character.data.hungerSearchThreshold) {
            _character.CommandProcessor.StartCoroutine(StartFoodSearch());
        }
    }

    private bool _isSearchingForFood = false;
    private IEnumerator StartFoodSearch() {
        if (_isSearchingForFood
            || _character.hungerComponent.edibles.Count == 0)
            yield break;

        _isSearchingForFood = true;

        IEdible food = null;
        PathNode targetNode = null;

        while (food is null || targetNode is null) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 2f));
            food = ItemFinder.FindClosestFood(_character.hungerComponent.edibles, _character.motionComponent.GridPosition, ref targetNode);
        }
        
        _eatFoodTask = new EatFoodTask(_character, targetNode, food);
        _eatFoodTask.TaskResultHandler += HandleGetFoodTaskResult;
        _character.AI.commandProcessor.AddTask(_eatFoodTask);

        while (_eatFoodTask != null) {
            yield return null;
        }
    }

    private void HandleGetFoodTaskResult(bool result) {
        _eatFoodTask = null;
        _isSearchingForFood = false;
    }
}