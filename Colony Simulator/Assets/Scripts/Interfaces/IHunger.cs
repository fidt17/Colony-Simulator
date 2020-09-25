using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHunger {
    HungerComponent hungerComponent { get; }
    void InitializeHungerComponent();
}
