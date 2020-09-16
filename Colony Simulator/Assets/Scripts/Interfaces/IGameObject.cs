using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameObject {
    
    GameObject GameObject { get; }
    void SetGameObject(GameObject gameObject, Vector2Int position);
}
