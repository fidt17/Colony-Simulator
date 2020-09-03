using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionController : MonoBehaviour
{
    public Vector2 position { get; private set; }

    [SerializeField]
    private float speed;
    public Character entity;
    public Tile destinationTile;


    public void SetPosition(Vector2 position) {

        gameObject.transform.position = position;
        this.position = position;
    }

    public void SetSpeed(float newSpeed) {

        speed = newSpeed;
    }

    private void Update() {

        MoveTowardsDestination();
    }

    public void SetDestination(Tile destTile) {

        ResetPath();
        destinationTile = destTile;
    }

    public void ResetPath() {

        destinationTile = null;
    }

    private void MoveTowardsDestination() {

        if (destinationTile == null)
            return;

        Vector2 dest = destinationTile.position - position;

        float dSpeed = speed * Time.deltaTime;

        if (dest.sqrMagnitude <= Mathf.Pow(dSpeed, 2)) {
            SetPosition(destinationTile.position);
            destinationTile = null;

            entity.animationController.SetVelocity(new Vector2(0, 0));
            return;   
        }

        dest.Normalize();
        
        gameObject.transform.Translate(dest * dSpeed);
        position = gameObject.transform.position;

        entity.animationController.SetVelocity(dest);
    }
}
