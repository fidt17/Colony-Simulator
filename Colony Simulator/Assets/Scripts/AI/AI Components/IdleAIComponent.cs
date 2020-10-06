using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAIComponent {
    
    private Character _character;

    private const float _coroutineCooldown = 5f;
    private const float _idleWaitTime = 5f;
    private const float _searchOffset = 5;

    public IdleAIComponent(Character character) {
        _character = character;
        _character.CommandProcessor.StartCoroutine(TryToWander());
    }

    private IEnumerator TryToWander() {
        while(true) {
            yield return new WaitForSeconds(_coroutineCooldown);

            if (_character.CommandProcessor.HasTask == true) {
                continue;
            }
            
            PathNode targetNode = null;
            PathNode startNode = _character.motionComponent.PathNode;

            while (targetNode is null) {
                Vector3 randomPosition = Random.insideUnitSphere * _searchOffset;
                Vector2Int checkPosition = new Vector2Int((int) (startNode.X + randomPosition.x), (int) (startNode.Y + randomPosition.y));
                PathNode checkNode = Pathfinder.NodeAt(checkPosition);
                
                if (checkNode?.region == startNode.region) {
                    targetNode = checkNode;
                }
            }

            Task wanderTask = new Task();
            wanderTask.AddCommand(new MoveCommand(_character.motionComponent, targetNode));
            wanderTask.AddCommand(new WaitCommand(_idleWaitTime));
            
            _character.CommandProcessor.AddTask(wanderTask);
        }
    }
}