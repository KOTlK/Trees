using System.Collections.Generic;
using Trees.Runtime;
using Trees.Runtime.OcTrees;
using Trees.Runtime.OcTrees.View;

namespace Trees.Tests
{
    public class OcTreeTestView : IOcTreeView<int>
    {
        public readonly List<TreeElement<int>> Elements = new();
        
        public void DrawBounds(AABB aabb)
        {
        }

        public void DrawElement(TreeElement<int> element)
        {
            Elements.Add(element);
        }
    }
}