using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionContent {

    private Dictionary<Type, object> content = new Dictionary<Type, object>();

    public void Add<T>(T value) {
        if (value == null) {
            return;
        }

        if (content.ContainsKey(typeof(T)) == false) {
            NewEntry<T>();
        }
        
        (content[typeof(T)] as List<T>).Add(value);
    }

    public void Remove<T>(T value) {
        (content[typeof(T)] as List<T>).Remove(value);
    }

    public List<T> Get<T>() => (content.ContainsKey(typeof(T))) ? (content[typeof(T)] as List<T>) : null;

    public void Clear() => content.Clear();

    private void NewEntry<T>() => content.Add(typeof(T), new List<T>());
}