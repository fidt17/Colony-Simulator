using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HungerComponent : MonoBehaviour {   
    
    public delegate void HandleHungerLevel(float value);
    public event HandleHungerLevel HungerLevelHandler;

    public float HungerLevel => _hunger;

    public List<Type> edibles = new List<Type>();// List of things character can eat.

    private Character _character;

    private float _hunger = 100;
    private float _hungerDecreasePerSecond = 1;

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
        HungerLevelHandler?.Invoke(_hunger);
        if (_hunger == 0) {
            _character.Die();
        }
    }
}