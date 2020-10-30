using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileContent {

    public bool HasItem => Item != null;

    public StockpilePart    StockpilePart     { get; private set; }
    public ConstructionPlan ConstructionPlan  { get; private set; }
    public StaticObject     StaticObject      { get; private set; }
    public Item             Item              { get; private set; }

    public List<Character> Characters { get; } = new List<Character>();
    public List<StaticJob> StaticJobs { get; } = new List<StaticJob>();
    
    private readonly Tile _tile;

    public TileContent(Tile tile) {
        _tile = tile;
    }

    public void SetStockpilePart(StockpilePart value) => StockpilePart = value;
    public void RemoveStockpilePart()                 => StockpilePart = null;

    public void SetConstructionPlan(ConstructionPlan value) => ConstructionPlan = value;
    public void RemoveConstructionPlan()                    => ConstructionPlan = null;
    
    public void AddStaticJob(StaticJob job)    => StaticJobs.Add(job);
    public void RemoveStaticJob(StaticJob job) => StaticJobs.Remove(job);
    
    public void PutStaticObjectOnTile(StaticObject staticObject, bool isTraversable) {
        StaticObject?.Destroy();
        StaticObject = staticObject;
        _tile.SetTraversability(isTraversable);
    }

    public void RemoveStaticObjectFromTile() {
        StaticObject = null;
        _tile.SetTraversability(true);
    }

    public void PutItemOnTile(Item item) {
        if (Item != null) {
            Debug.LogError("Item was destroyed because another item was placed upon it." + " pos: " + this.Item.Position);
            Item.Destroy();
        }
        Item = item;
    }

    public void RemoveItemFromTile() => Item = null;

    public void AddCharacter(Character character)    => Characters.Add(character);
    public void RemoveCharacter(Character character) => Characters.Remove(character);
}