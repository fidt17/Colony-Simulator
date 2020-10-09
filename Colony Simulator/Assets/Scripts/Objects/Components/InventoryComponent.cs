using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryComponent : MonoBehaviour {
    
    private List<Item> _inventory = new List<Item>();
    private Human _human;

    public void Initialize(Human human) {
        _human = human;
        _human.motionComponent.OnPositionChange += UpdateInventoryPositions;
    }

    public void PickItem(Item item) {
        _inventory.Add(item);
        item.RemoveFromTile();
    }

    public void DropItemAt(Item item, Vector2Int position) {
        if (_inventory.Contains(item)) {
            _inventory.Remove(item);
            item.SetPosition(position);
            item.PutOnTile();
        }
    }

    public void DropItem(Item item) => DropItemAt(item, _human.motionComponent.GridPosition);

    public void DropAll(){
        for (int i = _inventory.Count - 1; i >= 0; i--) {
            Item item = _inventory[i];
            DropItemAt(item, _human.motionComponent.GridPosition);
        }
    }

    private void UpdateInventoryPositions(Vector3 position) {
        foreach (Item item in _inventory) {
            item.gameObject.transform.position = position;
        }
    }
}   
