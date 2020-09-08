using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;
using UnityEditor;

public static class StaticObjectSpawnFactory
{   
    public static Dictionary<string, Type> types;
    public static Dictionary<string, StaticScriptableObject> scriptableObjects;

    public static string searchFilter = "l:static";

    public static bool isInitialized = false;

    public static void Initialize() {

        //Loading classes
        List<Type> entityTypes = Assembly.GetAssembly( typeof(StaticObject)).GetTypes()
                                                .Where(myType => myType.IsSubclassOf(typeof(StaticObject))).ToList();

        types = new Dictionary<string, Type>();

        foreach (Type type in entityTypes) {
            
            StaticObject tempStaticObject = Activator.CreateInstance(type) as StaticObject;
            types.Add(tempStaticObject.Name, type);
        }
        
        //Loading scriptable objects
        string[] assetNames = AssetDatabase.FindAssets(searchFilter);
        scriptableObjects = new Dictionary<string, StaticScriptableObject>();


        foreach (string SOName in assetNames) {

            var SOpath    = AssetDatabase.GUIDToAssetPath(SOName);
            var soInstance = AssetDatabase.LoadAssetAtPath<StaticScriptableObject>(SOpath);
            scriptableObjects.Add(soInstance.name, soInstance);
        }

        isInitialized = true;
    }

    public static StaticObject GetNewStaticObject(string typeName, string dataName, Vector2Int position) {

        if (!isInitialized)
            Initialize();
        
        if (!scriptableObjects.ContainsKey(dataName) || !types.ContainsKey(typeName))
            return null;

        GameObject gameObject = GameObject.Instantiate(scriptableObjects[dataName].prefab);
        StaticObject StaticObject = Activator.CreateInstance(types[typeName]) as StaticObject;
        
        StaticObject.SetData(scriptableObjects[dataName]);
        StaticObject.SetGameObject(gameObject, position);

        return StaticObject;
    }
}
