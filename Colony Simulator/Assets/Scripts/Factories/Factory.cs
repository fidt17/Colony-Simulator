using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class Factory {

    private static Dictionary<string, PrefabScriptableObject> data;
    private static bool _isInitialized = false;

    private static void Initialize() {
        string searchFilter = "l:PrefabScriptableObject";
        string[] assetNames = AssetDatabase.FindAssets(searchFilter);
        data = new Dictionary<string, PrefabScriptableObject>();

        foreach (string name in assetNames) {
            var path = AssetDatabase.GUIDToAssetPath(name);
            var instance = AssetDatabase.LoadAssetAtPath<PrefabScriptableObject>(path);
            data.Add(instance.dataName, instance);
        }
        _isInitialized = true;
    }

    public static T Create<T>(string dataName, Vector2Int position) where T : IPrefab, new() {
        if (!_isInitialized) {
            Initialize();
        }

        T t = new T();
        t.SetData(data[dataName]);
        t.SetGameObject(GameObject.Instantiate(data[dataName].prefab), position);
        return t;
    }

    public static GameObject Create(string dataName, Vector2Int position) {
        if (!_isInitialized) {
            Initialize();
        }

        GameObject obj = GameObject.Instantiate(data[dataName].prefab);
        obj.transform.position = new Vector3(position.x, position.y, 0);
        return obj;
    }
}