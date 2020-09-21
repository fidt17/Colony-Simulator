using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatFoodTask : Task {

    public EatFoodTask(Character character, PathNode targetNode, IEdible food) {

        this.AddCommand(new MoveCommand(character.motionComponent, targetNode));
        this.AddCommand(new RotateToCommand(character.motionComponent, GameManager.Instance.pathfinder.grid.GetNodeAt(food.GetEdiblePosition())));
        this.AddCommand(new WaitCommand(0.5f));
        this.AddCommand(new EatCommand(character.hungerComponent, food));
    }
}
