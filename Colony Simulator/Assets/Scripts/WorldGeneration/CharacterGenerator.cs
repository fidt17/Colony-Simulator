using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator
{
    public Human CreateHuman(Vector2Int position) {

        GameObject humanGO = GameObject.Instantiate(PrefabStorage.Instance.human);
        Human newHuman = new Human(humanGO);

        Tile startTile = GameManager.Instance.world.GetTileAt(position);

        newHuman.SetPosition(startTile);

        return newHuman;
    }
}
