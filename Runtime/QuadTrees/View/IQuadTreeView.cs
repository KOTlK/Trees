using System.Collections.Generic;
using UnityEngine;

namespace Trees.Runtime.QuadTrees.View
{
    public interface IQuadTreeView<in T>
    {
        void DrawBounds(Rectangle rectangle);
        void DrawPoints(IEnumerable<Vector3> points);
        void DrawItems(IEnumerable<T> items);
    }
}