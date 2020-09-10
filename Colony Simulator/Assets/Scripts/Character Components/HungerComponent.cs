using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HungerComponent : MonoBehaviour
{   
    public Character character;

    public int hunger = 100;
    public float hungerTick = 1f;

    public List<Type> edibles = new List<Type>();

    private void Start() {

        StartCoroutine(DecreaseHunger());
    }

    private void Update() {

        if (hunger < 85)
            StartCoroutine(StartFoodSearch());

        if (hunger == 0)
            character.Die();
    }

    public void ChangeHunger(int value) {

        hunger = Mathf.Clamp(hunger + value, 0, 100);
    }

    private bool isSearchingForFood = false;
    private Task getFoodTask = null;
    private IEnumerator StartFoodSearch() {

        if (isSearchingForFood || edibles.Count == 0 || getFoodTask != null)
            yield break;
        
        isSearchingForFood = true;
        
        IEdible food = null;
        while (food == null) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 2f));
            food = GameManager.Instance.natureManager.FindClosestFood(edibles, character.motionComponent.GetGridPosition());
        }

        getFoodTask = new Task();
        getFoodTask.AddCommand(new MoveCommand(character.motionComponent, GameManager.Instance.world.GetTileAt(food.GetEdiblePosition())));
        getFoodTask.AddCommand(new EatCommand(this, food));
        getFoodTask.TaskResultHandler += HandleGetFoodTaskResult;

        character.commandProcessor.AddTask(getFoodTask);
        character.commandProcessor.StartExecution();

        while(getFoodTask != null)
            yield return null;
    }

    private void HandleGetFoodTaskResult(bool result) {

        getFoodTask = null;
        isSearchingForFood = false;
    }

    private IEnumerator DecreaseHunger() {

        while (true) {
            yield return new WaitForSeconds(hungerTick);
            ChangeHunger(-1);
        }
    }
}
