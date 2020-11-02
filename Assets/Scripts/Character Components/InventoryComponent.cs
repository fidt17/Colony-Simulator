using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryComponent : CharacterComponent {

    public bool HasItem(Item item) => _inventory.Contains(item);
    
    private List<Item> _inventory = new List<Item>();
    private Human _human;

    public InventoryComponent(Human human) {
        _human = human;
        _human.MotionComponent.OnPositionChange += UpdateInventoryPositions;
    }

    public void PickItem(Item item) {
        _inventory.Add(item);
        item.RemoveFromTile();
    }

    public void DropItemAt(Item item, Vector2Int position) {
        if (_inventory.Contains(item)) {
            _inventory.Remove(item);

            Tile t = Utils.TileAt(position.x, position.y);
            if (t.isTraversable == false) {
                position = _human.MotionComponent.GridPosition;
            }
            
            item.SetPosition(position);
            item.PutOnTile();
        }
    }

    public void PlaceItemInto(Item item, IItemHolder itemHolder) {
        if (_inventory.Contains(item)) {
            _inventory.Remove(item);
            itemHolder.ItemIn(item);
        }
    }

    public void DropItem(Item item) => DropItemAt(item, _human.MotionComponent.GridPosition);

    public void DropAll(){
        for (int i = _inventory.Count - 1; i >= 0; i--) {
            Item item = _inventory[i];
            DropItemAt(item, _human.MotionComponent.GridPosition);
        }
    }

    private void UpdateInventoryPositions(Vector3 position) {
        foreach (Item item in _inventory) {
            item.gameObject.transform.position = position;
        }
    }

    public override void DisableComponent() {
        base.DisableComponent();

        DropAll();
        _human = null;
    }

    #region Testing

    public override bool CheckInitialization() {

        if (_human is null) {
            return false;
        }

        return true;
    }

    #endregion
}   
