using System.Collections;
using System.Collections.Generic;
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

    private Tile _destinationTile;
    private List<PathNode> _path;

    private MotionComponent _motionComponent;

    public RotateToCommand(MotionComponent motionComponent, Tile destinationTile) {
        
        _motionComponent = motionComponent;
        _destinationTile = destinationTile;
    }

    public override void Execute() {

        SetFacingDirection(_destinationTile.position - _motionComponent.GetGridPosition());
        _motionComponent.Stop();

        Finish(true);
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
}