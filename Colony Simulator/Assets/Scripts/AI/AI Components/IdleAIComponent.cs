using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAIComponent {
    
    private Character _character;
    private AIController _AI;

    private float _tryCooldown = 5f;
    private float searchOffset = 5;

    public IdleAIComponent(Character character, AIController AI) {

        _character = character;
        _AI = AI;
        _AI.StartCoroutine(TryToWander());
    }

    //Looking for a nearby position to go to. (Character can wander while he has nothing else to do)
    private IEnumerator TryToWander() {

        while(true) {

            yield return new WaitForSeconds(_tryCooldown);

            if (!_AI.commandProcessor.IsFree)
                continue;
            
            PathNode targetNode = null;
            Vector2Int startPosition = _character.motionComponent.GetGridPosition();
            PathNode startNode = GameManager.Instance.pathfinder.grid.GetNodeAt(startPosition);

            while (targetNode == null) {
                
                Vector3 randomPosition = Random.insideUnitSphere * searchOffset;
                Vector2Int checkPosition = new Vector2Int((int) (startPosition.x + randomPosition.x), (int) (startPosition.y + randomPosition.y));

                PathNode checkNode = GameManager.Instance.pathfinder.grid.GetNodeAt(checkPosition);
                
                if (checkNode == null) 
                    continue;

                if (checkNode.region == startNode.region)
                    targetNode = checkNode;
            }

            Task wanderTask = new Task();
            wanderTask.AddCommand(new MoveCommand(_character.motionComponent, targetNode));
            wanderTask.AddCommand(new WaitCommand(2f));
            
            _AI.commandProcessor.AddTask(wanderTask);
        }
    }
}
