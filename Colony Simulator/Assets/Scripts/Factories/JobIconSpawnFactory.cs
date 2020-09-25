using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;
using UnityEditor;

public static class JobIconSpawnFactory {
       
    public static Dictionary<string, JobIconScriptableObject> scriptableObjects;

    public static string searchFilter = "l:job";
    public static bool isInitialized = false;

    public static void Initialize() {
        string[] assetNames = AssetDatabase.FindAssets(searchFilter);
        scriptableObjects = new Dictionary<string, JobIconScriptableObject>();

        foreach (string name in assetNames) {
            var path = AssetDatabase.GUIDToAssetPath(name);
            var instance = AssetDatabase.LoadAssetAtPath<JobIconScriptableObject>(path);
            scriptableObjects.Add(instance.name, instance);
        }

        isInitialized = true;
    }

    public static GameObject GetNewJobIcon(string name, Vector2Int position) {
        if (!isInitialized)
            Initialize();
        
        if (!scriptableObjects.ContainsKey(name))
            return null;

        GameObject go = GameObject.Instantiate(scriptableObjects[name].prefab);
        go.transform.position = new Vector3(position.x, position.y, 0);
        return go;
    }
}