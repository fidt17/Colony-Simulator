using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

public static class Factory {

    private static Dictionary<string, PrefabScriptableObject>       _data                 = new Dictionary<string, PrefabScriptableObject>();
    private static Dictionary<string, ConstructionScriptableObject> _constructionPlanData = new Dictionary<string, ConstructionScriptableObject>();
    private static Dictionary<Type, Transform>                      _objectsParents       = new Dictionary<Type, Transform>();
    private static bool                                             _isInitialized        = false;

    private static void Initialize() {
        foreach (string name in AssetDatabase.FindAssets("l:PrefabScriptableObject")) {
            var path = AssetDatabase.GUIDToAssetPath(name);
            var instance = AssetDatabase.LoadAssetAtPath<PrefabScriptableObject>(path);
            _data.Add(instance.dataName, instance);
        }
        
        foreach (string name in AssetDatabase.FindAssets("l:ConstructionPlan")) {
            var path = AssetDatabase.GUIDToAssetPath(name);
            var instance = AssetDatabase.LoadAssetAtPath<ConstructionScriptableObject>(path);
            _constructionPlanData.Add(instance.dataName, instance);
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

    public static T CreateData<T>(string dataName, Vector2Int position) where T : IData, new() {
        if (!_isInitialized) {
            Initialize();
        }

        T t = new T();
        t.SetData(_data[dataName], position);
        return t;
    }

    public static T Create<T>(string dataName, Vector2Int position) where T : IPrefab, new() {
        if (!_isInitialized) {
            Initialize();
        }

        T t = new T();
        t.SetData(_data[dataName], position);
        t.SetGameObject(GameObject.Instantiate(_data[dataName].prefab, GetParent<T>()));
        return t;
    }

    public static ConstructionPlan CreateConstructionPlan(string constructionName, Vector2Int position) {
        if (!_isInitialized) {
            Initialize();
        }
        
        ConstructionPlan plan = new ConstructionPlan();
        plan.SetData(_constructionPlanData[constructionName], position);
        plan.SetGameObject(GameObject.Instantiate(_constructionPlanData[constructionName].planPrefab, GetParent<ConstructionPlan>()));
        return plan;
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