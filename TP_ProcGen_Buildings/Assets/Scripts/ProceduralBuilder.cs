using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProceduralBuilder : MonoBehaviour
{
    public BuilderManager builderManager;
    private ProcGrid procGrid;

    public bool respectSytle = false;
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
        for (int y = 0; y < procGrid.Size.y; y++)               //For each cell in grid (startinf drom ground)
        {
            for (int x = 0; x < procGrid.Size.x; x++)           //              |
            {
                for (int z = 0; z < procGrid.Size.z; z++)       //              |
                {
                    Cell cell = procGrid.GetCell(x, y, z);
                    if (cell.enabled)                           
                    {
                        Module building = BuildingDatabase.GetRandomBuilding(cell.buildProperties);     //Pick one matching cell properties
                        if (building != null)       //If a module was found
                        {
                            PlaceBlock(building, cell);     //Place block at cell pos
                        }
                    }
                }
            }
        }
        builderManager.ClearPreview();
    }

    private void PlaceBlock(Module building, Cell cell)
    {
        float rotation = 0;
        switch (cell.orientation)
        {
            case EOrientation.Right:
                rotation = -90;
                break;
            case EOrientation.Left:
                rotation = 90;
                break;
            case EOrientation.Back:
                rotation = 180;
                break;
            default:
                break;
        }

        Instantiate(building.gameObject, cell.GetCenter(), Quaternion.Euler(0, rotation, 0));
        if (respectSytle)
        {
            Cell topCell = procGrid.GetCellInDirection(cell, EDirection.Top);
            if (topCell != null)
            {
                topCell.buildProperties.style = building.buildProperties.style;     //Propagate window style to top cell
            }
        }
    }

    private void ApplyBaseFilters()
    {
        for (int x = 0; x < procGrid.Size.x; x++)                   //For all cells in grid
        {
            for (int y = 0; y < procGrid.Size.y; y++)               //          |
            {
                for (int z = 0; z < procGrid.Size.z; z++)           //          |
                {
                    Cell cell = procGrid.GetCell(x, y, z);
                    if (!cell.enabled)          //If cell is not enabled (enalbed = has preview block)
                        continue;

                    if (CheckIsCorner(cell))    //If cell is a corner
                    {
                        cell.buildProperties.corner = true;                                 //Set corner prop to true
                        Cell topCell = procGrid.GetCellInDirection(cell, EDirection.Top);   //Get top cell
                        if (topCell != null)
                            topCell.buildProperties.corner = true;               //Propagate corner property to top cell 
                    }

                    if (CheckIsGround(cell))    //If cell is on ground                                                   
                    {
                        cell.buildProperties.ground = true;
                    }
                    else if (CheckIsTop(cell))  //If cell is on top
                    {
                        cell.buildProperties.roof = true;
                    }

                    if (cell.orientation == EOrientation.Front) //If cell has default orientation
                    {
                        cell.orientation = ComputeOrientation(cell);    //Compute orientation 
                        Cell topCell = procGrid.GetCellInDirection(cell, EDirection.Top);
                        if (topCell != null)
                            topCell.orientation = cell.orientation;       //Propagate orientation property to top cell 
                    }
                }
            }
        }
    }

    private EOrientation ComputeOrientation(Cell cell)
    {
        Cell backCell = procGrid.GetCellInDirection(cell, EDirection.Back);
        Cell rightCell = procGrid.GetCellInDirection(cell, EDirection.Right);
        Cell frontCell = procGrid.GetCellInDirection(cell, EDirection.Front);
        Cell leftCell = procGrid.GetCellInDirection(cell, EDirection.Left);

        bool back = backCell != null && backCell.enabled;
        bool right = rightCell != null && rightCell.enabled;
        bool front = frontCell != null && frontCell.enabled;
        bool left = leftCell != null && leftCell.enabled;

        EOrientation resultOrientation = EOrientation.Front;

        if (back && !right && !front && !left)
            resultOrientation = EOrientation.Front;
        else if (!back && right && !front && !left)
            resultOrientation = EOrientation.Front;
        else if (!back && !right && front && !left)
            resultOrientation = EOrientation.Right;
        else if (!back && !right && !front && left)
            resultOrientation = EOrientation.Front;

        else if (back && right && !front && !left)
            resultOrientation = EOrientation.Left;
        else if (back && !right && front && !left)
            resultOrientation = EOrientation.Right;
        else if (back && !right && !front && left)
            resultOrientation = EOrientation.Front;
        else if (!back && right && front && !left)
            resultOrientation = EOrientation.Back;
        else if (!back && right && !front && left)
            resultOrientation = EOrientation.Front;
        else if (!back && !right && front && left)
            resultOrientation = EOrientation.Right;

        else if (back && right && front && !left)
            resultOrientation = EOrientation.Left;
        else if (back && right && !front && left)
            resultOrientation = EOrientation.Front;
        else if (back && !right && front && left)
            resultOrientation = EOrientation.Right;
        else if (!back && right && front && left)
            resultOrientation = EOrientation.Back;

        else if (back && right && front && left)
            resultOrientation = EOrientation.Front;
        else if (!back && !right && !front && !left)
            resultOrientation = EOrientation.Front;

        return resultOrientation;
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
