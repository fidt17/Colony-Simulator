using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator
{   
    public int perlinSeed = 10;
    private GameObject tileParent;

    public WorldGenerator() {

        tileParent = GameObject.Find("World_Tiles");

        if (tileParent == null) {
            Debug.LogError("World_Tiles GameObject is missing!");
            return;
        }
    }

    public void GenerateEmptyWorld(Vector2Int dimensions, ref Tile[,] grid) {

        InitializeGrid(dimensions, ref grid);

        for (int x = 0; x < dimensions.x; x++) {
            for (int y = 0; y < dimensions.y; y++) {

                GameObject newTile = GameObject.Instantiate(PrefabStorage.Instance.protoTile);
                newTile.transform.position = new Vector3(x, y, -10);
                newTile.transform.parent = tileParent.transform;
                newTile.name = "Tile (" + x + ";" + y + ")";

                Tile t = new Tile(new Vector2Int(x, y), newTile);

                grid[x, y] = t;
            }
        }
    }

    public void GenerateTerrainWithPerlinNoise(Vector2Int dimensions, ref Tile[,] grid, float seaLevel = 0.33f) {

        GenerateEmptyWorld(dimensions, ref grid);

        PerlinNoise pn = new PerlinNoise(perlinSeed);

        int nOctaves = pn.CalculateMaxOctavesCount(dimensions.x);
        float fBias = 2f;

        float[,] perlinArray = new float[dimensions.x, dimensions.y];

        pn.Get2DPerlinNoise(dimensions.x, dimensions.y, nOctaves, fBias, ref perlinArray);

        //Min and max values that perlin array has
        float minP = 1;
        float maxP = 0;

        //calculating max and min values of perlin array
        for(int x = 0; x < dimensions.x; x++) {
            for(int y = 0; y < dimensions.y; y++) {
                
                float v = perlinArray[x,y];

                minP = Mathf.Min(v, minP);
                maxP = Mathf.Max(v, maxP);
			}
		}

        float elevationRange = maxP - minP;

        float sea = minP + elevationRange * seaLevel;
        float sand = sea + (maxP - sea) * 0.2f;

        SpriteRenderer waterSR = PrefabStorage.Instance.waterTile.GetComponent<SpriteRenderer>();
        SpriteRenderer greenBrickSR = PrefabStorage.Instance.greenBrickTile.GetComponent<SpriteRenderer>();
        SpriteRenderer redBrickSR = PrefabStorage.Instance.redBrickTile.GetComponent<SpriteRenderer>();

        for (int x = 0; x < dimensions.x; x++) {
            for(int y = 0; y < dimensions.y; y++) {

                float height = perlinArray[x,y];

                Tile tile = grid[x,y];

                if(height < sea) {
                    
                    tile.SetTileType(TileType.water, false, waterSR);
				} else if(height < sand) {
                    
                    tile.SetTileType(TileType.water, true, greenBrickSR);
				} else {

                    tile.SetTileType(TileType.water, true, redBrickSR);
				}
			}
		}
    }

    private void InitializeGrid(Vector2Int dimensions, ref Tile[,] grid) {

        DestroyGrid(ref grid);

        grid = new Tile[dimensions.x, dimensions.y];
    }

    private void DestroyGrid(ref Tile[,] grid) {

        if (grid == null)
            return;

        for (int x = 0; x < grid.GetLength(0); x++) {
            for (int y = 0; y < grid.GetLength(1); y++) {

                grid[x,y].DestroyGO();
            }
        }
    }
}
