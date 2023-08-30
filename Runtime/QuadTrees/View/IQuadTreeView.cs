using UnityEngine;

namespace Trees.Runtime.QuadTrees.View
{
    public interface IQuadTreeView<in T>
    {
        void DrawBounds(Rectangle rectangle);
        void DrawPoint(Vector3 point);
        void DrawItem(T item);
    }
}