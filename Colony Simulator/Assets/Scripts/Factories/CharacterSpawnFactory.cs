using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;
using UnityEditor;

public static class CharacterSpawnFactory
{   
    public static Dictionary<string, Type> types;
    public static Dictionary<string, CharacterScriptableObject> scriptableObjects;

    public static string searchFilter = "l:character";

    public static bool isInitialized = false;

    public static void Initialize() {

        //Loading classes
        List<Type> entityTypes = Assembly.GetAssembly( typeof(Character)).GetTypes()
                                                .Where(myType => myType.IsSubclassOf(typeof(Character))).ToList();

        types = new Dictionary<string, Type>();

        foreach (Type type in entityTypes) {
            
            Character tempCharacter = Activator.CreateInstance(type) as Character;
            types.Add(tempCharacter.Name, type);
        }
        
        //Loading scriptable objects
        string[] assetNames = AssetDatabase.FindAssets(searchFilter);
        scriptableObjects = new Dictionary<string, CharacterScriptableObject>();

        foreach (string SOName in assetNames) {

            var SOpath    = AssetDatabase.GUIDToAssetPath(SOName);
            var soInstance = AssetDatabase.LoadAssetAtPath<CharacterScriptableObject>(SOpath);
            scriptableObjects.Add(soInstance.name, soInstance);
        }

        if (scriptableObjects.Count != types.Count)
            Debug.LogWarning("Count of scriptableobject are not the same as count of types. Did you misspelled the name?");

        isInitialized = true;
    }

    public static Character GetNewCharacter(string name) {

        if (!isInitialized)
            Initialize();

        if (!scriptableObjects.ContainsKey(name) || !types.ContainsKey(name))
            return null;

        GameObject go = GameObject.Instantiate(scriptableObjects[name].prefab);
        Character character = Activator.CreateInstance(types[name], go) as Character;
        character.SetGameObject(go);

        return character;
    }

    public static CharacterScriptableObject GetScriptableObject(string name) {

        if (!isInitialized)
            Initialize();

        if (!scriptableObjects.ContainsKey(name))
            return null;

        return scriptableObjects[name];
    }
}
