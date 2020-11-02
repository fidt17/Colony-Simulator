using System.Collections.Generic;

public interface IItemHolder {
    List<Item> Items { get; }
    void ItemIn(Item item);
    Item ItemOut(Item item);
}