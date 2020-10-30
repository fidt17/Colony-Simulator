using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCommand : Command {
    
    private Item _item;
    private InventoryComponent _picker;
    private Vector2Int _destination;

    public DropCommand(Item item, InventoryComponent picker, Vector2Int destination) {
        _item = item;
        _picker = picker;
        _destination = destination;
        _item.OnDestroyed += OnObjectDestroyedOutside;
    }

    public override void Execute() {
        _item.OnDestroyed -= OnObjectDestroyedOutside;
        _picker.DropItemAt(_item, _destination);
        Finish(true);   
    }

    public override void Abort() {
        if (_item != null) {  
            _item.OnDestroyed -= OnObjectDestroyedOutside;
        }
        _picker.DropItem(_item);
    }

    private void OnObjectDestroyedOutside(object source, EventArgs e) {
        (source as Item).OnDestroyed -= OnObjectDestroyedOutside;
        _item = null;
        Finish(false);
    }
}