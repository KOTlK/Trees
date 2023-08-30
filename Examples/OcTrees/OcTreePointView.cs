using Trees.Examples.QuadTrees;
using Trees.Runtime.OcTrees.View;
using UnityEngine;

namespace Trees.Examples.OcTrees
{
    public class OcTreePointView : OcTreeGizmos<Point>
    {
        [SerializeField] private Color _directionColor = Color.blue;

        protected override void OnDrawGizmos()
        {
            DisplayBounds();
            
            while (ElementsQueue.Count > 0)
            {
                var point = ElementsQueue.Dequeue();

                Gizmos.color = PointColor;
                Gizmos.DrawSphere(point.Position, PointSize);
                Gizmos.color = _directionColor;
                Gizmos.DrawRay(point.Position, point.Value.Direction);
            }
        }
    }
}