using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerAIComponent {

    private AIController _AI;
    private Character _character;

    private Task _eatFoodTask = null;
    private bool _isSearchingForFood = false;

    public HungerAIComponent(Character character, AIController AI) {

        _AI = AI;
        _character = character;
        AssignListeners();
    }

    public void AssignListeners() {

        _character.hungerComponent.HungerLevelHandler += HandleHungerLevel;
    }

    public void UnassignListeners() {

        _character.hungerComponent.HungerLevelHandler -= HandleHungerLevel;

        if (_eatFoodTask != null)
            _eatFoodTask.TaskResultHandler -= HandleGetFoodTaskResult;
    }

    private void HandleHungerLevel(float hungerLevel) {

        if (hungerLevel < _character.data.hungerSearchThreshold)
            _AI.StartCoroutine(StartFoodSearch());
    }

    private IEnumerator StartFoodSearch() {

        HungerComponent hungerComponent = _character.hungerComponent;
        List<Type> edibles = _character.hungerComponent.edibles;

        if (_isSearchingForFood
            || edibles.Count == 0
            || _eatFoodTask != null)
            yield break;
        
        _isSearchingForFood = true;
        
        IEdible food = null;
        PathNode targetNode = null;
        while (food is null || targetNode is null) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 2f));
            food = ItemFinder.FindClosestFood(edibles, _character.motionComponent.GetGridPosition(), ref targetNode);
        }
        
        _eatFoodTask = new EatFoodTask(_character, targetNode, food);
        _eatFoodTask.TaskResultHandler += HandleGetFoodTaskResult;
        _character.AI.commandProcessor.AddTask(_eatFoodTask);

        while (_eatFoodTask != null)
            yield return null;
    }

    private void HandleGetFoodTaskResult(bool result) {

        _eatFoodTask = null;
        _isSearchingForFood = false;
    }
}
