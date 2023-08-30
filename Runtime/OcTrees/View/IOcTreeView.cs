namespace Trees.Runtime.OcTrees.View
{
    public interface IOcTreeView<T>
    {
        void DrawBounds(AABB aabb);
        void DrawElement(TreeElement<T> element);
    }
}