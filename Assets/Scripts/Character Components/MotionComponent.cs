using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FacingDirection {
    east = 0,
    north = 1,
    west = 2,
    south = 3
}

public class MotionComponent : CharacterComponent {

    public delegate void OnVelocityChange(Vector2 newVelocty, FacingDirection facingDirection);
    public event OnVelocityChange VelocityHandler;

    public delegate void PositionHandler(Vector3 position);
    public event PositionHandler OnPositionChange;

    public delegate void GridPositionHandler(Vector2Int previousPosition, Vector2Int currentPosition);
    public event GridPositionHandler OnGridPositionChange;

    public Vector2 WorldPosition => (Vector2) _gameObject.transform.position;
    public Vector2Int GridPosition => _lastTraversableNode.position;
    public PathNode PathNode => Utils.NodeAt(GridPosition);
    public float SpeedValue => _speed;

    public FacingDirection facingDirection = FacingDirection.south;

    private float _speed;
    private PathNode _lastTraversableNode;
    private GameObject _gameObject;

    public MotionComponent(float speed, Vector2Int position, GameObject gameObject) {
        _speed = speed;
        _gameObject = gameObject;
        _gameObject.transform.position = Utils.ToVector3(position);
        _lastTraversableNode = Utils.NodeAt(Utils.ToVector2Int(position));
    }
        
    public void Stop() {
        if (PathNode.isTraversable == false) {
            SetPosition(_lastTraversableNode.position);
        }
        VelocityHandler?.Invoke(Vector2.zero, facingDirection);
    }

    public void SetPosition(Vector2 position) {
        _gameObject.transform.position = position;
        OnGridPositionChange?.Invoke(_lastTraversableNode.position, Utils.ToVector2Int(position));
        _lastTraversableNode = Utils.NodeAt(Utils.ToVector2Int(position));
    } 

    public void Translate(Vector2 normalizedDestination) {
        _gameObject.transform.Translate(normalizedDestination * _speed * Time.deltaTime);
        VelocityHandler?.Invoke(normalizedDestination, facingDirection);

        PathNode currentNode = Utils.NodeAt(Utils.ToVector2Int(WorldPosition));
        if (currentNode.isTraversable) {
            OnGridPositionChange?.Invoke(_lastTraversableNode.position, currentNode.position);
            _lastTraversableNode = currentNode;
        }
        OnPositionChange?.Invoke(WorldPosition);
    }

    public void MoveCharacterToTraversableTile() {
        System.Func<Tile, bool> requirementsFunction = delegate(Tile t) {
            if (t == null) {
                return false;
            } else {
                return t.isTraversable;
            }
        };
        Tile tile = SearchEngine.FindClosestTileWhere(GridPosition, requirementsFunction, false);
        
        if (tile != null) {
            SetPosition(tile.position);
        } else {
            Debug.LogError("Cannot find traversable tile for character at" + GridPosition);
        }
    }

    public override void DisableComponent() {
        base.DisableComponent();

        VelocityHandler      = null;
        OnPositionChange     = null;
        OnGridPositionChange = null;

        _lastTraversableNode = null;
    }

    #region Testing

    public override bool CheckInitialization() {
        if (SpeedValue < 0) {
            return false;
        }

        if (_lastTraversableNode is null) {
            return false;
        }

        return true;
    }

    public bool CheckInitialization(Vector2Int supposedPosition) {

        if (CheckInitialization() == false) {
            return false;
        }

        //Position test
        if (_gameObject.transform.position != Utils.ToVector3(supposedPosition)) {
            return false;
        }

        if (_lastTraversableNode.position != supposedPosition) {
            return false;
        }

        return true;
    }

    #endregion
}