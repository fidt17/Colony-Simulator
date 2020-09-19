﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterWindow : WindowComponent {

    public Character character;

    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private Transform characterHungerFillImage;

    private void Update() {

        if (character == null)
            return;

        characterName.text = character.data.name;
        characterHungerFillImage.localScale = new Vector3(character.hungerComponent.HungerLevel / 100, 1, 1);
    }

    public override void CloseWindow() {

        base.CloseWindow();
        character = null;
    }
}