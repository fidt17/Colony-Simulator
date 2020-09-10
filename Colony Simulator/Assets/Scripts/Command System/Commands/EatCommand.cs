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

        ((StaticObject) food).OnDestroy += OnFoodDestroyed;
    }

    public override void Abort() { 

        ((StaticObject) food).OnDestroy -= OnFoodDestroyed;
    }

    public override void Execute() {

        if (food == null) {
            Finish(false);
            return;
        }

        ((StaticObject) food).OnDestroy -= OnFoodDestroyed;
        food.Eat(hungerComponent);
        Finish(true);
    }

    private void OnFoodDestroyed(StaticObject food) {

        Finish(false);
    }
}
