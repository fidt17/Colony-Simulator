using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPrefab {
    void SetData(PrefabScriptableObject data);
    void SetGameObject(GameObject obj, Vector2Int position);
    void Destroy();
}