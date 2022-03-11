using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : SelectbleButton
{
    public Barrack Barrack;
    public override void TryBuy()
    {
        Barrack.CreateUnit(SelectblePrefab);
    }
}
