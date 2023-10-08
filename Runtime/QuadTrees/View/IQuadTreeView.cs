using UnityEngine;

namespace Trees.Runtime.QuadTrees.View
{
    public interface IQuadTreeView<in T>
    {
        void DrawBounds(Rectangle rectangle);
        void DrawElement(Vector3 position, T element);
        void DrawElementsCount(int count);
    }
}