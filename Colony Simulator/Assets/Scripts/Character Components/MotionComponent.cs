using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MotionComponent : MonoBehaviour
{
    [SerializeField]
    public float speed;

    //Control 8-way animation on a characted (at least for now it is the only use).
    public delegate void OnVelocityChange(Vector2 newVelocty);
    public event OnVelocityChange VelocityHandler;

    public void SetPosition(Vector2 position) {

        gameObject.transform.position = position;
        VelocityHandler?.Invoke(Vector2.zero);
    }

    public void SetSpeed(float newSpeed) {

        speed = newSpeed;
    }

    public void Translate(Vector2 normalizedDestination) {

        gameObject.transform.Translate(normalizedDestination * speed * Time.deltaTime);
        VelocityHandler?.Invoke(normalizedDestination);
    }

    public Vector2 GetWorldPosition() {

        return (Vector2) gameObject.transform.position;
    }

    public Vector2Int GetGridPosition() {

        Vector3 worldPosition = GetWorldPosition();
        return new Vector2Int( (int) (worldPosition.x + 0.5f), (int) (worldPosition.y + 0.5f) );
    }
}
