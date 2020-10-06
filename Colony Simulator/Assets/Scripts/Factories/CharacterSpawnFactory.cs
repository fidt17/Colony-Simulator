using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;
using UnityEditor;

public static class CharacterSpawnFactory {
       
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

        isInitialized = true;
    }

    public static Character GetNewCharacter(string typeName, string dataName, Vector2Int position) {
        if (!isInitialized)
            Initialize();

        if (!scriptableObjects.ContainsKey(dataName) || !types.ContainsKey(typeName))
            return null;

        GameObject gameObject = GameObject.Instantiate(scriptableObjects[dataName].prefab);
        Character character = Activator.CreateInstance(types[typeName], gameObject) as Character;

        character.SetData(scriptableObjects[dataName]);
        character.SetGameObject(gameObject, position);

        return character;
    }
}