using System.Collections.Generic;

public interface IItemHolder {
    List<Item> items { get; }
    void ItemIn(Item item);
    Item ItemOut(Item item);
}