using System;
using UnityEngine;

namespace Trees.Runtime.OcTrees
{
    public struct Sphere
    {
        public Vector3 Position;
        public float Radius;

        public Sphere(Vector3 position, float radius)
        {
            Position = position;
            Radius = radius;
        }

        public bool Contains(Vector3 point)
        {
            return (point - Position).sqrMagnitude < Radius * Radius;
        }

        public bool Intersect(Sphere sphere)
        {
            var distance = (sphere.Position - Position).sqrMagnitude;
            var rad = sphere.Radius * sphere.Radius + Radius * Radius;

            return distance < rad;
        }

        public bool Intersect(AABB aabb)
        {
            var dMin = 0f;
            var r2 = Radius * Radius;
            var bMin = aabb.Min;
            var bMax = aabb.Max;

            for(var i = 0; i < 3; i++) {
                if(Position[i] < bMin[i])
                    dMin += Mathf.Pow(Position[i] - bMin[i], 2); 
                else if(Position[i] > bMax[i])
                    dMin += Mathf.Pow(Position[i] - bMax[i], 2);     
            }

            return dMin <= r2;
        }
    }
}