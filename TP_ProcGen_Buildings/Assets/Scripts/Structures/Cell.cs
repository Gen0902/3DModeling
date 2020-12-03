using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    [HideInInspector] public BuildProperties buildProperties;

    public Vector3Int Position { get; private set; }

    public bool enabled = false;

    public Cell(int x, int y, int z, bool enable = false)
    {
        Position = new Vector3Int(x, y, z);
        enabled = enable;
        buildProperties = new BuildProperties();
    }

    public Vector3 GetCenter()
    {
        return new Vector3(Position.x + 0.5f, Position.y + 0.5f, Position.z + 0.5f);
    }
}
