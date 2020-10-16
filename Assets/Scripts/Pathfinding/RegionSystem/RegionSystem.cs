using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RegionSystem {

    public static List<Region> regions { get; private set; }

    public static void Initialize() {
        SubregionSystem.CreateSubregions();
        CreateRegions();
    }

    public static void UpdateSystemAt(int x, int y) {
        SubregionSystem.UpdateSubregionAt(x, y);
        ResetRegions();
        CreateRegions();
    }

    private static void CreateRegions() {
        regions = new List<Region>();
        foreach (Subregion subregion in SubregionSystem.subregions) {
            if (subregion.region == null) {
                regions.Add(CreateRegionAt(subregion));
            }
        }
    }

    private static void ResetRegions() {
        if (regions != null) {
            for (int i = regions.Count - 1; i >= 0; i--) {
                regions[i].Reset();
            }
        }
    }

    private static Region CreateRegionAt(Subregion subregion) {
        Region region = new Region();
        List<Subregion> openSet = new List<Subregion>();
        openSet.Add(subregion);
        do {
        } while(NextWaveIteration(ref openSet, ref region));

        return region;
	}


    private static bool NextWaveIteration(ref List<Subregion> openSet, ref Region region) {
        if (openSet.Count == 0) {
            return false;
        }

        for (int i = openSet.Count - 1; i >= 0; i--) {
            if (openSet[i].region != region) {
                if (openSet[i].region is null) {
                    region.AddSubregion(openSet[i]);
                    continue;
                }
            }
        }

        foreach (Subregion neighbour in openSet[0].neighbouringSubregions) {
            if (neighbour.region == region || openSet.Contains(neighbour)) {
                continue;
            }
            openSet.Add(neighbour);
        }
        openSet.RemoveAt(0);

        return true;
	}
}