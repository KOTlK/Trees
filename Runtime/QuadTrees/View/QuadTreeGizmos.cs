using System.Collections.Generic;
using UnityEngine;

namespace Trees.Runtime.QuadTrees.View
{
    public abstract class QuadTreeGizmos<T> : MonoBehaviour, IQuadTreeView<T>
    {
        [SerializeField] protected Color BoundsColor = Color.green;
        [SerializeField] protected Color PointColor = Color.red;
        [SerializeField] protected float PointSize = 0.5f;

        protected readonly Queue<Rectangle> BoundsQueue = new();
        protected readonly Queue<Vector3> PointsQueue = new();
        protected readonly Queue<T> ItemsQueue = new();

        public virtual void DrawBounds(Rectangle rectangle)
        {
            BoundsQueue.Enqueue(rectangle);
        }

        public virtual void DrawPoint(Vector3 point)
        {
            PointsQueue.Enqueue(point);
        }

        public virtual void DrawItem(T item)
        {
            ItemsQueue.Enqueue(item);
        }

        protected void DisplayBounds()
        {
            Gizmos.color = BoundsColor;
            
            while (BoundsQueue.Count > 0)
            {
                var bounds = BoundsQueue.Dequeue();

                var halfExtents = bounds.HalfExtents;

                var point0 = bounds.Position + new Vector3(-halfExtents.x, halfExtents.y);
                var point1 = bounds.Position + new Vector3(halfExtents.x, halfExtents.y);
                var point2 = bounds.Position + new Vector3(halfExtents.x, -halfExtents.y);
                var point3 = bounds.Position + new Vector3(-halfExtents.x, -halfExtents.y);
                
                Gizmos.DrawLine(point0, point1);
                Gizmos.DrawLine(point1, point2);
                Gizmos.DrawLine(point2, point3);
                Gizmos.DrawLine(point3, point0);
            }
        }

        protected void DisplayPoints()
        {
            Gizmos.color = PointColor;
            
            while (PointsQueue.Count > 0)
            {
                var point = PointsQueue.Dequeue();

                Gizmos.DrawSphere(point, PointSize);
            }
        }

        protected abstract void OnDrawGizmosSelected();
    }
}