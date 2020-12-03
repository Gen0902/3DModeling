using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcGrid
{
    private Cell[,,] grid;
    public Vector3Int Size { get; private set; }

    public ProcGrid(Vector3Int size)
    {
        Size = size;
        grid = new Cell[Size.x, Size.y, Size.z];
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                for (int z = 0; z < Size.z; z++)
                {
                    grid[x, y, z] = new Cell(x, y, z);
                }
            }
        }
    }

    public void EnableCell(Vector3Int cellPos)
    {
        grid[cellPos.x, cellPos.y, cellPos.z].enabled = true;
    }

    public void DisableCell(Vector3Int cellPos)
    {
        grid[cellPos.x, cellPos.y, cellPos.z].enabled = false;
    }

    public Cell GetCell(Vector3Int pos)
    {
        return grid[pos.x, pos.y, pos.z];
    }

    public Cell GetCell(int x, int y, int z)
    {
        if (IsInGrid(x, y, z))
            return grid[x, y, z];
        else
            return null;
    }

    public Vector3Int? GetCellByWorldPos(Vector3 position)
    {
        Vector3Int cellPos = new Vector3Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), Mathf.FloorToInt(position.z));
        if (cellPos.x < 0 || cellPos.x >= Size.x || cellPos.y < 0 || cellPos.y >= Size.y || cellPos.z < 0 || cellPos.z >= Size.z)
        {
            return null;
        }
        else
            return cellPos;
    }

    public Cell GetCellInDirection(Cell cell, EDirection direction)
    {
        int newX = cell.Position.x;
        int newY = cell.Position.y;
        int newZ = cell.Position.z;
        switch (direction)
        {
            case EDirection.Self:
                break;
            case EDirection.Right:
                newX++;
                break;
            case EDirection.Top:
                newY++;
                break;
            case EDirection.Back:
                newZ++;
                break;
            case EDirection.Left:
                newX--;
                break;
            case EDirection.Down:
                newY--;
                break;
            case EDirection.Front:
                newZ--;
                break;
            default:
                break;
        }

        if (IsInGrid(newX, newY, newZ))
            return GetCell(newX, newY, newZ);
        else
            return null;
    }

    public Vector3 CellCenter(Vector3Int cellPos)
    {
        return new Vector3(cellPos.x + 0.5f, cellPos.y + 0.5f, cellPos.z + 0.5f);
    }

    public bool IsInGrid(int x, int y, int z)
    {
        if (x < 0 || x >= Size.x || y < 0 || y >= Size.y || z < 0 || z >= Size.z)
            return false;
        else
            return true;
    }
}
