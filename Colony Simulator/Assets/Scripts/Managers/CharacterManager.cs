using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    private CharacterGenerator _CG;

    List<Human> colonists = new List<Human>();

    private void Awake() {

        if (Instance != null) {

            Debug.LogError("Only one CharacterManager can exist!");
            Destroy(gameObject);
        }

        Instance = this;

        _CG = new CharacterGenerator();
    }

    public void CreateInitialCharacters() {

        Human human = _CG.CreateHuman(new Vector2Int(45, 35));
        colonists.Add(human);        
    }
}
