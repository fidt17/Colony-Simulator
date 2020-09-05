using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager
{
    private CharacterGenerator _CG;

    List<Human> colonists = new List<Human>();

    public CharacterManager() {
        
        _CG = new CharacterGenerator();
    }

    public void CreateInitialCharacters() {

        Human human = _CG.CreateHuman(new Vector2Int(45, 35));
        colonists.Add(human);        
    }
}
