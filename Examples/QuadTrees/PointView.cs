using Trees.Runtime.QuadTrees.View;
using UnityEngine;

namespace Trees.Examples.QuadTrees
{
    public class PointView : QuadTreeGizmos<Point>
    {
        [SerializeField] private Color _directionColor = Color.blue;

        protected override void OnDrawGizmosSelected()
        {
            DisplayBounds();
            
            while (ElementsQueue.Count > 0)
            {
                var (position, point) = ElementsQueue.Dequeue();

                Gizmos.color = PointColor;
                Gizmos.DrawSphere(point.Position, PointSize);
                Gizmos.color = _directionColor;
                Gizmos.DrawRay(point.Position, point.Direction);
            }
        }
    }
}