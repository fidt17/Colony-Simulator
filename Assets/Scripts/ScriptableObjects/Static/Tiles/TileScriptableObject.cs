using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Static/Tiles/TileScriptableObject", order = 1)]
public class TileScriptableObject : StaticScriptableObject {
    public TileType tileType;
    public Color defaultColor = Color.white;

    [Header("Central sand tile")]
    public Sprite sand_central = default;

    [Header("Water Borders")]
    public Sprite water_border_N = default;
    public Sprite water_border_E = default;
    public Sprite water_border_S = default;
    public Sprite water_border_W = default;

    [Header("Grass Borders")]
    public Sprite grass_border_corner = default;
    public Sprite grass_border_Lcorner = default;
    public Sprite grass_border_side    = default;
    public Sprite grass_border_pi      = default;
    public Sprite grass_border_tunnel  = default;
    public Sprite grass_border_all     = default;
    
    
    #region Related to tile sprite correction
    
    public Color[,] GetColorsForTile(Tile tile)
    {
        int _tileResolution = 16;
        Color[,] colorMatrix = new Color[_tileResolution, _tileResolution];
        MergeSprites(ref colorMatrix, tile.GetSprite());
        
        
        TileType type = tile.type;
        if (type != TileType.sand)
        {
            return colorMatrix;
        }
        
        TileType[,] borderMatrix = new TileType[3, 3];
        FindBorders(tile, ref borderMatrix);

        Color[,] mainSprite = GrassBorderCheck(borderMatrix);
        for (int pixelX = 0; pixelX < _tileResolution; pixelX++) {
            for (int pixelY = 0; pixelY < _tileResolution; pixelY++) {
                colorMatrix[pixelX, pixelY] = mainSprite[pixelX, pixelY];
            }
        }
        
        Sprite northBorder = null;
        Sprite eastBorder = null;
        Sprite southBorder = null;
        Sprite westBorder = null;
        
        WaterBorderCheck(ref borderMatrix, ref northBorder, ref eastBorder, ref southBorder, ref westBorder);
        
        if (northBorder != null)
        {
            MergeSprites(ref colorMatrix, northBorder);
        }
        
        if (eastBorder != null)
        {
            MergeSprites(ref colorMatrix, eastBorder);
        }
        
        if (southBorder != null)
        {
            MergeSprites(ref colorMatrix, southBorder);
        }
        
        if (westBorder != null)
        {
            MergeSprites(ref colorMatrix, westBorder);
        }
        
        return colorMatrix;
    }

