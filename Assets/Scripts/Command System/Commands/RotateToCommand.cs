using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class RotateToCommand : Command {
       
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

    private Node _destinationNode;
    private List<Node> _path;

    private MotionComponent _motionComponent;

    public RotateToCommand(MotionComponent motionComponent, Node destinationNode) {
        _motionComponent = motionComponent;
        _destinationNode = destinationNode;
    }

    public override void Execute() {
        SetFacingDirection(_destinationNode.Position - _motionComponent.GridPosition);
        _motionComponent.Stop();
        Finish(true);
    }

    public override void Abort() => _motionComponent.Stop();

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
}