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

        if (hunger < 95)
            StartCoroutine(StartFoodSearch());
    }

    public void ChangeHunger(int value) {

        hunger += value;
        hunger = Mathf.Clamp(hunger, 0, 100);
    }

    private bool isSearchingForFood = false;
    private Task getFoodTask = null;
    private IEnumerator StartFoodSearch() {

        if (isSearchingForFood || edibles.Count == 0 || getFoodTask != null)
            yield break;
        
        isSearchingForFood = true;
        
        Grass food = null;
        while (food == null) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 10f));

            //temporal solution
            //float startTime = Time.realtimeSinceStartup;//Debug
            food = GameManager.Instance.natureManager.FindClosestFood(edibles, character.motionComponent.GetGridPosition());
            //Debug.Log("Found food in: " + (Time.realtimeSinceStartup - startTime));//Debug

            if( food != null )
                break;
        }

        getFoodTask = new Task();
        getFoodTask.AddCommand(new MoveCommand(character.motionComponent, GameManager.Instance.world.GetTileAt(food.position)));
        getFoodTask.AddCommand(new EatCommand(this, food));
        getFoodTask.TaskResultHandler += HandleGetFoodTaskResult;

        character.AddTask(getFoodTask);
        character.StartTaskExecution();

        while(getFoodTask != null) {
            yield return null;
        }

        isSearchingForFood = false;
    }

    private void HandleGetFoodTaskResult(bool result) {

        getFoodTask = null;
    }

    private IEnumerator DecreaseHunger() {

        while (true) {
            yield return new WaitForSeconds(hungerTick);
            ChangeHunger(-1);
        }
    }
}
