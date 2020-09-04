using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionController : MonoBehaviour
{
    public Vector2 position { get; private set; }
    public Vector2Int gridPosition { get; private set; }

    [SerializeField]
    private float speed;
    public Character entity;

    private List<PathNode> path;

    public void SetPosition(Vector2 position) {

        gameObject.transform.position = position;
        this.position = position;
        gridPosition = new Vector2Int( (int) (position.x + 0.5f), (int) (position.y + 0.5f) );
    }

    public void SetSpeed(float newSpeed) {

        speed = newSpeed;
    }

    private void Update() {

        MoveTowardsDestination();
    }

    public void SetDestination(Tile destTile) {

        if (destTile == null)
            return;

        ResetPath();

        path = GameManager.Instance.pathfinder.GetPath(gridPosition, destTile.position);
    }

    public void ResetPath() {

        path = new List<PathNode>();
        entity.animationController.SetVelocity(new Vector2(0, 0));
    }

    private void MoveTowardsDestination() {

        if (path == null || path.Count == 0)
            return;

        Vector2 nextNode = path[0].position;

        Vector2 dest = nextNode - position;

        float dSpeed = speed * Time.deltaTime;

        if (dest.sqrMagnitude <= Mathf.Pow(dSpeed, 2)) {
            SetPosition(nextNode);
            path.RemoveAt(0);

            if (path.Count == 0) {
                ResetPath();
                return;
            }

            return;   
        }

        dest.Normalize();
        
        gameObject.transform.Translate(dest * dSpeed);
        position = gameObject.transform.position;
        gridPosition = new Vector2Int( (int) (position.x + 0.5f), (int) (position.y + 0.5f) );

        entity.animationController.SetVelocity(dest);
    }
}
