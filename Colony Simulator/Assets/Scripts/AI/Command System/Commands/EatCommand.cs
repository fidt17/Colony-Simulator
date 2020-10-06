using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatCommand : Command {

    private IEdible _food;
    private HungerComponent _hungerComponent;

    public EatCommand(HungerComponent hungerComponent, IEdible food) {
        _hungerComponent = hungerComponent;
        _food = food;
        (_food as IDestroyable).OnDestroyed += OnFoodDestroyed;
    }

    public override void Execute() {
        (_food as IDestroyable).OnDestroyed -= OnFoodDestroyed;
        _hungerComponent.Eat(_food);
        Finish(true);
    }

    public override void Abort() => (_food as IDestroyable).OnDestroyed -= OnFoodDestroyed;
    
    private void OnFoodDestroyed(object sender, EventArgs e) => Finish(false);
}