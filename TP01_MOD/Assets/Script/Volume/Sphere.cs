using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Volume
{
    [Serializable]
    public class Sphere
    {
        [Range(0,10)] public float radius;
        public Vector3 center;
        public EType type;
        public float materialFieldForce;

        public enum EType
        {
            Fill,
            Extrusion,
        }


    }

}