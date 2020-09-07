using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager
{
    List<Human> colonists = new List<Human>();
    List<Rabbit> rabbits = new List<Rabbit>();

    public void CreateInitialCharacters() {

        Human human = CharacterGenerator.CreateHuman(new Vector2Int(45, 35));
        if (human != null)
            colonists.Add(human);

        int rabbitCount = 1;

        for (int i = 0; i < rabbitCount; i++) {
            
            Rabbit rabbit = CharacterGenerator.CreateRabbit(new Vector2Int(40, 35));
            if (rabbit != null)
                rabbits.Add(rabbit);
        }
    }
}
