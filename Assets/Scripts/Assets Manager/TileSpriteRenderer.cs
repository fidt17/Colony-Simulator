using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpriteRenderer : Singleton<TileSpriteRenderer> {

    [Header("Central tiles")]
    public Sprite sand_central;

    [Header("Water Borders")]
    public Sprite water_border_N;
    public Sprite water_border_E;
    public Sprite water_border_S;
    public Sprite water_border_W;

    [Header("Grass Borders")]
    public Sprite grass_border_corner;
    public Sprite grass_border_Lcorner;
    public Sprite grass_border_side;
    public Sprite grass_border_pi;
    public Sprite grass_border_tunnel;
    public Sprite grass_border_all;

    #region Tile Sprite Generation

    /*
    public void UpdateTileBorders(Tile tile) {
        TileType type = tile.type;
        
        SpriteRenderer mainSprite  = tile.gameObject.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        SpriteRenderer northBorder = null;
        SpriteRenderer eastBorder  = null;
        SpriteRenderer southBorder = null;
        SpriteRenderer westBorder  = null;
        TileType[,] borderMatrix   = new TileType[3,3];

        if (type != TileType.sand) {
            return;
        }

        FindBorders(tile, ref northBorder, ref eastBorder, ref southBorder, ref westBorder, ref borderMatrix);
        
        WaterBorderCheck(borderMatrix, northBorder, eastBorder, southBorder, westBorder);
        
        GrassBorderCheck(borderMatrix, mainSprite);
    }

    private void WaterBorderCheck(TileType[,] borderMatrix,
                                  SpriteRenderer northBorder,
                                  SpriteRenderer eastBorder,
                                  SpriteRenderer southBorder,
                                  SpriteRenderer westBorder) {

        //north
        if (borderMatrix[1,2] == TileType.water) {
            northBorder.sprite = water_border_N;
        }
        //east
        if (borderMatrix[2,1] == TileType.water) {
            eastBorder.sprite = water_border_E;
        }
        //south
        if (borderMatrix[1,0] == TileType.water) {
            southBorder.sprite = water_border_S;
        }
        //west
        if (borderMatrix[0,1] == TileType.water) {
            westBorder.sprite = water_border_W;
        }
    }
    */

    /*
    private void GrassBorderCheck(TileType[,] borderMatrix,
                                  SpriteRenderer mainSprite) {

        #region Corner

        if (borderMatrix[1,2] == TileType.grass
            && borderMatrix[0,1] == TileType.grass
            && borderMatrix[2,1] != TileType.grass
            && borderMatrix[1,0] != TileType.grass) {
            
            mainSprite.sprite = grass_border_corner;
            mainSprite.transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
            return;
        }
        else if (borderMatrix[1,2] == TileType.grass
            && borderMatrix[0,1] != TileType.grass
            && borderMatrix[2,1] == TileType.grass
            && borderMatrix[1,0] != TileType.grass) {
            
            mainSprite.sprite = grass_border_corner;
            mainSprite.transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
            return;
        }
        else if (borderMatrix[1,2] != TileType.grass
            && borderMatrix[0,1] != TileType.grass
            && borderMatrix[2,1] == TileType.grass
            && borderMatrix[1,0] == TileType.grass) {
            
            mainSprite.sprite = grass_border_corner;
            mainSprite.transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
            return;
        }
        else if (borderMatrix[1,2] != TileType.grass
            && borderMatrix[0,1] == TileType.grass
            && borderMatrix[2,1] != TileType.grass
            && borderMatrix[1,0] == TileType.grass) {
            
            mainSprite.sprite = grass_border_corner;
            mainSprite.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
            return;
        }

        #endregion
    
        #region One Side

        if (borderMatrix[1,2] == TileType.grass
            && borderMatrix[0,1] != TileType.grass
            && borderMatrix[2,1] != TileType.grass
            && borderMatrix[1,0] != TileType.grass) {
            
            mainSprite.sprite = grass_border_side;
            mainSprite.transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
            return;
        }
        else if (borderMatrix[1,2] != TileType.grass
            && borderMatrix[0,1] == TileType.grass
            && borderMatrix[2,1] != TileType.grass
            && borderMatrix[1,0] != TileType.grass) {
            
            mainSprite.sprite = grass_border_side;
            mainSprite.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
            return;
        }
        else if (borderMatrix[1,2] != TileType.grass
            && borderMatrix[0,1] != TileType.grass
            && borderMatrix[2,1] == TileType.grass
            && borderMatrix[1,0] != TileType.grass) {
            
            mainSprite.sprite = grass_border_side;
            mainSprite.transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
            return;
        }
        else if (borderMatrix[1,2] != TileType.grass
            && borderMatrix[0,1] != TileType.grass
            && borderMatrix[2,1] != TileType.grass
            && borderMatrix[1,0] == TileType.grass) {
            
            mainSprite.sprite = grass_border_side;
            mainSprite.transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
            return;
        }

        #endregion
    
        #region PI

        if (borderMatrix[1,2] == TileType.grass
            && borderMatrix[0,1] == TileType.grass
            && borderMatrix[2,1] != TileType.grass
            && borderMatrix[1,0] == TileType.grass) {
            
            mainSprite.sprite = grass_border_pi;
            mainSprite.transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
            return;
        }
        else if (borderMatrix[1,2] == TileType.grass
            && borderMatrix[0,1] == TileType.grass
            && borderMatrix[2,1] == TileType.grass
            && borderMatrix[1,0] != TileType.grass) {
            
            mainSprite.sprite = grass_border_pi;
            mainSprite.transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
            return;
        }
        else if (borderMatrix[1,2] == TileType.grass
            && borderMatrix[0,1] != TileType.grass
            && borderMatrix[2,1] == TileType.grass
            && borderMatrix[1,0] == TileType.grass) {
            
            mainSprite.sprite = grass_border_pi;
            mainSprite.transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
            return;
        }
        else if (borderMatrix[1,2] != TileType.grass
            && borderMatrix[0,1] == TileType.grass
            && borderMatrix[2,1] == TileType.grass
            && borderMatrix[1,0] == TileType.grass) {
            
            mainSprite.sprite = grass_border_pi;
            mainSprite.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
            return;
        }

        #endregion
    
        #region Tunnel

        if (borderMatrix[1,2] == TileType.grass
            && borderMatrix[0,1] != TileType.grass
            && borderMatrix[2,1] != TileType.grass
            && borderMatrix[1,0] == TileType.grass) {
            
            mainSprite.sprite = grass_border_tunnel;
            mainSprite.transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
            return;
        }
        else if (borderMatrix[1,2] != TileType.grass
            && borderMatrix[0,1] == TileType.grass
            && borderMatrix[2,1] == TileType.grass
            && borderMatrix[1,0] != TileType.grass) {
            
            mainSprite.sprite = grass_border_tunnel;
            mainSprite.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
            return;
        }

        #endregion
    
        #region All

        if (borderMatrix[1,2] == TileType.grass
            && borderMatrix[0,1] == TileType.grass
            && borderMatrix[2,1] == TileType.grass
            && borderMatrix[1,0] == TileType.grass) {
            
            mainSprite.sprite = grass_border_all;
            mainSprite.transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
            return;
        }

        #endregion
    
        #region Small Corner

        if (borderMatrix[1,2] != TileType.grass
            && borderMatrix[2,1] != TileType.grass
            && borderMatrix[2,2] == TileType.grass) {
            
            mainSprite.sprite = grass_border_Lcorner;
            mainSprite.transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
            return;
        }
        else if (borderMatrix[1,2] != TileType.grass
            && borderMatrix[0,1] != TileType.grass
            && borderMatrix[0,2] == TileType.grass) {
            
            mainSprite.sprite = grass_border_Lcorner;
            mainSprite.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
            return;
        }
        else if (borderMatrix[1,0] != TileType.grass
            && borderMatrix[0,1] != TileType.grass
            && borderMatrix[0,0] == TileType.grass) {
            
            mainSprite.sprite = grass_border_Lcorner;
            mainSprite.transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
            return;
        }
        else if (borderMatrix[1,0] != TileType.grass
            && borderMatrix[2,1] != TileType.grass
            && borderMatrix[2,0] == TileType.grass) {
            
            mainSprite.sprite = grass_border_Lcorner;
            mainSprite.transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
            return;
        }

        #endregion
    }
    */

    #endregion
}
