using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : Command
{   
    private MotionComponent _motionComponent;

    private Tile _destinationTile;
    private List<PathNode> _path;

    public MoveCommand(MotionComponent motionComponent, Tile destinationTile) {
        
        _motionComponent = motionComponent;
        _destinationTile = destinationTile;
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

        //float startTime = Time.realtimeSinceStartup;
        _path = GameManager.Instance.pathfinder.GetPath(_motionComponent.GetGridPosition(), _destinationTile.position);
        //Debug.Log(Time.realtimeSinceStartup - startTime);
    }

    private void MoveTowardsDestination() {
        
        if (_path.Count == 0) {
            Finish(true);
            return;
        }

        Vector2 nextNode = _path[0].position;
        Vector2 destination = nextNode - _motionComponent.GetWorldPosition();

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

    public override void Abort() {

        _motionComponent.Stop();
    }

    public override void Finish(bool succeed) {
        
        base.Finish(succeed);
        _motionComponent.Stop();
    }
}
