﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlacable {
    void PutOnTile();
    void RemoveFromTile();
    void AddToRegionContent();
    void RemoveFromRegionContent();
}