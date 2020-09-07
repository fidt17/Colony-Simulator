using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterGenerator
{
    public static Human CreateHuman(Vector2Int position) {

        var character = CharacterSpawnFactory.GetNewCharacter("human") as Human;

        if (character == null) {

            Debug.LogError("Was not able to create new character.");
            return null;
        }

        character.motionComponent.SetPosition(position);

        return character;
    }

    public static Rabbit CreateRabbit(Vector2Int position) {
        
        var character = CharacterSpawnFactory.GetNewCharacter("rabbit") as Rabbit;

        if (character == null) {

            Debug.LogError("Was not able to create new character.");
            return null;
        }

        character.motionComponent.SetPosition(position);

        return character;
    }
}
