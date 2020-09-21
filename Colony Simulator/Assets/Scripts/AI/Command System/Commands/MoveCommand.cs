﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : Command {
    
    #region Directions

    private readonly Vector2Int NORTH      = new Vector2Int( 0,  1);
    private readonly Vector2Int NORTH_WEST = new Vector2Int(-1,  1);
    private readonly Vector2Int NORTH_EAST = new Vector2Int( 1,  1);
    private readonly Vector2Int WEST       = new Vector2Int(-1,  0);
    private readonly Vector2Int EAST       = new Vector2Int( 1,  0);
    private readonly Vector2Int SOUTH      = new Vector2Int( 0, -1);
    private readonly Vector2Int SOUTH_WEST = new Vector2Int(-1, -1);
    private readonly Vector2Int SOUTH_EAST = new Vector2Int( 1, -1);

    #endregion

    private PathNode _destinationNode;
    private List<PathNode> _path;

    private MotionComponent _motionComponent;

    public MoveCommand(MotionComponent motionComponent, PathNode destinationNode) {
        
        _motionComponent = motionComponent;
        _destinationNode = destinationNode;
    }

    public override void Execute() {

        if (HasPath() == false)
            return;

        MoveTowardsDestination();
    }

    private bool HasPath() {

        if (_path == null) {
            FindPath();

            if (_path == null) {
                Finish(false);
                return false;
            }
        }

        return true;
    }

    private void FindPath() {

        if(_destinationNode == null) {
            Finish(false);
            return;
        }

        _path = GameManager.Instance.pathfinder.GetPath(_motionComponent.GetGridPosition(), _destinationNode.position);
    }

    private void MoveTowardsDestination() {
        
        if (_path.Count == 0) {
            Finish(true);
            return;
        }

        Vector2Int nextNode = _path[0].position;
        Vector2 destination = nextNode - _motionComponent.GetWorldPosition();
        SetFacingDirection(nextNode - _motionComponent.GetGridPosition());

        float deltaSpeed = _motionComponent.SpeedValue * Time.deltaTime;

        //If character is close enough to the destination tile
        if (destination.sqrMagnitude <= Mathf.Pow(deltaSpeed, 2)) {

            _motionComponent.SetPosition(nextNode);
            _path.RemoveAt(0);
        } else {

            destination.Normalize();
            _motionComponent.Translate(destination);
        }
    }

    private void SetFacingDirection(Vector2Int destination) {

        if (destination == NORTH || destination == NORTH_EAST || destination == NORTH_WEST) {
            _motionComponent.facingDirection = FacingDirection.north;
        } else if (destination == EAST) {
            _motionComponent.facingDirection = FacingDirection.east;
        } else if (destination == WEST) {
            _motionComponent.facingDirection = FacingDirection.west;
        } else if (destination == SOUTH || destination == SOUTH_EAST || destination == SOUTH_WEST) {
            _motionComponent.facingDirection = FacingDirection.south;
        }
    }

    public override void Abort() => _motionComponent.Stop();

    public override void Finish(bool succeed) {
        
        base.Finish(succeed);
        _motionComponent.Stop();
    }
}