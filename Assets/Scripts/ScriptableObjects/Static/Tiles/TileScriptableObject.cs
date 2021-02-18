using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Static/Tiles/TileScriptableObject", order = 1)]
public class TileScriptableObject : StaticScriptableObject {
    public TileType tileType;
    public Vector3  avgColor;
    public Color defaultColor = Color.white;

    private int _tileResolution = 16;

    public void CalculateAvgColor()
    {
        Vector3 color = new Vector3(0, 0, 0);

        int totalPixelCount = 0;
        for (int pixelX = 0; pixelX < _tileResolution; pixelX++) {
            for (int pixelY = 0; pixelY < _tileResolution; pixelY++) {
                Color pixelColor = prefabSprite.texture.GetPixel(pixelX + (int) prefabSprite.rect.x, pixelY + (int) prefabSprite.rect.y);
                if (pixelColor.a == 0)
                {
                    continue;
                }

                color.x += pixelColor.r;
                color.y += pixelColor.g;
                color.z += pixelColor.b;
                totalPixelCount++;
            }
        }

        color /= totalPixelCount;
        avgColor = color;
    }

    private void SumColors(int x, int y, int tileX, int tileY, ref Vector3 blendColor, int offset)
    {
        int totalTileCount = 0;
        for (int i = -offset; i < offset; i++)
        {
            for (int j = -offset; j < offset; j++)
            {
                int checkX = x + i;
                int checkY = y + j;

                int checkTileX = tileX;
                int checkTileY = tileY;
                
                if (checkX < 0)
                {
                    checkTileX--;
                }

                if (checkX >= _tileResolution)
                {
                    checkTileX++;
                }

                if (checkY < 0)
                {
                    checkTileY--;
                }

                if (checkY >= _tileResolution)
                {
                    checkTileY++;
                }

                Tile checkTile = Utils.TileAt(checkTileX, checkTileY);

                if (checkTile is null)
                {
                    continue;
                }

                if (checkTile.data.avgColor == Vector3.zero)
                {
                    checkTile.data.CalculateAvgColor();
                }
                Vector3 tileAvgColor = checkTile.data.avgColor;
                
                totalTileCount++;

                blendColor.x += tileAvgColor.x;
                blendColor.y += tileAvgColor.y;
                blendColor.z += tileAvgColor.z;
            }
        }

        blendColor /= totalTileCount;
    }
    
    public Color[,] GetColorsForTile(Tile tile)
    {
        Color[,] colorMatrix = new Color[_tileResolution, _tileResolution];
        MergeSprites(ref colorMatrix, tile.GetSprite());

        int tileX = tile.position.x;
        int tileY = tile.position.y;

        int width = 3;
        int offset = 3;

        for (int x = 0; x < _tileResolution; x++)
        {
            for (int y = 0; y < _tileResolution; y++)
            {
                if (x    >= width && x <= _tileResolution - width
                    && y >= width && y <= _tileResolution - width)
                {
                    continue;
                }
                
                Vector3 blendColor = new Vector3(0, 0, 0);
                SumColors(x, y, tileX, tileY, ref blendColor, offset);
                colorMatrix[x, y] = new Color(blendColor.x, blendColor.y, blendColor.z, 1);
            }
        }
        
        
        return colorMatrix;
    }

    private void MergeSprites(ref Color[,] colorMatrix, Sprite sprite)
    {
        for (int pixelX = 0; pixelX < _tileResolution; pixelX++) {
            for (int pixelY = 0; pixelY < _tileResolution; pixelY++) {
                Color pixelColor = sprite.texture.GetPixel(pixelX + (int) sprite.rect.x, pixelY + (int) sprite.rect.y);
                if (pixelColor.a == 0)
                {
                    continue;
                }
                colorMatrix[pixelX, pixelY] = pixelColor;
            }
        }
    }
}

public enum TileType {
    empty,
    sand,
    grass,
    water
}
