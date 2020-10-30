using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterWindow : WindowComponent {

    public Character character;

    [SerializeField] private TextMeshProUGUI characterName      = null;
    [SerializeField] private Transform characterHungerFillImage = null;

    private void Update() {
        if (character is null) {
            return;
        }

        characterName.text = character.Data.name;
        characterHungerFillImage.localScale = new Vector3(character.HungerComponent.HungerLevel / 100, 1, 1);
    }

    public override void CloseWindow() {
        base.CloseWindow();
        character = null;
    }
}