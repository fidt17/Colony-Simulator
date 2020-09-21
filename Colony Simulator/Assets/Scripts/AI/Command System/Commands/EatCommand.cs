using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatCommand : Command {

    private IEdible _food;
    private HungerComponent _hungerComponent;

    public EatCommand(HungerComponent hungerComponent, IEdible food) {

        _hungerComponent = hungerComponent;
        _food = food;

        ((StaticObject) _food).OnDestroy += OnFoodDestroyed;
    }

    public override void Execute() {

        if (_food == null) {
            Finish(false);
            return;
        }

        ((StaticObject) _food).OnDestroy -= OnFoodDestroyed;
        _food.Eat(_hungerComponent);
        Finish(true);
    }

    public override void Abort() => ((StaticObject) _food).OnDestroy -= OnFoodDestroyed;

    private void OnFoodDestroyed(StaticObject food) => Finish(false);
}