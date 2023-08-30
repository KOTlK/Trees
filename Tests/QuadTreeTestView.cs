using System.Collections.Generic;
using Trees.Runtime.QuadTrees;
using Trees.Runtime.QuadTrees.View;

namespace Trees.Tests
{
    public class QuadTreeTestView : IQuadTreeView<int>
    {
        public List<TreeElement<int>> Elements = new();

        public void DrawBounds(Rectangle rectangle)
        {
        }

        public void DrawElement(TreeElement<int> element)
        {
            Elements.Add(element);
        }
    }
}