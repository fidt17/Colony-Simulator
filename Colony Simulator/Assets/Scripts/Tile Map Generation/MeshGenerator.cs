using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : Singleton<MeshGenerator> {

    private enum RenderQueue {
        tile = 0,
        overlapArea = 1
    }

    private const float _chunkUpdateCooldownInSeconds = 0.1f;
    private float _frameLock = 0.2f;//To prevent game from freezing for too long while updating chunks.        

    private const int _chunkSizeX = 5;
    private const int _chunkSizeY = 5;
    private const float _tileSize = 1f;
    private const int _tileResolution = 16;

    [SerializeField] private Material _defaultSpriteMaterial = null;
    [SerializeField] private Texture2D _overlapAreaTexture = null;

    private GameObject[,] chunks;
    private Queue<Vector2Int> _chunksToUpdate = new Queue<Vector2Int>();
    private Stack<Vector2Int> _tempStackOfChunksToUpdate = new Stack<Vector2Int>();

    public void Initialize() {
        chunks = new GameObject[Utils.MapSize / _chunkSizeX, Utils.MapSize / _chunkSizeY];
        for (int x = 0; x < Utils.MapSize; x += _chunkSizeX) {
            for (int y = 0; y < Utils.MapSize; y += _chunkSizeY) {
                GenerateChunk(x, y);
            }
        }
    }

    #region Updating chunks

    public void UpdateChunkAt(int x, int y) {
        Vector2Int chunkCoordinates = new Vector2Int(x / _chunkSizeX, y / _chunkSizeY);
        if (_tempStackOfChunksToUpdate.Contains(chunkCoordinates) == false) {
            _tempStackOfChunksToUpdate.Push(chunkCoordinates);
            StartCoroutine(AddToStack());
        }
    }

    private bool _isAddingToStack = false;
    private IEnumerator AddToStack() {
        if (_isAddingToStack) {
            yield break;
        }
        _isAddingToStack = true;
        yield return null;
        while (_tempStackOfChunksToUpdate.Count != 0) {
            Vector2Int chunkToUpdate = _tempStackOfChunksToUpdate.Pop();
            if (_chunksToUpdate.Contains(chunkToUpdate) == false) {
                _chunksToUpdate.Enqueue(chunkToUpdate);
            }
        }
        StartCoroutine(UpdateChunks());
        _isAddingToStack = false;
    }

    private bool _isUpdatingChunks = false;
    private IEnumerator UpdateChunks() {
        if (_isUpdatingChunks) {
            yield break;
        }
        _isUpdatingChunks = true;
        float sTime = Time.realtimeSinceStartup;
        while (_chunksToUpdate.Count != 0) {
            float leftFrames = _frameLock;
            while (leftFrames > 0 && _chunksToUpdate.Count != 0) {
                float startTime = Time.realtimeSinceStartup;
                Vector2Int chunkCoordinates = _chunksToUpdate.Dequeue();
                GameObject chunk = chunks[chunkCoordinates.x, chunkCoordinates.y];
                chunk.GetComponent<MeshRenderer>().material.mainTexture = GenerateTexture(chunkCoordinates.x * _chunkSizeX, chunkCoordinates.y * _chunkSizeY);
                leftFrames -= (Time.realtimeSinceStartup - startTime) / 1.0f;
            }
            for (int i = 0; i < 60 - _frameLock; i++) {
                yield return null;
            }
        }
        _isUpdatingChunks = false;
    }

    #endregion

    private void GenerateChunk(int x, int y) {
        GameObject chunk = new GameObject();
        chunk.name = "chunk";
        MeshFilter meshFilter = chunk.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = chunk.AddComponent<MeshRenderer>();
        chunks[x / _chunkSizeX, y /_chunkSizeY] = chunk;

        float xOffset = (x / _chunkSizeX) * _chunkSizeX - 0.5f;
        float yOffset = (y / _chunkSizeY) * _chunkSizeY - 0.5f;

        meshFilter.mesh = GenerateMesh(xOffset, yOffset, _chunkSizeX, _chunkSizeY);
        meshRenderer.material = _defaultSpriteMaterial;
        meshRenderer.material.mainTexture = GenerateTexture((x / _chunkSizeX) * _chunkSizeX, (y / _chunkSizeY) * _chunkSizeY);
    }

    private Mesh GenerateMesh(float xOffset, float yOffset, int width, int height) {
        int tilesCount = width * height;

        int vSizeX = width + 1;
        int vSizeY = height + 1;

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
                uv[y * vSizeX + x] = new Vector2( (float) x / width, (float) y / height);
            }
        }

        //Generating triangles
        for  (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                int tileIndex = y * width + x;
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

    private Mesh GenerateMeshOverNodes(List<PathNode> nodes) {

        int verticesCount = nodes.Count * 4;
        int trianglesCount = nodes.Count * 2;

        //Mesh data
        Vector3[] vertices = new Vector3[verticesCount];
        Vector3[] normals = new Vector3[verticesCount];
        Vector2[] uv = new Vector2[verticesCount];

        int[] triangles = new int[trianglesCount * 3];

        int verticesIndex = 0;//4
        int triangleIndex = 0;//6
        foreach (PathNode node in nodes) {
            vertices[verticesIndex + 0] = new Vector3(node.x - 0.5f, node.y + 0.5f, 90);
            vertices[verticesIndex + 1] = new Vector3(node.x + 0.5f, node.y + 0.5f, 90);
            vertices[verticesIndex + 2] = new Vector3(node.x - 0.5f, node.y - 0.5f, 90);
            vertices[verticesIndex + 3] = new Vector3(node.x + 0.5f, node.y - 0.5f, 90);
            
            normals[verticesIndex + 0] = new Vector3(0, 0, 1);
            normals[verticesIndex + 1] = new Vector3(0, 0, 1);
            normals[verticesIndex + 2] = new Vector3(0, 0, 1);
            normals[verticesIndex + 3] = new Vector3(0, 0, 1);

            uv[verticesIndex + 0] = new Vector2(0, 1);
            uv[verticesIndex + 1] = new Vector2(1, 1);
            uv[verticesIndex + 2] = new Vector2(0, 0);
            uv[verticesIndex + 3] = new Vector2(1, 0);

            triangles[triangleIndex + 0] = verticesIndex + 0;
            triangles[triangleIndex + 1] = verticesIndex + 1;
            triangles[triangleIndex + 2] = verticesIndex + 3;

            triangles[triangleIndex + 3] = verticesIndex + 2;
            triangles[triangleIndex + 4] = verticesIndex + 0;
            triangles[triangleIndex + 5] = verticesIndex + 3;

            verticesIndex += 4;
            triangleIndex += 6;
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
        Texture2D texture = new Texture2D(_chunkSizeX * _tileResolution, _chunkSizeY * _tileResolution);
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

    private GameObject GenerateOverlapArea(int x, int y, int width, int height, Color color) {
        GameObject area = new GameObject();
        area.name = "AREA";
        MeshFilter meshFilter = area.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = area.AddComponent<MeshRenderer>();

        float offset = -0.5f;

        meshFilter.mesh = GenerateMesh(offset, offset, width, height);
        meshRenderer.material = _defaultSpriteMaterial;
        meshRenderer.material.renderQueue = (int) RenderQueue.overlapArea;
        meshRenderer.material.mainTexture = _overlapAreaTexture;

        return area;
    }

    //Due to the fact that Unit has a limit of max 65535 verices per mesh, I am spliting one mesh into multiple.
    public List<GameObject> GenerateOverlapAreaOverNodes(List<PathNode> nodes, Color color) {
        List<GameObject> areas = new List<GameObject>();
        while (nodes.Count != 0) {
            List<PathNode> extra = new List<PathNode>();
            if (nodes.Count * 4 > 65535) {
                for (int i = nodes.Count - 1; i >= 0 && extra.Count * 4 < 65535 - 4; i--) {
                    extra.Add(nodes[i]);
                    nodes.RemoveAt(i);
                }
            } else {
                extra = new List<PathNode>(nodes);
                nodes.Clear();
            }

            GameObject area = new GameObject();
            areas.Add(area);
            area.name = "Area";
            MeshFilter meshFilter = meshFilter = area.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = area.AddComponent<MeshRenderer>();

            meshFilter.mesh = GenerateMeshOverNodes(extra);
            meshRenderer.material = _defaultSpriteMaterial;
            meshRenderer.material.renderQueue = (int) RenderQueue.overlapArea;
            meshRenderer.material.mainTexture = _overlapAreaTexture;
            meshRenderer.material.color = color;
        }
        return areas;
    }
}