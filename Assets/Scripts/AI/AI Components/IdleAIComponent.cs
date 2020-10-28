﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAIComponent {
    
    private Character _character;

    private const float _coroutineCooldown = 5f;
    private const float _idleWaitTime = 1f;
    private const float _searchOffset = 5;

    public IdleAIComponent(Character character) {
        _character = character;
        _character.CommandProcessor.StartCoroutine(TryToWander());
    }

    private IEnumerator TryToWander() {
        while(true) {
            yield return new WaitForSeconds(Random.Range(0, _coroutineCooldown));

            if (_character.CommandProcessor.HasTask == true) {
                continue;
            }
            
            PathNode targetNode = null;
            PathNode startNode = _character.motionComponent.PathNode;

            while (targetNode is null) {
                Vector3 randomPosition = Random.insideUnitSphere * _searchOffset;
                Vector2Int checkPosition = new Vector2Int((int) (startNode.x + randomPosition.x), (int) (startNode.y + randomPosition.y));
                PathNode checkNode = Utils.NodeAt(checkPosition);
                if (checkNode == null) {
                    continue;
                }
                
                if (Pathfinder.CompareCharacterRegionWith(_character, checkNode.Region)) {
                    targetNode = checkNode;
                    break;
                }
                //this waiting is needed for situations when character is surrounded by not traversable objects.
                yield return new WaitForSeconds(10);
            }

            Task wanderTask = new Task();
            wanderTask.AddCommand(new MoveCommand(_character.motionComponent, targetNode.position));
            wanderTask.AddCommand(new WaitCommand(_idleWaitTime));
            
            _character.CommandProcessor.AddTask(wanderTask);
        }
    }
}