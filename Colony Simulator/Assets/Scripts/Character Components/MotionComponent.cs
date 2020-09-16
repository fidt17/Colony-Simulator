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

    public FacingDirection facingDirection = FacingDirection.south;
    public float SpeedValue => _speed;

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

    public Vector2 GetWorldPosition() => (Vector2) gameObject.transform.position;

    public Vector2Int GetGridPosition() {
        
        Vector2 worldPosition = GetWorldPosition();
        return new Vector2Int( (int) (worldPosition.x + 0.5f), (int) (worldPosition.y + 0.5f) );
    }
}