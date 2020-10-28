using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FacingDirection {
    east = 0,
    north = 1,
    west = 2,
    south = 3
}

public class MotionComponent : MonoBehaviour {

    public delegate void OnVelocityChange(Vector2 newVelocty, FacingDirection facingDirection);
    public event OnVelocityChange VelocityHandler;

    public delegate void PositionHandler(Vector3 position);
    public event PositionHandler OnPositionChange;

    public delegate void GridPositionHandler(Vector2Int previousPosition, Vector2Int currentPosition);
    public event GridPositionHandler OnGridPositionChange;

    public Vector2 WorldPosition => (Vector2) gameObject.transform.position;
    public Vector2Int GridPosition => _lastTraversableNode.position;
    public PathNode PathNode => Utils.NodeAt(GridPosition);
    public float SpeedValue => _speed;

    private PathNode _lastTraversableNode;

    public FacingDirection facingDirection = FacingDirection.south;

    private float _speed;

    public void Initialize(float speed, Vector2Int position) {
        _speed = speed;
        SetPosition(position);
    }
        
    public void Stop() {
        if (PathNode.isTraversable == false) {
            SetPosition(_lastTraversableNode.position);
        }
        VelocityHandler?.Invoke(Vector2.zero, facingDirection);
    }

    public void SetPosition(Vector2 position) {
        gameObject.transform.position = position;
        OnGridPositionChange?.Invoke(_lastTraversableNode.position, Utils.ToVector2Int(position));
        _lastTraversableNode = Utils.NodeAt(Utils.ToVector2Int(position));
    } 

    public void Translate(Vector2 normalizedDestination) {
        gameObject.transform.Translate(normalizedDestination * _speed * Time.deltaTime);
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
}