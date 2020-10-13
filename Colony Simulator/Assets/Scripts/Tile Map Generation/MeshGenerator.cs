using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : Singleton<MeshGenerator> {

    private const int _chunkSizeX = 10;
    private const int _chunkSizeY = 10;
    private const float _tileSize = 1f;
    private const int _tileResolution = 16;

    [SerializeField] private Material _defaultSpriteMaterial = null;

    private GameObject[,] chunks;

    public void Initialize() {
        chunks = new GameObject[Utils.MapSize / _chunkSizeX, Utils.MapSize / _chunkSizeY];
        for (int x = 0; x < Utils.MapSize; x += _chunkSizeX) {
            for (int y = 0; y < Utils.MapSize; y += _chunkSizeY) {
                GenerateChunk(x, y);
            }
        }
    }

    public void UpdateChunkAt(int x, int y) {
        int chunkXIndex = x / _chunkSizeX;
        int chunkYIndex = y / _chunkSizeY;
        GameObject chunk = chunks[chunkXIndex, chunkYIndex];

        MeshFilter meshFilter = chunk.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = chunk.GetComponent<MeshRenderer>();

        float xOffset = (x / _chunkSizeX) * _chunkSizeX - 0.5f;
        float yOffset = (y / _chunkSizeY) * _chunkSizeY - 0.5f;

        meshFilter.mesh = GenerateMesh(xOffset, yOffset);
        meshRenderer.material = _defaultSpriteMaterial;
        meshRenderer.material.mainTexture = GenerateTexture((x / _chunkSizeX) * _chunkSizeX, (y / _chunkSizeY) * _chunkSizeY);
    }

    public void GenerateChunk(int x, int y) {
        GameObject chunk = new GameObject();
        chunk.name = "chunk";
        MeshFilter meshFilter = chunk.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = chunk.AddComponent<MeshRenderer>();
        //chunks[x / _chunkSizeX, y /_chunkSizeY] = chunk;

        float xOffset = (x / _chunkSizeX) * _chunkSizeX - 0.5f;
        float yOffset = (y / _chunkSizeY) * _chunkSizeY - 0.5f;

        meshFilter.mesh = GenerateMesh(xOffset, yOffset);
        meshRenderer.material = _defaultSpriteMaterial;
        meshRenderer.material.mainTexture = GenerateTexture((x / _chunkSizeX) * _chunkSizeX, (y / _chunkSizeY) * _chunkSizeY);
    }

    private Mesh GenerateMesh(float xOffset, float yOffset) {
        int tilesCount = _chunkSizeX * _chunkSizeY;

        int vSizeX = _chunkSizeX + 1;
        int vSizeY = _chunkSizeY + 1;

        int verticesCount = vSizeX * vSizeY;
        int trianglesCount = tilesCount * 2;

        //Mesh data
        Vector3[] vertices = new Vector3[verticesCount];
        Vector3[] normals = new Vector3[verticesCount];
        Vector2[] uv = new Vector2[verticesCount];

        int[] triangles = new int[trianglesCount * 3];

        //Generating vertices/normals/uv
        for (int y = 0; y < vSizeY; y++) {
            for (int x = 0; x < vSizeX; x++) {
                vertices[y * vSizeX + x] = new Vector3(x * _tileSize + xOffset, y * _tileSize + yOffset, 100);
                normals[y * vSizeX + x] = new Vector3(0, 0, 1);
                uv[y * vSizeX + x] = new Vector2( (float) x / _chunkSizeX, (float) y / _chunkSizeY);
            }
        }

        //Generating triangles
        for  (int y = 0; y < _chunkSizeY; y++) {
            for (int x = 0; x < _chunkSizeX; x++) {
                int tileIndex = y * _chunkSizeX + x;
                int triangleIndex = tileIndex * 6;

                triangles[triangleIndex + 0] = y * vSizeX + x +          0;
                triangles[triangleIndex + 1] = y * vSizeX + x + vSizeX + 0;
                triangles[triangleIndex + 2] = y * vSizeX + x + vSizeX + 1;

                triangles[triangleIndex + 3] = y * vSizeX + x +          0;
                triangles[triangleIndex + 4] = y * vSizeX + x + vSizeX + 1;
                triangles[triangleIndex + 5] = y * vSizeX + x +          1;
            }
        }

        //Assembling mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = triangles;

        return mesh;
    }

    private Texture2D GenerateTexture(int xOffset, int yOffset) {
        int textureWidth = _chunkSizeX * _tileResolution;
        int textureHeight = _chunkSizeY * _tileResolution;
        Texture2D texture = new Texture2D(textureWidth, textureHeight);

        for (int y = yOffset; y < yOffset + _chunkSizeY; y++) {
            for (int x = xOffset; x < xOffset + _chunkSizeX; x++) {
                Tile t = Utils.TileAt(x, y);
                Sprite sprite = t.GetSprite();
                Color color = t.GetColor();

                int pixelStartX = (x % _chunkSizeX) * _tileResolution;
                int pixelStartY = (y % _chunkSizeY) * _tileResolution;
                for (int pixelX = pixelStartX; pixelX < pixelStartX + _tileResolution; pixelX++) {
                    for (int pixelY = pixelStartY; pixelY < pixelStartY + _tileResolution; pixelY++) {
                        Color pixelColor = sprite.texture.GetPixel(pixelX % _tileResolution + (int) sprite.rect.x, pixelY % _tileResolution + (int) sprite.rect.y) * color;
                        texture.SetPixel(pixelX, pixelY, pixelColor);
                    }
                }
            }
        }
        texture.filterMode = FilterMode.Point;

        texture.Apply();

        return texture;
    }
}