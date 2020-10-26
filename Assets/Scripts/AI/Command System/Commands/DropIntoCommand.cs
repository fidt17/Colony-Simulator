using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropIntoCommand : Command {
    
    private Item _item;
    private InventoryComponent _picker;
    private Vector2Int _destination;
    private IItemHolder _itemHolder;

    public DropIntoCommand(Item item, InventoryComponent picker, IItemHolder itemHolder) {
        _item = item;
        _picker = picker;
        _destination = (itemHolder as StaticObject).position;
        _itemHolder = itemHolder;
        _item.OnDestroyed += OnObjectDestroyedOutside;
    }

    public override void Execute() {
        _item.OnDestroyed -= OnObjectDestroyedOutside;
        _picker.PlaceItemInto(_item, _itemHolder);
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