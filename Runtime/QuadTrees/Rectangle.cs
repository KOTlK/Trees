using System;
using UnityEngine;

namespace Trees.Runtime.QuadTrees
{
    public struct Rectangle
    {
        public Vector3 Position;
        public Vector3 Size;

        public Rectangle(Vector3 position, Vector3 size)
        {
            Position = position;
            Size = size;
        }

        public Vector3 HalfExtents => Size * 0.5f;

        public bool Contains(Vector3 point)
        {
            var halfExtents = HalfExtents;

            if (Position.x + halfExtents.x < point.x)
                return false;

            if (Position.y + halfExtents.y < point.y)
                return false;

            if (Position.x - halfExtents.x > point.x)
                return false;

            if (Position.y - halfExtents.y > point.y)
                return false;

            return true;
        }
        
        public bool Intersect(Rectangle rectangle)
        {
            var half = HalfExtents;
            var targetHalf = rectangle.HalfExtents;
            
            var dx = rectangle.Position.x - Position.x;
            var px = (targetHalf.x + half.x) - Math.Abs(dx);
            if (px <= 0) 
            {
                return false;
            }

            var dy = rectangle.Position.y - Position.y;
            var py = (targetHalf.y + half.y) - Math.Abs(dy);
            if (py <= 0) 
            {
                return false;
            }


            return true;
        }

        public bool Intersect(Circle circle)
        {
            var difference = circle.Position - Position;
            var halfExtents = HalfExtents;
            var clamped = Clamp(difference, -halfExtents, halfExtents);
            var closest = Position + clamped;
            difference = closest - circle.Position;

            return difference.sqrMagnitude <= circle.Radius * circle.Radius;
        }
        
        private static Vector3 Clamp(Vector3 vector, Vector3 min, Vector3 max)
        {
            return new Vector3(Mathf.Clamp(vector.x, min.x, max.x), Mathf.Clamp(vector.y, min.y, max.y));
        }
    }
}