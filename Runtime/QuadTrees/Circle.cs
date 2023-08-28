using UnityEngine;

namespace Trees.Runtime.QuadTrees
{
    public struct Circle
    {
        public Vector3 Position;
        public float Radius;

        public Circle(Vector3 position, float radius)
        {
            Position = position;
            Radius = radius;
        }

        public bool Contains(Vector3 point)
        {
            var direction = point - Position;

            return direction.sqrMagnitude < Radius * Radius;
        }
    }
}