﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HungerComponent : CharacterComponent {   
    
    public float HungerLevel { get; private set; } = 100;

    public readonly List<Type> Edibles = new List<Type>(); // List of things character can eat.

    private readonly float _hungerDecreasePerSecond;

    private Character   _character;
    private EatFoodTask _eatFoodTask;

    public HungerComponent(Character character) {
        _character = character;
        _hungerDecreasePerSecond = _character.Data.hungerDecreasePerSecond;
        RunCoroutine(DecreaseHunger());
    }

    public void Eat(IEdible food) {
        ChangeHunger(food.NutritionValue);
        (food as IPrefab).Destroy();
    }

    private void ChangeHunger(float value) {
        HungerLevel = Mathf.Clamp(HungerLevel + value, 0, 100);
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
        if (HungerLevel < _character.Data.hungerSearchThreshold) {
            RunCoroutine(StartFoodSearch());
        }

        if (HungerLevel == 0) {
            _character.Die();
        }
    }

    private bool _isSearchingForFood = false;
    private IEnumerator StartFoodSearch() {
        if (_isSearchingForFood || Edibles.Count == 0) {
            yield break;
        }
        _isSearchingForFood = true;

        Grass grass = null;
        while (grass is null) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 2f));
            grass = SearchEngine.FindClosestGrass(_character.MotionComponent.GridPosition);
        }
        
        _eatFoodTask = new EatFoodTask(_character, Pathfinder.FindNodeNear(Utils.NodeAt(grass.Position), _character.MotionComponent.PathNode), grass);
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

    public override void DisableComponent() {
        base.DisableComponent();

        _character = null;
        _eatFoodTask = null;
    }

    #region Testing

    public override bool CheckInitialization() {
        if (_character is null) {
           return false;
        }

        if (_hungerDecreasePerSecond == -1) {
            return false;
        }

        return true;
    }

    #endregion
}