using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EatFoodTask : Task {

    public EatFoodTask(Character character, Node targetNode, IEdible food) {
        this.AddCommand(new MoveCommand(character.MotionComponent, targetNode.Position));
        this.AddCommand(new RotateToCommand(character.MotionComponent, Utils.NodeAt(food.GetEdiblePosition())));
        this.AddCommand(new WaitCommand(0.5f));
        this.AddCommand(new EatCommand(character.HungerComponent, food));
    }
}
