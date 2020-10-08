using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stockpile {

    public GameObject gameObject;

    private List<StockpilePart> _parts = new List<StockpilePart>();
    private Color _stockpileColor;

    public Stockpile() {
        StockpileManager.AddStockpile(this);
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
            GameObject.Destroy(gameObject, 1);
            StockpileManager.RemoveStockpile(this);
        }
    } 

    private void GenerateColor() => _stockpileColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), 0.25f);    
}