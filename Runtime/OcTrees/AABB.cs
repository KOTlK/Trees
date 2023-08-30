using UnityEngine;

namespace Trees.Runtime.OcTrees
{
    public struct AABB
    {
        public Vector3 Position;
        public Vector3 Size;

        public AABB(Vector3 position, Vector3 size)
        {
            Position = position;
            Size = size;
        }

        public Vector3 HalfExtents => Size * 0.5f;

        public Vector3 Max => Position + HalfExtents;
        public Vector3 Min => Position - HalfExtents;

        public bool Contains(Vector3 point)
        {
            var halfExtents = HalfExtents;

            if (Position.x + halfExtents.x < point.x)
                return false;

            if (Position.y + halfExtents.y < point.y)
                return false;

            if (Position.z + halfExtents.z < point.z)
                return false;

            if (Position.x - halfExtents.x > point.x)
                return false;

            if (Position.y - halfExtents.y > point.y)
                return false;

            if (Position.z - halfExtents.z > point.z)
                return false;

            return true;
        }

        public bool Intersect(AABB aabb)
        {
            var min = Min;
            var max = Max;
            var targetMin = aabb.Min;
            var targetMax = aabb.Max;
            
            return 
                min.x <= targetMax.x &&
                max.x >= targetMin.x &&
                min.y <= targetMax.y &&
                max.y >= targetMin.y &&
                min.z <= targetMax.z &&
                max.z >= targetMin.z
            ;
        }

        public bool Intersect(Sphere sphere)
        {
            var dMin = 0f;
            var r2 = sphere.Radius * sphere.Radius;
            var bMin = Min;
            var bMax = Max;

            for(var i = 0; i < 3; i++) {
                if(sphere.Position[i] < bMin[i])
                    dMin += Mathf.Pow(sphere.Position[i] - bMin[i], 2); 
                else if(sphere.Position[i] > bMax[i])
                    dMin += Mathf.Pow(sphere.Position[i] - bMax[i], 2);     
            }

            return dMin <= r2;
        }
    }
}