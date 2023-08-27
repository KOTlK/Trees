using UnityEngine;

namespace Trees.Runtime.QuadTrees
{
    public struct Rectangle
    {
        public Vector2 Position;
        public Vector2 Size;

        public Rectangle(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        public Vector2 HalfExtents => Size * 0.5f;

        public bool Contains(Vector2 point)
        {
            var halfExtents = HalfExtents;
            
            if (point.x > Position.x + halfExtents.x)
            {
                return false;
            }

            if (point.y > Position.x + halfExtents.x)
            {
                return false;
            }

            if (point.x < Position.x - halfExtents.x)
            {
                return false;
            }

            if (point.y < Position.y - halfExtents.y)
            {
                return false;
            }

            return true;
        }
        
        public bool Intersect(Rectangle rectangle)
        {
            return Position.x + Size.x >= rectangle.Position.x &&
                   rectangle.Position.x + rectangle.Size.x >= Position.x &&
                   Position.y + Size.y >= rectangle.Position.y &&
                   rectangle.Position.y + rectangle.Size.y >= Position.y;
        }
    }
}