using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldGenerator {

    public static int perlinSeed = 12;
    
    private static GameObject _tileParent;
    private static GameSettingsScriptableObject _gameSettings;

    public static void GenerateWorld(GameSettingsScriptableObject gameSettings, ref Tile[,] grid) {

        _gameSettings = gameSettings;
        grid = new Tile[_gameSettings.worldWidth, _gameSettings.worldHeight];

        perlinSeed = gameSettings.seed;
        FindTileParent();
        GenerateTerrainWithPerlinNoise(ref grid);
        GenerateVegetation(ref grid);
        GenerateCharacters();
    }

    private static void FindTileParent() {

        _tileParent = GameObject.Find("World_Tiles");

        if (_tileParent == null) {
            Debug.LogError("World_Tiles GameObject is missing!");
            return;
        }
    }

    private static void GenerateTerrainWithPerlinNoise(ref Tile[,] grid, float seaLevel = 0.33f) {

        PerlinNoise pn = new PerlinNoise(perlinSeed);

        int nOctaves = pn.CalculateMaxOctavesCount(_gameSettings.worldWidth);
        float fBias = 2f;

        float[,] perlinArray = new float[_gameSettings.worldWidth, _gameSettings.worldHeight];

        pn.Get2DPerlinNoise(_gameSettings.worldWidth, _gameSettings.worldHeight, nOctaves, fBias, ref perlinArray);

        //Min and max values that perlin array has
        float minP = 1;
        float maxP = 0;

        //calculating max and min values of perlin array
        for(int x = 0; x < _gameSettings.worldWidth; x++) {
            for(int y = 0; y < _gameSettings.worldHeight; y++) {
                
                float v = perlinArray[x,y];

                minP = Mathf.Min(v, minP);
                maxP = Mathf.Max(v, maxP);
			}
		}

        float elevationRange = maxP - minP;

        float sea = minP + elevationRange * seaLevel;
        float sand = sea + (maxP - sea) * 0.2f;

        for (int x = 0; x < _gameSettings.worldWidth; x++) {
            for (int y = 0; y < _gameSettings.worldHeight; y++) {

                float height = perlinArray[x,y];

                string dataName = "tile";

                if (height < sea) {
                    dataName = "water_tile";
				} else if (height < sand) {
                    dataName = "sand_tile";
				} else {
                    dataName = "grass_tile";
				}

                Tile tile = StaticObjectSpawnFactory.GetNewStaticObject("tile", dataName, new Vector2Int(x, y)) as Tile;
                tile.GameObject.transform.parent = _tileParent.transform;
                grid[x, y] = tile;
			}
		}

        for (int x = 0; x < _gameSettings.worldWidth; x++)
            for (int y = 0; y < _gameSettings.worldHeight; y++)
                TileSpriteRenderer.Instance.UpdateTileBorders(grid[x,y]);
    }

    private static void GenerateVegetation(ref Tile[,] grid) {
        
        for (int x = 0; x < _gameSettings.worldWidth; x++) {
            for (int y = 0; y < _gameSettings.worldHeight; y++) {
                
                Tile tile = grid[x, y];

                if (tile.type != TileType.grass)
                    continue;

                float r = Random.Range(0f, 1f);

                if (r > 0.95f)
                    StaticObjectSpawnFactory.GetNewStaticObject("tree", "tall tree", new Vector2Int(x, y));
                else if (r > 0.6f)
                    StaticObjectSpawnFactory.GetNewStaticObject("grass", "grass", new Vector2Int(x, y));
			}
		}
    }

    private static void GenerateCharacters() {
        
        for (int i = 0; i < _gameSettings.humanCount; i++) {

            Human human = CharacterSpawnFactory.GetNewCharacter("human", "human", new Vector2Int(45, 35)) as Human;
            if (human != null)
                GameManager.Instance.characterManager.colonists.Add(human);
        }

        for (int i = 0; i < _gameSettings.rabbitCount; i++) {
            
            Tile t = null;

            while (t == null || !t.isTraversable)
                t = GameManager.Instance.world.GetTileAt(new Vector2Int((int) Random.Range(0, 50), (int) Random.Range(0, 50)));

            Rabbit rabbit = CharacterSpawnFactory.GetNewCharacter("rabbit", "rabbit", t.position) as Rabbit;
            if (rabbit != null)
                GameManager.Instance.characterManager.rabbits.Add(rabbit);
        }
    }
}