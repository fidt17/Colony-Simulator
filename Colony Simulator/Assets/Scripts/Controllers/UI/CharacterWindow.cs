using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterWindow : WindowComponent
{
    public Character character;

    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterHunger;

    private void Awake() {

        FindWindowObjects();
    }

    private void FindWindowObjects() {

        characterName = transform.Find("Character Name TMP").GetComponent<TextMeshProUGUI>();
        characterHunger = transform.Find("Character Hunger TMP").GetComponent<TextMeshProUGUI>();
    }

    private void Update() {

        if (character == null)
            return;

        characterName.text = character.data.name;
        characterHunger.text = "Hunger: " + character.hungerComponent.hunger;
    }

    public override void CloseWindow() {

        base.CloseWindow();
        character = null;
    }
}
