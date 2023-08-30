using UnityEngine;

namespace Trees.Runtime.OcTrees.View
{
    public interface IOcTreeView<in T>
    {
        void DrawBounds(AABB aabb);
        void DrawPoint(Vector3 point);
        void DrawValue(T value);
    }
}