using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Volume
{
    public class Box
    {
        public Vector3 Start { get; private set; }
        public Vector3 End { get; private set; }
        public Vector3 Size { get; private set; }

        public Node[,,] nodes;
        public Vector3Int NodesLength { get; private set; }


        public Box(Vector3 _start, Vector3 _end, float _cellSize)
        {
            Size = _end - _start;
            NodesLength = new Vector3Int(
                Mathf.CeilToInt(Size.x / _cellSize),
                Mathf.CeilToInt(Size.y / _cellSize),
                Mathf.CeilToInt(Size.z / _cellSize));

            nodes = new Node[NodesLength.x, NodesLength.y, NodesLength.z];

            Start = _start;
            End = Start + new Vector3(NodesLength.x * _cellSize, NodesLength.y * _cellSize, NodesLength.z * _cellSize);

            for (int x = 0; x < NodesLength.x; x++)
            {
                for (int y = 0; y < NodesLength.y; y++)
                {
                    for (int z = 0; z < NodesLength.z; z++)
                    {
                        nodes[x, y, z] = new Node(new Vector3(x * _cellSize, y * _cellSize, z * _cellSize) + Start);
                        //Gizmos.DrawSphere(new Vector3(x * _cellSize , y * _cellSize, z * _cellSize) + start,0.1f);
                    }
                }
            }
        }

        public static Box CreateBoundingBox(Sphere[] _spheres, float _cellSize)
        {
            if (_spheres.Length <= 0)
                return null;

            Vector3 start = _spheres[0].center;
            Vector3 end = _spheres[0].center;

            foreach (Sphere sphere in _spheres)
            {
                start = Vector3.Min(sphere.center - sphere.radius * Vector3.one, start);
                end = Vector3.Max(sphere.center + sphere.radius * Vector3.one, end);
            }

            Vector3Int startInt = new Vector3Int(Mathf.FloorToInt(start.x), Mathf.FloorToInt(start.y), (Mathf.FloorToInt(start.z)));
            Vector3Int endInt = new Vector3Int(Mathf.CeilToInt(end.x), Mathf.CeilToInt(end.y), Mathf.CeilToInt(end.z));
            Vector3Int diag = endInt - startInt;

            int maxLength = Mathf.Max(diag.x, diag.y, diag.z);
            endInt.x += maxLength - diag.x;
            endInt.y += maxLength - diag.y;
            endInt.z += maxLength - diag.z;

            Box box = new Box(startInt, endInt, _cellSize);

            return box;
        }


        public Vector3Int GetStartInt()
        {
            return new Vector3Int((int)Mathf.Floor(Start.x), (int)Mathf.Floor(Start.y), (int)Mathf.Floor(Start.z));
        }

        public Vector3Int GetEndInt()
        {
            return new Vector3Int((int)Mathf.Ceil(End.x), (int)Mathf.Ceil(End.y), (int)Mathf.Ceil(End.z));
        }

        //public Box(Vector3 _position, Vector3 _size)
        //{
        //    position = _position;
        //    size = _size;
        //}

        //public static Box CreateBox(Vector3 start, Vector3 end, float cellSize)
        //{
        //    Vector3 size = end - start;
        //    Vector3 pos = (end - start) / 2;
        //    pos = pos + start; 


        //    return new Box(pos, size);
        //}
    }

    public class Node
    {
        public Vector3 Position { get; private set; }
        public int weight { get; set; }

        public Node(Vector3 position)
        {
            Position = position;
            weight = 0;
        }
    }
}
