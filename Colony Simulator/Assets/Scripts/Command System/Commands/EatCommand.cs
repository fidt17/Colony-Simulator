using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatCommand : Command
{
    private HungerComponent hungerComponent;
    private IEdible _food;

    public EatCommand(HungerComponent hungerComponent, IEdible food) {

        this.hungerComponent = hungerComponent;
        _food = food;

        ((StaticObject) _food).OnDestroy += OnFoodDestroyed;
    }

    public override void Abort() { 

        ((StaticObject) _food).OnDestroy -= OnFoodDestroyed;
    }

    public override void Execute() {

        if (_food == null) {
            Finish(false);
            return;
        }

        ((StaticObject) _food).OnDestroy -= OnFoodDestroyed;
        _food.Eat(hungerComponent);
        Finish(true);
    }

    private void OnFoodDestroyed(StaticObject food) {

        Finish(false);
    }
}
