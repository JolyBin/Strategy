using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public float CellSize = 1f;
    public Camera RaycastCamera;
    public Building CurrentBuilding;

    public Dictionary<Vector2Int, Building> BuildingDictionary = new Dictionary<Vector2Int, Building>();

    ResourcesManager _resourcesManager;

    Plane _plane;
    void Start()
    {
        _resourcesManager = FindObjectOfType<ResourcesManager>();
        _plane = new Plane(Vector3.up, Vector3.zero);
    }
    void Update()
    {
        if (!CurrentBuilding)
            return;

        Ray ray = RaycastCamera.ScreenPointToRay(Input.mousePosition);

        float distance;
        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance) / CellSize;

        int x = Mathf.RoundToInt(point.x);
        int z = Mathf.RoundToInt(point.z);

        CurrentBuilding.transform.position = new Vector3(x, 0, z) * CellSize ;

        if (CheckAllow(x, z, CurrentBuilding))
        {
            CurrentBuilding.DisplayAccerptablePosition();
            if (Input.GetMouseButtonDown(0))
            {
                InstallBuilding(x, z, CurrentBuilding);
                _resourcesManager.SetMoney(-CurrentBuilding.Price);
                CurrentBuilding = null;
            }

        }
        else
        {
            CurrentBuilding.DisplayUnaccerptablePosition();
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(CurrentBuilding.gameObject);
            CurrentBuilding = null;
        }
    }

    bool CheckAllow(int xPositionm, int zPosition, Building building)
    {
        for (int x = 0; x < building.XSize; x++)
        {
            for (int z = 0; z < building.ZSizr; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPositionm + x, zPosition + z);
                if(BuildingDictionary.ContainsKey(coordinate))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void InstallBuilding(int xPositionm, int zPosition, Building buildin)
    {
        for (int x = 0; x < buildin.XSize; x++)
        {
            for (int z = 0; z < buildin.ZSizr; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPositionm + x, zPosition + z);
                BuildingDictionary.Add(coordinate, buildin);
            }
        }
    }

    public void CreateBuilding(GameObject buildingPrefab)
    {
        if (_resourcesManager.Money >= buildingPrefab.GetComponent<Building>().Price)
        {
            GameObject newBuilding = Instantiate(buildingPrefab);
            CurrentBuilding = newBuilding.GetComponent<Building>();
        }
    }
}
