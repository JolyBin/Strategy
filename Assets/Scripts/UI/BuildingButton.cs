using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingButton : SelectbleButton
{
    public BuildingPlacer BuildingPlacer;

    public override void TryBuy()
    {
        if (!BuildingPlacer.CurrentBuilding)
        {
            BuildingPlacer.CreateBuilding(SelectblePrefab);
        }
    }
}
