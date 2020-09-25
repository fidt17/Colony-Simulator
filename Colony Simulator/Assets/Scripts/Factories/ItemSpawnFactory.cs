using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;
using UnityEditor;

public static class ItemSpawnFactory {
       
    public static Dictionary<string, Type> types;
    public static Dictionary<string, ItemScriptableObject> scriptableObjects;

    public static string searchFilter = "l:item";

    public static bool isInitialized = false;

    public static void Initialize() {
        List<Type> entityTypes = Assembly.GetAssembly( typeof(Item)).GetTypes()
                                                .Where(myType => myType.IsSubclassOf(typeof(Item))).ToList();
        types = new Dictionary<string, Type>();

        foreach (Type type in entityTypes) {
            Item tempItem = Activator.CreateInstance(type) as Item;
            types.Add(tempItem.Name, type);
        }
        
        string[] assetNames = AssetDatabase.FindAssets(searchFilter);
        scriptableObjects = new Dictionary<string, ItemScriptableObject>();
        foreach (string name in assetNames) {
            var path = AssetDatabase.GUIDToAssetPath(name);
            var instance = AssetDatabase.LoadAssetAtPath<ItemScriptableObject>(path);
            scriptableObjects.Add(instance.name, instance);
        }

        isInitialized = true;
    }

    public static Item GetNewItem(string typeName, string dataName, Vector2Int position) {
        if (!isInitialized)
            Initialize();
        
        if (!scriptableObjects.ContainsKey(dataName) || !types.ContainsKey(typeName))
            return null;

        GameObject gameObject = GameObject.Instantiate(scriptableObjects[dataName].prefab);
        Item Item = Activator.CreateInstance(types[typeName]) as Item;
        
        Item.SetData(scriptableObjects[dataName]);
        Item.SetGameObject(gameObject, position);

        return Item;
    }
}