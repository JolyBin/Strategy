using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrack : Building
{
    public Transform Spawn;

    ResourcesManager _resourcesManager;

    protected override void Start()
    {
        base.Start();
        _resourcesManager = FindObjectOfType<ResourcesManager>();
    }
    public void CreateUnit(GameObject unit)
    {
        Management.CurrentSelectionState = SelectionState.BuildingSelected;
        int price = unit.GetComponent<Unit>().Price;
        if (_resourcesManager.Money >= price)
        {
            GameObject newUnit = Instantiate(unit, Spawn.position, Quaternion.identity);
            _resourcesManager.SetMoney(-price);

            Vector3 offset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            newUnit.GetComponent<Unit>().WhenClickOnGround(Spawn.position + offset);
        }
    }
}
