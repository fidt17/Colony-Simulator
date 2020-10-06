using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component {

    private static T _instance;

    public static T GetInstance() {
        if (_instance is null) {
            _instance = FindObjectOfType<T>();
            if (_instance is null) {
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                _instance = obj.AddComponent<T>();
            }
        }

        return _instance;
    }

    protected virtual void Awake() {
        if (_instance is null) {
            _instance = this as T;
        } else if (_instance != this as T) {
            Destroy(gameObject);
        }
    }
}