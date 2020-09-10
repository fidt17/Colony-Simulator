using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : Command
{   
    private MotionComponent motionComponent;

    private Tile destinationTile;
    private List<PathNode> path;

    public MoveCommand(MotionComponent motionComponent, Tile destinationTile) {
        
        this.motionComponent = motionComponent;
        this.destinationTile = destinationTile;
    }

    public void FindPath() {

        //float startTime = Time.realtimeSinceStartup;
        path = GameManager.Instance.pathfinder.GetPath(motionComponent.GetGridPosition(), destinationTile.position);
        //Debug.Log(Time.realtimeSinceStartup - startTime);
    }

    private bool HasPath() {

        if (path == null) {
            FindPath();

            if (path == null) {
                Finish(false);
                return false;
            }
        }

        return true;
    }

    public override void Execute() {

        if (HasPath() == false)
            return;

        MoveTowardsDestination();
    }

    public override void Abort() {

        motionComponent.Stop();
    }

    private void MoveTowardsDestination() {
        
        if (path.Count == 0) {
            Finish(true);
            return;
        }

        Vector2 nextNode = path[0].position;
        Vector2 destination = nextNode - motionComponent.GetWorldPosition();

        float deltaSpeed = motionComponent.speed * Time.deltaTime;

        //If character is close enough to the destination tile
        if (destination.sqrMagnitude <= Mathf.Pow(deltaSpeed, 2)) {

            motionComponent.SetPosition(nextNode);
            path.RemoveAt(0);
        } else {

            destination.Normalize();
            motionComponent.Translate(destination);
        }
    }
}
