using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldGenerator
{   
    public static int perlinSeed = 12;
    private static GameObject tileParent;

    private static void FindTileParent() {

        tileParent = GameObject.Find("World_Tiles");

        if (tileParent == null) {
            Debug.LogError("World_Tiles GameObject is missing!");
            return;
        }
    }

    private static void GenerateEmptyWorld(Vector2Int dimensions, ref Tile[,] grid) {

        grid = new Tile[dimensions.x, dimensions.y];

        for (int x = 0; x < dimensions.x; x++) {
            for (int y = 0; y < dimensions.y; y++) {

                Tile tile = StaticObjectSpawnFactory.GetNewStaticObject("tile","tile",new Vector2Int(x, y)) as Tile;
                tile.GameObject.transform.parent = tileParent.transform;

                grid[x, y] = tile;
            }
        }
    }

    public static void GenerateWorld(Vector2Int dimensions, ref Tile[,] grid) {

        grid = new Tile[dimensions.x, dimensions.y];

        FindTileParent();
        GenerateTerrainWithPerlinNoise(dimensions, ref grid);
        GenerateVegetation(dimensions, ref grid);
        GenerateCharacters();
    }

    private static void GenerateTerrainWithPerlinNoise(Vector2Int dimensions, ref Tile[,] grid, float seaLevel = 0.33f) {

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

        for (int x = 0; x < dimensions.x; x++) {
            for(int y = 0; y < dimensions.y; y++) {

                float height = perlinArray[x,y];

                string dataName = "tile";

                if(height < sea) {
                    dataName = "water_tile";
				} else if(height < sand) {
                    dataName = "sand_tile";
				} else {
                    dataName = "grass_tile";
				}

                Tile tile = StaticObjectSpawnFactory.GetNewStaticObject("tile", dataName, new Vector2Int(x, y)) as Tile;
                grid[x, y] = tile;
			}
		}

        for (int x = 0; x < dimensions.x; x++)
            for(int y = 0; y < dimensions.y; y++)
                TileSpriteRenderer.Instance.UpdateTileBorders(grid[x,y]);
    }

    private static void GenerateVegetation(Vector2Int dimensions, ref Tile[,] grid) {
        
        for(int x = 0; x < dimensions.x; x++) {
            for(int y = 0; y < dimensions.y; y++) {
                
                Tile tile = grid[x, y];

                if (tile.type != TileType.grass)
                    continue;

                if (Random.Range(0f, 1f) > 0.8f) {
                    
                    Grass grass = StaticObjectSpawnFactory.GetNewStaticObject("grass", "grass", new Vector2Int(x, y)) as Grass;
                    GameManager.Instance.natureManager.grass.Add(grass);
                }
			}
		}
    }

    private static void GenerateCharacters() {
        
        Human human = CharacterSpawnFactory.GetNewCharacter("human", "human", new Vector2Int(45, 35)) as Human;
        if (human != null)
            GameManager.Instance.characterManager.colonists.Add(human);

        int rabbitCount = 1;

        for (int i = 0; i < rabbitCount; i++) {
            
            Rabbit rabbit = CharacterSpawnFactory.GetNewCharacter("rabbit", "rabbit", new Vector2Int(40, 35)) as Rabbit;
            if (rabbit != null)
                GameManager.Instance.characterManager.rabbits.Add(rabbit);
        }
    }
}
