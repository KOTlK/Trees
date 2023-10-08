using System.Collections.Generic;
using Trees.Runtime;
using Trees.Runtime.QuadTrees;
using Trees.Runtime.QuadTrees.View;
using UnityEngine;

namespace Trees.Tests
{
    public class QuadTreeTestView : IQuadTreeView<int>
    {
        public List<(Vector3, int)> Elements = new();

        public void DrawBounds(Rectangle rectangle)
        {
        }

        public void DrawElement(Vector3 position, int element)
        {
            Elements.Add((position, element));
        }

        public void DrawElementsCount(int count)
        {
            
        }
    }
}