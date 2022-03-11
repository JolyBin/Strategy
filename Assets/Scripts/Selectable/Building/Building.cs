using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : SelectableObject
{
    public int XSize = 3;
    public int ZSizr = 3;
    public Renderer Renderer;

    private Color _startColor;

    private void Awake()
    {
        _startColor = Renderer.material.color;
        //BuildingPlacer buildingPlacer = buildingPlacer = FindObjectOfType<BuildingPlacer>();
        //Vector3 point = transform.position / buildingPlacer.CellSize;

        //int x = Mathf.RoundToInt(point.x);
        //int z = Mathf.RoundToInt(point.z);

        //transform.position = new Vector3(x, 0, z) * buildingPlacer.CellSize;

        //buildingPlacer.InstallBuilding(x, z, this);
    }

    private void OnDrawGizmosSelected()
    {
        float cellSize = FindObjectOfType<BuildingPlacer>().CellSize;
        for (int x = 0; x < XSize; x++)
        {
            for (int z = 0; z < XSize; z++)
            {
                Gizmos.DrawWireCube(transform.position + new Vector3(x, 0, z) * cellSize, new Vector3(1, 0, 1) * cellSize);
            }
        }
    }

    public void DisplayUnaccerptablePosition()
    {
        Renderer.material.color = Color.red;
    }

    public void DisplayAccerptablePosition()
    {
        Renderer.material.color = _startColor;
    }
}
