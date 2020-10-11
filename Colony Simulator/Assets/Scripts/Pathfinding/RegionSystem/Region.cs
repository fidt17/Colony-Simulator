using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region {

    public List<Subregion> subregions = new List<Subregion>();

    public void AddSubregion(Subregion subregion) {
        subregions.Add(subregion);
        subregion.SetRegion(this);
    }

    public void RemoveSubregion(Subregion subregion) {
        subregions.Remove(subregion);
        subregion.SetRegion(null);
    }

    public void Reset() {
        for (int i = subregions.Count - 1; i >= 0; i--) {
            RemoveSubregion(subregions[i]);
        }
        RegionSystem.regions.Remove(this);
    }
}