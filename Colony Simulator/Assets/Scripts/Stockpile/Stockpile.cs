using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stockpile {

    public GameObject gameObject;

    private List<StockpilePart> _parts = new List<StockpilePart>();
    private Color _stockpileColor;

    public Stockpile() {
        GenerateColor();
    }

    public void AddPart(StockpilePart part) {
        _parts.Add(part);
        part.gameObject.transform.parent = gameObject.transform;
        part.spriteRenderer.color = _stockpileColor;
    }
    
    public void RemovePart(StockpilePart part) {
        _parts.Remove(part);
        if (_parts.Count == 0) {
            GameObject.Destroy(gameObject);
            StockpileManager.GetInstance().RemoveStockpile(this);
        }
    }

    public StockpilePart FindPlaceForItem(Item item) {
        foreach(StockpilePart part in _parts) {
            PathNode node = Utils.NodeAt(part.position);
            if (part.haulJob != null || part.HasItem) {
                continue;
            }

            if (node.Region != Utils.NodeAt(item.position).Region) {
                continue;
            }

            return part;            
        }
        return null;
    }

    private void GenerateColor() => _stockpileColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), 0.25f);    
}