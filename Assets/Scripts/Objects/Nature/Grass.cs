﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : Vegetation, IEdible {

    public int NutritionValue => 20;
    public Vector2Int GetEdiblePosition() => Position;

    public    override void AddToRegionContent()      => Utils.NodeAt(Position.x, Position.y).Subregion.Content.Add<Grass>(this);
    protected override void RemoveFromRegionContent() => Utils.NodeAt(Position.x, Position.y).Subregion.Content.Remove<Grass>(this);
}