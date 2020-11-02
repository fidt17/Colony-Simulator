﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAIComponent : CharacterComponent {
    
    private Character _character;

    private const float _coroutineCooldown = 10f;
    private const float _idleWaitTime = 5f;
    private const float _searchOffset = 5;

    public IdleAIComponent(Character character, CommandProcessor cp) {
        _character = character;
        cp.StartCoroutine(TryToWander());
    }

    private IEnumerator TryToWander() {
        while(true) {
            yield return new WaitForSeconds(Random.Range(5, _coroutineCooldown));

            if (_character.CommandProcessor.HasTask == true) {
                continue;
            }
            
            PathNode targetNode = null;
            PathNode startNode = _character.MotionComponent.PathNode;

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
            wanderTask.AddCommand(new MoveCommand(_character.MotionComponent, targetNode.position));
            wanderTask.AddCommand(new WaitCommand(_idleWaitTime));
            
            _character.CommandProcessor.AddTask(wanderTask);
        }
    }

    public override bool CheckInitialization() {
        throw new System.NotImplementedException();
    }
}