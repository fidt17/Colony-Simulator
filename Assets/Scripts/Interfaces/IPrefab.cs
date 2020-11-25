using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPrefab {
    void SetData(PrefabScriptableObject data, Vector2Int position);
    void SetGameObject(GameObject obj);
    void Destroy();
}
