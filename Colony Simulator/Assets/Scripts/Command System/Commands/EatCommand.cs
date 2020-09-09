using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatCommand : Command
{
    private HungerComponent hungerComponent;
    private IEdible food;

    public EatCommand(HungerComponent hungerComponent, IEdible food) {

        this.hungerComponent = hungerComponent;
        this.food = food;
    }

    public override void Execute() {

        base.Execute();

        if (food == null) {
            Abort();
            return;
        }

        hungerComponent.ChangeHunger(food.NutritionValue);
        food.Eat();

        Finish();
    }
}
