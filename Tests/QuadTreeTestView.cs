using System.Collections.Generic;
using System.Linq;
using Trees.Runtime.QuadTrees;
using Trees.Runtime.QuadTrees.View;
using UnityEngine;

namespace Trees.Tests
{
    public class QuadTreeTestView : IQuadTreeView<int>
    {
        public List<Vector3> Points;
        public List<int> Items;

        public QuadTreeTestView()
        {
            Points = new List<Vector3>();
            Items = new List<int>();
        }

        public void DrawBounds(Rectangle rectangle)
        {
        }

        public void DrawPoint(Vector3 point)
        {
            Points.Add(point);
        }

        public void DrawItem(int item)
        {
            Items.Add(item);
        }
    }
}