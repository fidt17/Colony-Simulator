using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Static/WallScriptableObject", order = 1)]
public class WallScriptableObject : StaticScriptableObject
{
    public Color wallColorFilter;
    
    [Header("Wall sprites")]
    public Sprite wall_c = default;
    public Sprite wall_n = default;
    public Sprite wall_e = default;
    public Sprite wall_ne = default;
    public Sprite wall_s = default;
    public Sprite wall_ns = default;
    public Sprite wall_es = default;
    public Sprite wall_nes = default;
    public Sprite wall_w = default;
    public Sprite wall_wn = default;
    public Sprite wall_we = default;
    public Sprite wall_wne = default;
    public Sprite wall_ws = default;
    public Sprite wall_wns = default;
    public Sprite wall_wes = default;
    public Sprite wall_wnes = default;
    
    private enum Directions
    {
        WEST,
        NORTH,
        EAST,
        SOUTH
    }
    
    public void ApplyCorrectWallSprite(Construction wall, bool recursive = true)
    {
        bool[] borders = new bool[4];
        FindNeighbourConstructions(wall, borders, recursive);
        
        SpriteRenderer sr = wall.gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = GetSpriteBasedOnNeighbours(borders);;
        sr.color = wallColorFilter;
    }

    private void FindNeighbourConstructions(Construction wall, bool[] borders, bool recursive)
    {
        Tile t;
        StaticObject neighbour;
        int x, y;
        
        x = -1;
        y = 0;
        t = Utils.TileAt(wall.Position.x + x, wall.Position.y + y);
        neighbour = t?.Contents.StaticObject;
        if (neighbour?.Data?.dataName == wall.Data.dataName)
        {
            borders[(int)Directions.WEST] = true;
            if (recursive)
            {
                ((WallScriptableObject) neighbour.Data).ApplyCorrectWallSprite(neighbour as Construction, false);
            }
        }
        else
        {
            borders[(int) Directions.WEST] = false;
        }
        
        x = 0;
        y = 1;
        t = Utils.TileAt(wall.Position.x + x, wall.Position.y + y);
        neighbour = t?.Contents.StaticObject;
        if (neighbour?.Data?.dataName == wall.Data.dataName)
        {
            borders[(int)Directions.NORTH] = true;
            if (recursive)
            {
                ((WallScriptableObject) neighbour.Data).ApplyCorrectWallSprite(neighbour as Construction, false);
            }
        }
        else
        {
            borders[(int) Directions.NORTH] = false;
        }
        
        x = 1;
        y = 0;
        t = Utils.TileAt(wall.Position.x + x, wall.Position.y + y);
        neighbour = t?.Contents.StaticObject;
        if (neighbour?.Data?.dataName == wall.Data.dataName)
        {
            borders[(int)Directions.EAST] = true;
            if (recursive)
            {
                ((WallScriptableObject) neighbour.Data).ApplyCorrectWallSprite(neighbour as Construction, false);
            }
        }
        else
        {
            borders[(int) Directions.EAST] = false;
        }
        
        x = 0;
        y = -1;
        t = Utils.TileAt(wall.Position.x + x, wall.Position.y + y);
        neighbour = t?.Contents.StaticObject;
        if (neighbour?.Data?.dataName == wall.Data.dataName)
        {
            borders[(int)Directions.SOUTH] = true;
            if (recursive)
            {
                ((WallScriptableObject) neighbour.Data).ApplyCorrectWallSprite(neighbour as Construction, false);
            }
        }
        else
        {
            borders[(int) Directions.SOUTH] = false;
        }
    }

    private Sprite GetSpriteBasedOnNeighbours(bool[] borders)
    {
        Sprite result = wall_c;

        if (borders[(int) Directions.WEST]  == true
         && borders[(int) Directions.NORTH] == true
         && borders[(int) Directions.EAST]  == true
         && borders[(int) Directions.SOUTH] == true)
        {
            result = wall_wnes;
            return result;
        }
        
        if (borders[(int) Directions.WEST]     == true
            && borders[(int) Directions.NORTH] == false
            && borders[(int) Directions.EAST]  == true
            && borders[(int) Directions.SOUTH] == true)
        {
            result = wall_wes;
            return result;
        }
        
        if (borders[(int) Directions.WEST]     == true
            && borders[(int) Directions.NORTH] == true
            && borders[(int) Directions.EAST]  == false
            && borders[(int) Directions.SOUTH] == true)
        {
            result = wall_wns;
            return result;
        }
        
        if (borders[(int) Directions.WEST]     == true
            && borders[(int) Directions.NORTH] == false
            && borders[(int) Directions.EAST]  == false
            && borders[(int) Directions.SOUTH] == true)
        {
            result = wall_ws;
            return result;
        }
        
        if (borders[(int) Directions.WEST]     == true
            && borders[(int) Directions.NORTH] == true
            && borders[(int) Directions.EAST]  == true
            && borders[(int) Directions.SOUTH] == false)
        {
            result = wall_wne;
            return result;
        }
        
        if (borders[(int) Directions.WEST]     == true
            && borders[(int) Directions.NORTH] == false
            && borders[(int) Directions.EAST]  == true
            && borders[(int) Directions.SOUTH] == false)
        {
            result = wall_we;
            return result;
        }
        
        if (borders[(int) Directions.WEST]     == true
            && borders[(int) Directions.NORTH] == true
            && borders[(int) Directions.EAST]  == false
            && borders[(int) Directions.SOUTH] == false)
        {
            result = wall_wn;
            return result;
        }
        
        if (borders[(int) Directions.WEST]     == true
            && borders[(int) Directions.NORTH] == false
            && borders[(int) Directions.EAST]  == false
            && borders[(int) Directions.SOUTH] == false)
        {
            result = wall_w;
            return result;
        }
        
        if (borders[(int) Directions.WEST]     == false
            && borders[(int) Directions.NORTH] == true
            && borders[(int) Directions.EAST]  == true
            && borders[(int) Directions.SOUTH] == true)
        {
            result = wall_nes;
            return result;
        }
        
        if (borders[(int) Directions.WEST]     == false
            && borders[(int) Directions.NORTH] == false
            && borders[(int) Directions.EAST]  == true
            && borders[(int) Directions.SOUTH] == true)
        {
            result = wall_es;
            return result;
        }
        
        if (borders[(int) Directions.WEST]     == false
            && borders[(int) Directions.NORTH] == true
            && borders[(int) Directions.EAST]  == false
            && borders[(int) Directions.SOUTH] == true)
        {
            result = wall_ns;
            return result;
        }
        
        if (borders[(int) Directions.WEST]     == false
            && borders[(int) Directions.NORTH] == false
            && borders[(int) Directions.EAST]  == false
            && borders[(int) Directions.SOUTH] == true)
        {
            result = wall_s;
            return result;
        }
        
        if (borders[(int) Directions.WEST]     == false
            && borders[(int) Directions.NORTH] == true
            && borders[(int) Directions.EAST]  == true
            && borders[(int) Directions.SOUTH] == false)
        {
            result = wall_ne;
            return result;
        }
        
        if (borders[(int) Directions.WEST]     == false
            && borders[(int) Directions.NORTH] == false
            && borders[(int) Directions.EAST]  == true
            && borders[(int) Directions.SOUTH] == false)
        {
            result = wall_e;
            return result;
        }
        
        if (borders[(int) Directions.WEST]     == false
            && borders[(int) Directions.NORTH] == true
            && borders[(int) Directions.EAST]  == false
            && borders[(int) Directions.SOUTH] == false)
        {
            result = wall_n;
            return result;
        }
        
        return result;
    }
}
