using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatFoodTask : Task {

    public EatFoodTask(Character character, PathNode targetNode, IEdible food) {
        this.AddCommand(new MoveCommand(character.MotionComponent, targetNode.position));
        this.AddCommand(new RotateToCommand(character.MotionComponent, Utils.NodeAt(food.GetEdiblePosition())));
        this.AddCommand(new WaitCommand(0.5f));
        this.AddCommand(new EatCommand(character.HungerComponent, food));
    }
}
