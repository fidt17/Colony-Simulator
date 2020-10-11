using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

public static class Factory {

    private static Dictionary<string, PrefabScriptableObject> _data;
    private static Dictionary<Type, Transform> _objectsParents = new Dictionary<Type, Transform>();
    private static bool _isInitialized = false;

    private static void Initialize() {
        string searchFilter = "l:PrefabScriptableObject";
        string[] assetNames = AssetDatabase.FindAssets(searchFilter);
        _data = new Dictionary<string, PrefabScriptableObject>();

        foreach (string name in assetNames) {
            var path = AssetDatabase.GUIDToAssetPath(name);
            var instance = AssetDatabase.LoadAssetAtPath<PrefabScriptableObject>(path);
            _data.Add(instance.dataName, instance);
        }
        _isInitialized = true;
    }

    private static Transform GetParent<T>() {
        if (_objectsParents.ContainsKey(typeof(T)) == false) {
            GameObject parent = new GameObject();
            parent.name = typeof(T).ToString();
            _objectsParents.Add(typeof(T), parent.transform);
            return parent.transform;
        }
        return _objectsParents[typeof(T)];
    }

    public static T Create<T>(string dataName, Vector2Int position) where T : IPrefab, new() {
        if (!_isInitialized) {
            Initialize();
        }

        T t = new T();
        t.SetData(_data[dataName]);
        
        GameObject obj = GameObject.Instantiate(_data[dataName].prefab);
        obj.transform.SetParent(GetParent<T>());
        t.SetGameObject(obj, position);
        return t;
    }

    public static GameObject Create(string dataName, Vector2Int position) {
        if (!_isInitialized) {
            Initialize();
        }

        GameObject obj = GameObject.Instantiate(_data[dataName].prefab);
        obj.transform.position = new Vector3(position.x, position.y, 0);
        return obj;
    }
}