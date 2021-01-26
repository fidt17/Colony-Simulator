using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public static class WorldGenerator {

    public static int perlinSeed = 12;
    
    private static GameSettingsScriptableObject _gameSettings;

    public static void GenerateWorld(GameSettingsScriptableObject gameSettings, ref Tile[,] grid) {
        _gameSettings = gameSettings;
        grid = new Tile[Utils.MapSize, Utils.MapSize];

        if (gameSettings.testWorld) {
            GenerateTestTerrain(ref grid);
            MeshGenerator.GetInstance().Initialize();
            Pathfinder.Initialize(Utils.MapSize, Utils.MapSize, ref grid);
        }
        else {
            perlinSeed = gameSettings.seed;
            GenerateTerrainWithPerlinNoise(ref grid);
            MeshGenerator.GetInstance().Initialize();
            Pathfinder.Initialize(Utils.MapSize, Utils.MapSize, ref grid);
            if (gameSettings.vegetation == true) {
                GenerateVegetation(ref grid);
            }
            GenerateCharacters();
        }
    }

    private static void GenerateTestTerrain(ref Tile[,] grid) {
        for (int x = 0; x < Utils.MapSize; x++) {
            for (int y = 0; y < Utils.MapSize; y++) {
                grid[x, y] = Factory.CreateData<Tile>("grass tile", new Vector2Int(x, y));
            }
        }
    }
    
    private static void GenerateTerrainWithPerlinNoise(ref Tile[,] grid, float seaLevel = 0.33f) {
        PerlinNoise pn = new PerlinNoise(perlinSeed);
        int nOctaves = pn.CalculateMaxOctavesCount(Utils.MapSize);
        float fBias = 2f;
        float[,] perlinArray = new float[Utils.MapSize, Utils.MapSize];

        pn.Get2DPerlinNoise(Utils.MapSize, Utils.MapSize, nOctaves, fBias, ref perlinArray);

        //Min and max values that perlin array has
        float minP = 1;
        float maxP = 0;

        //calculating max and min values of perlin array
        for(int x = 0; x < Utils.MapSize; x++) {
            for(int y = 0; y < Utils.MapSize; y++) {
                float v = perlinArray[x,y];
                minP = Mathf.Min(v, minP);
                maxP = Mathf.Max(v, maxP);
			}
		}

        float elevationRange = maxP - minP;
        float sea = minP + elevationRange * seaLevel;
        float sand = sea + (maxP - sea) * 0.2f;

        for (int x = 0; x < Utils.MapSize; x++) {
            for (int y = 0; y < Utils.MapSize; y++) {

                float height = perlinArray[x,y];
                string dataName = "tile";

                if (height < sea) {
                    dataName = "water tile";
				} else if (height < sand) {
                    dataName = "sand tile";
				} else {
                    dataName = "grass tile";
				}

                grid[x, y] = Factory.CreateData<Tile>(dataName, new Vector2Int(x, y));
			}
		}
    }

    private static void GenerateVegetation(ref Tile[,] grid) {
        for (int x = 0; x < Utils.MapSize; x++) {
            for (int y = 0; y < Utils.MapSize; y++) {
                
                Tile tile = grid[x, y];
                if (tile.type != TileType.grass) {
                    continue;
                }

                float r = Random.Range(0f, 1f);

                if (r > 0.95f) {
                    Factory.Create<Tree>("tall tree", new Vector2Int(x, y));
                } else if (r > 0.6f) {
                    Factory.Create<Grass>("grass", new Vector2Int(x, y));
                }
			}
		}
    }

    private static void GenerateCharacters() {
        
        for (int i = 0; i < _gameSettings.humanCount; i++) {
            Tile t = null;
            while (t == null || !t.IsTraversable) {
                t = Utils.RandomTile();
            }
            Human human = Factory.Create<Human>("human", t.position);
        }

        for (int i = 0; i < _gameSettings.rabbitCount; i++) {
            Tile t = null;
            while (t == null || !t.IsTraversable) {
                t = Utils.RandomTile();
            }
            Rabbit rabbit = Factory.Create<Rabbit>("rabbit", t.position);
        }
    }
}