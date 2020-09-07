using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MotionComponent : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private List<PathNode> path;

    //Control 8-way animation on a characted (at least for now it is the only use).
    public delegate void OnVelocityChange(Vector2 newVelocty);
    public event OnVelocityChange VelocityHandler;

    #region InProgress

    public void A() {

        StartCoroutine(Wander());
    }

    public IEnumerator Wander() {

        while (true) {

            if (path == null) {

                Tile t = null;

                while(t == null) {

                    t = GameManager.Instance.world.GetTileAt(new Vector2Int((int) Random.Range(0, 50), (int) Random.Range(0, 50)));
                }

                SetDestination(t);
            }

            yield return new WaitForSeconds(2f);
        }
    }

    #endregion

    private void Update() {

        MoveTowardsDestination();
    }

    public void SetPosition(Vector2 position) {

        gameObject.transform.position = position;
    }

    public void SetSpeed(float newSpeed) {

        speed = newSpeed;
    }

    #region Pathfinding

    public void SetDestination(Tile destinationTile) {

        if (destinationTile == null || !destinationTile.isTraversable)
            return;

        ResetPath();
        float startTime = Time.realtimeSinceStartup;
        path = GameManager.Instance.pathfinder.GetPath(GetGridPosition(), destinationTile.position);
    }

    public void ResetPath() {

        path = null;
        VelocityHandler?.Invoke(new Vector2(0, 0));
    }

    private void MoveTowardsDestination() {

        if (path == null)
            return;
        
        if (path.Count == 0) {
            ResetPath();
            return;
        }

        Vector2 nextNode = path[0].position;
        Vector2 destination = nextNode - GetWorldPosition();

        float deltaSpeed = speed * Time.deltaTime;

        //If character is close enough to the destination tile
        if (destination.sqrMagnitude <= Mathf.Pow(deltaSpeed, 2)) {

            SetPosition(nextNode);
            path.RemoveAt(0);

            if (path.Count == 0)
                ResetPath();

            return;   
        }

        destination.Normalize();

        gameObject.transform.Translate(destination * deltaSpeed);
        VelocityHandler?.Invoke(destination);
    }

    #endregion

    private Vector2 GetWorldPosition() {

        return (Vector2) gameObject.transform.position;
    }

    private Vector2Int GetGridPosition() {

        Vector3 worldPosition = GetWorldPosition();

        return new Vector2Int( (int) (worldPosition.x + 0.5f), (int) (worldPosition.y + 0.5f) );
    }
}
