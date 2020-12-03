using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProceduralBuilder : MonoBehaviour
{
    public BuilderManager builderManager;
    private ProcGrid procGrid;

    public bool startGeneration = false;

    private void Start()
    {
        BuildingDatabase.Initialize();
    }

    private void Update()
    {
        if (startGeneration)
        {
            startGeneration = false;
            StartGeneration();
        }
    }

    public void StartGeneration()
    {
        if (builderManager == null)
        {
            Debug.LogError("BuildManager can't be null");
            return;
        }

        procGrid = builderManager.Grid;
        ApplyBaseFilters();
        PlaceBuildings();
    }

    private void PlaceBuildings()
    {
        for (int y = 0; y < procGrid.Size.y; y++)
        {
            for (int x = 0; x < procGrid.Size.x; x++)
            {
                for (int z = 0; z < procGrid.Size.z; z++)
                {
                    Cell cell = procGrid.GetCell(x, y, z);
                    if (cell.enabled)
                    {
                        Building building = BuildingDatabase.GetRandomBuilding(cell.buildProperties);
                        if (building != null)
                            Instantiate(building.gameObject, cell.GetCenter(), Quaternion.identity);
                    }
                }
            }
        }
        builderManager.ClearPreview();
    }

    private void PlaceBlock()
    {

    }

    public void ApplyBaseFilters()
    {
        for (int x = 0; x < procGrid.Size.x; x++)
        {
            for (int y = 0; y < procGrid.Size.y; y++)
            {
                for (int z = 0; z < procGrid.Size.z; z++)
                {
                    Cell cell = procGrid.GetCell(x, y, z);

                    if (!cell.buildProperties.corner && CheckIsCorner(cell))
                    {
                        cell.buildProperties.corner = true;
                        Cell topCell = procGrid.GetCellInDirection(cell, EDirection.Top);
                        if (topCell != null)
                            topCell.buildProperties.corner = true;
                    }

                    if (CheckIsGround(cell))
                    {
                        cell.buildProperties.ground = true;
                    }
                    else if (CheckIsTop(cell))
                    {
                        cell.buildProperties.roof = true;
                    }
                }
            }
        }
    }

    private bool CheckIsTop(Cell cell)
    {
        Cell topCell = procGrid.GetCellInDirection(cell, EDirection.Top);
        if (topCell != null)
            return !topCell.enabled;
        else
            return true;
    }

    private bool CheckIsGround(Cell cell)
    {
        return (cell.Position.y == 0);
    }

    private bool CheckIsCorner(Cell cell)
    {
        Cell[] sideCells = new Cell[7];
        sideCells[0] = procGrid.GetCellInDirection(cell, EDirection.Back);
        sideCells[1] = procGrid.GetCellInDirection(cell, EDirection.Right);
        sideCells[2] = procGrid.GetCellInDirection(cell, EDirection.Front);
        sideCells[3] = procGrid.GetCellInDirection(cell, EDirection.Left);

        sideCells[4] = procGrid.GetCellInDirection(cell, EDirection.Back);
        sideCells[5] = procGrid.GetCellInDirection(cell, EDirection.Right);
        sideCells[6] = procGrid.GetCellInDirection(cell, EDirection.Front);

        for (int i = 0; i <= 3; i++)
        {
            bool conditionA = false;
            bool conditionB = false;

            bool res = conditionA && conditionB;

            if (sideCells[i] != null && sideCells[i].enabled && sideCells[i + 1] != null && sideCells[i + 1].enabled)
                conditionA = true;

            if (((sideCells[i + 2] != null && !sideCells[i + 2].enabled) || sideCells[i + 2] == null) == true
                    && ((sideCells[i + 3] != null && !sideCells[i + 3].enabled) || sideCells[i + 3] == null) == true)
                conditionB = true;

            if (conditionA == true && conditionB == true)
                return true;
        }

        return false;
    }
}
