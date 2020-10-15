using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {

    empty,
    sand,
    grass,
    water
}

public class Tile : IData {

    private class TileHighlight {

        public Color Color => _color;

        private Color _color = Color.white;
        private Color _defaultColor = Color.white;
        private Vector2Int _position;

        public TileHighlight(Vector2Int position, Color defaultColor) {
            _position = position;
            _defaultColor = defaultColor;
        }

        public void SetColor(Color color) {
            _color = color;
            MeshGenerator.GetInstance().UpdateChunkAt(_position.x, _position.y);
        }

        public void ResetColor() {
            _color = _defaultColor;
            MeshGenerator.GetInstance().UpdateChunkAt(_position.x, _position.y);
        }
    }

    public TileType type { get; set; }
    public TileContent content { get; private set; }

    public TileScriptableObject data { get; protected set; }

    public Vector2Int position   { get; protected set; }
    public bool isTraversable    { get; protected set; }

    private TileHighlight _tileHighlight;

    public void SetData(PrefabScriptableObject data, Vector2Int position) {
        this.data = data as TileScriptableObject;
        isTraversable = this.data.isTraversable;
        type = this.data.tileType;
        this.position = position;

        content = new TileContent(this);
        _tileHighlight = new TileHighlight(this.position, this.data.defaultColor);
    }

    public void SetTraversability(bool isTraversable) {
        if (this.isTraversable != isTraversable) {
            this.isTraversable = isTraversable;

            PathNode node = Utils.NodeAt(position);
            if (node != null) {
                node.isTraversable = isTraversable;
            }

            Pathfinder.UpdateSystemAt(position.x, position.y);
        }
    }

    public Sprite GetSprite() => data.prefabSprite;
    public Color GetColor() => _tileHighlight.Color;
    public void SetColor(Color color) => _tileHighlight.SetColor(color);
    public void ResetColor() => _tileHighlight.ResetColor();
}