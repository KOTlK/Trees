using System.Collections.Generic;
using UnityEngine;

namespace Trees.Runtime.OcTrees.View
{
    public interface IOcTreeView<in T>
    {
        void DrawBounds(AABB aabb);
        void DrawPoints(IEnumerable<Vector3> points);
        void DrawValues(IEnumerable<T> values);
    }
}