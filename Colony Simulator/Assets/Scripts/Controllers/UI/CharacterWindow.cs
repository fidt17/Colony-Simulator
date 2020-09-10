using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterWindow : WindowComponent
{
    public Character character;

    private TextMeshProUGUI characterName;
    private TextMeshProUGUI characterHunger;

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
        characterHunger.text = "Hunger: " + character.hungerComponent.HungerLevel.ToString("0.0");
    }

    public override void CloseWindow() {

        base.CloseWindow();
        character = null;
    }
}
