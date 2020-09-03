using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator
{
    public Human CreateHuman(Vector2Int position) {

        GameObject humanGO = GameObject.Instantiate(PrefabStorage.Instance.human);
        Human newHuman = new Human(humanGO);

        newHuman.motionController.SetPosition(position);

        //delete me

        Tile t = GameManager.Instance.world.GetTileAt(new Vector2Int(40, 40));

        newHuman.motionController.SetDestination(t);
        //

        return newHuman;
    }
}
