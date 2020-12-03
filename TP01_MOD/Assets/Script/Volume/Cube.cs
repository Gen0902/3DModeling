using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cube
{
    public Vector3 position;
    public Vector3 size;

    public Cube(Vector3 _position, Vector3 _size)
    {
        position = _position;
        size = _size;
    }
}
