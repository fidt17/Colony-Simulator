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

    public Vector2 WorldPosition => (Vector2) gameObject.transform.position;
    public Vector2Int GridPosition => new Vector2Int( (int) (WorldPosition.x + 0.5f), (int) (WorldPosition.y + 0.5f) );
    public PathNode PathNode => GameManager.Instance.pathfinder.grid.GetNodeAt(GridPosition);
    public float SpeedValue => _speed;

    public FacingDirection facingDirection = FacingDirection.south;

    private float _speed;

    public void Initialize(float speed, Vector2Int position) {
        _speed = speed;
        SetPosition(position);
    }
        
    public void Stop() => VelocityHandler?.Invoke(Vector2.zero, facingDirection);

    public void SetPosition(Vector2 position) => gameObject.transform.position = position;

    public void Translate(Vector2 normalizedDestination) {
        gameObject.transform.Translate(normalizedDestination * _speed * Time.deltaTime);
        VelocityHandler?.Invoke(normalizedDestination, facingDirection);
    }
}