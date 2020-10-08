using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickCommand : Command {
    
    private Item _item;
    private InventoryComponent _picker;

    public PickCommand(Item item, InventoryComponent picker) {
        _item = item;
        _picker = picker;
        _item.OnDestroyed += OnObjectDestroyedOutside;
    }

    public override void Execute() {
        _picker.PickItem(_item);
        Finish(true);   
    }

    public override void Abort() {
        if (_item != null) {  
            _item.OnDestroyed -= OnObjectDestroyedOutside;
        }
    }

    private void OnObjectDestroyedOutside(object sender, EventArgs e) {
        _item.OnDestroyed -= OnObjectDestroyedOutside;
        _item = null;
        Finish(false);
    }
}