    private void MergeSprites(ref Color[,] colorMatrix, Sprite sprite)
    {
        int _tileResolution = 16;
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
    
    private void FindBorders(Tile tile, ref TileType[,] borderMatrix)
    {
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                Vector2Int checkPosition = tile.position + new Vector2Int(x, y);
                Tile t = Utils.TileAt(checkPosition);
                borderMatrix[x +1, y +1] = (t is null) ? TileType.empty : t.type;
            }
        }
    }
    
    private void WaterBorderCheck(ref TileType[,] borderMatrix,
                                  ref Sprite northBorder,
                                  ref Sprite eastBorder,
                                  ref Sprite southBorder,
                                  ref Sprite westBorder) {

        //north
        if (borderMatrix[1,2] == TileType.water) {
            northBorder = water_border_N;
        }
        //east
        if (borderMatrix[2,1] == TileType.water) {
            eastBorder = water_border_E;
        }
        //south
        if (borderMatrix[1,0] == TileType.water) {
            southBorder = water_border_S;
        }
        //west
        if (borderMatrix[0,1] == TileType.water) {
            westBorder = water_border_W;
        }
    }

    private Color[,] Rotate90Clockwise(Sprite sprite, int n)
    {
        int _tileResolution = 16;
        Color[,] colorMatrix = new Color[_tileResolution, _tileResolution];
        Color[,] rotatedMatrix = new Color[_tileResolution, _tileResolution];

        for (int pixelX = 0; pixelX < _tileResolution; pixelX++) {
            for (int pixelY = 0; pixelY < _tileResolution; pixelY++) {
                Color pixelColor = sprite.texture.GetPixel(pixelX + (int) sprite.rect.x, pixelY + (int) sprite.rect.y);
                colorMatrix[pixelX, pixelY] = pixelColor;
            }
        }

        if (n == 0)
        {
            return colorMatrix;
        }
        
        for (int r = 0; r < n; r++)
        {
            rotatedMatrix = new Color[_tileResolution, _tileResolution];
            int j = 0;
            int p = 0;
            int q = 0;
            int i = _tileResolution - 1;

            for (int k = 0; k < _tileResolution; k++)
            {
                while (i >= 0)
                {
                    rotatedMatrix[p, q++] = colorMatrix[i--, j];
                }

                j++;
                i = _tileResolution - 1;
                q = 0;
                p++;
            }

            colorMatrix = rotatedMatrix;
        }
        
        return rotatedMatrix;
    }
    
    private Color[,] GrassBorderCheck(TileType[,] borderMatrix) {

        #region Corner

        if (borderMatrix[1,2] == TileType.grass
            && borderMatrix[0,1] == TileType.grass
            && borderMatrix[2,1] != TileType.grass
            && borderMatrix[1,0] != TileType.grass)
        {

            return Rotate90Clockwise(grass_border_corner, 0);
        }
        else if (borderMatrix[1,2] == TileType.grass
            && borderMatrix[0,1] != TileType.grass
            && borderMatrix[2,1] == TileType.grass
            && borderMatrix[1,0] != TileType.grass) {
            
            return Rotate90Clockwise(grass_border_corner, 1);
        }
        else if (borderMatrix[1,2] != TileType.grass
            && borderMatrix[0,1] != TileType.grass
            && borderMatrix[2,1] == TileType.grass
            && borderMatrix[1,0] == TileType.grass) {
            
            return Rotate90Clockwise(grass_border_corner, 2);
        }
        else if (borderMatrix[1,2] != TileType.grass
            && borderMatrix[0,1] == TileType.grass
            && borderMatrix[2,1] != TileType.grass
            && borderMatrix[1,0] == TileType.grass) {
            
            return Rotate90Clockwise(grass_border_corner, 3);
        }

        #endregion
    
        #region One Side

        if (borderMatrix[1,2] == TileType.grass
            && borderMatrix[0,1] != TileType.grass
            && borderMatrix[2,1] != TileType.grass
            && borderMatrix[1,0] != TileType.grass) {
            
            return Rotate90Clockwise(grass_border_side, 0);
        }
        else if (borderMatrix[1,2] != TileType.grass
            && borderMatrix[0,1] == TileType.grass
            && borderMatrix[2,1] != TileType.grass
            && borderMatrix[1,0] != TileType.grass) {
            
            return Rotate90Clockwise(grass_border_side, 3);
        }
        else if (borderMatrix[1,2] != TileType.grass
            && borderMatrix[0,1] != TileType.grass
            && borderMatrix[2,1] == TileType.grass
            && borderMatrix[1,0] != TileType.grass) {
            
            return Rotate90Clockwise(grass_border_side, 1);
        }
        else if (borderMatrix[1,2] != TileType.grass
            && borderMatrix[0,1] != TileType.grass
            && borderMatrix[2,1] != TileType.grass
            && borderMatrix[1,0] == TileType.grass) {
            
            return Rotate90Clockwise(grass_border_side, 2);
        }

        #endregion
    
        #region PI

        if (borderMatrix[1,2] == TileType.grass
            && borderMatrix[0,1] == TileType.grass
            && borderMatrix[2,1] != TileType.grass
            && borderMatrix[1,0] == TileType.grass) {
            
            return Rotate90Clockwise(grass_border_pi, 0);
        }
        else if (borderMatrix[1,2] == TileType.grass
            && borderMatrix[0,1] == TileType.grass
            && borderMatrix[2,1] == TileType.grass
            && borderMatrix[1,0] != TileType.grass) {
            
            return Rotate90Clockwise(grass_border_pi, 1);
        }
        else if (borderMatrix[1,2] == TileType.grass
            && borderMatrix[0,1] != TileType.grass
            && borderMatrix[2,1] == TileType.grass
            && borderMatrix[1,0] == TileType.grass) {
            
            return Rotate90Clockwise(grass_border_pi, 2);
        }
        else if (borderMatrix[1,2] != TileType.grass
            && borderMatrix[0,1] == TileType.grass
            && borderMatrix[2,1] == TileType.grass
            && borderMatrix[1,0] == TileType.grass) {
            
            return Rotate90Clockwise(grass_border_pi, 3);
        }

        #endregion
    
        #region Tunnel

        if (borderMatrix[1,2] == TileType.grass
            && borderMatrix[0,1] != TileType.grass
            && borderMatrix[2,1] != TileType.grass
            && borderMatrix[1,0] == TileType.grass) {
            
            return Rotate90Clockwise(grass_border_tunnel, 0);
        }
        else if (borderMatrix[1,2] != TileType.grass
            && borderMatrix[0,1] == TileType.grass
            && borderMatrix[2,1] == TileType.grass
            && borderMatrix[1,0] != TileType.grass) {
            
            return Rotate90Clockwise(grass_border_tunnel, 1);
        }

        #endregion
    
        #region All

        if (borderMatrix[1,2] == TileType.grass
            && borderMatrix[0,1] == TileType.grass
            && borderMatrix[2,1] == TileType.grass
            && borderMatrix[1,0] == TileType.grass) {
            
            return Rotate90Clockwise(grass_border_all, 0);
        }

        #endregion
    
        #region Small Corner

        if (borderMatrix[1,2] != TileType.grass
            && borderMatrix[2,1] != TileType.grass
            && borderMatrix[2,2] == TileType.grass) {
            
            return Rotate90Clockwise(grass_border_Lcorner, 0);
        }
        else if (borderMatrix[1,2] != TileType.grass
            && borderMatrix[0,1] != TileType.grass
            && borderMatrix[0,2] == TileType.grass) {
            
            return Rotate90Clockwise(grass_border_Lcorner, 3);
        }
        else if (borderMatrix[1,0] != TileType.grass
            && borderMatrix[0,1] != TileType.grass
            && borderMatrix[0,0] == TileType.grass) {
            
            return Rotate90Clockwise(grass_border_Lcorner, 2);
        }
        else if (borderMatrix[1,0] != TileType.grass
            && borderMatrix[2,1] != TileType.grass
            && borderMatrix[2,0] == TileType.grass) {
            
            return Rotate90Clockwise(grass_border_Lcorner, 1);
        }
        
        #endregion
        
        return Rotate90Clockwise(sand_central, 0);
    }
    
    #endregion
}

public enum TileType {
    empty,
    sand,
    grass,
    water
}
