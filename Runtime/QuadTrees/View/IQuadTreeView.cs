using UnityEngine;

namespace Trees.Runtime.QuadTrees.View
{
    public interface IQuadTreeView<T>
    {
        void DrawBounds(Rectangle rectangle);
        void DrawElement(TreeElement<T> element);
    }
}