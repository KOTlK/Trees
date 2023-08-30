using System.Collections.Generic;
using UnityEngine;

namespace Trees.Runtime.OcTrees.View
{
    public abstract class OcTreeGizmos<T> : MonoBehaviour, IOcTreeView<T>
    {
        [SerializeField] protected Color BoundsColor = Color.green;
        [SerializeField] protected Color PointColor = Color.red;
        [SerializeField] protected float PointSize = 0.5f;

        protected readonly Queue<AABB> BoundsQueue = new();
        protected readonly Queue<Vector3> PointsQueue = new();
        protected readonly Queue<T> ItemsQueue = new();

        public virtual void DrawBounds(AABB aabb)
        {
            BoundsQueue.Enqueue(aabb);
        }

        public virtual void DrawPoint(Vector3 point)
        {
            PointsQueue.Enqueue(point);
        }

        public virtual void DrawValue(T item)
        {
            ItemsQueue.Enqueue(item);
        }

        protected void DisplayBounds()
        {
            Gizmos.color = BoundsColor;
            
            while (BoundsQueue.Count > 0)
            {
                var bounds = BoundsQueue.Dequeue();

                var point0 = bounds.Max;
                var point7 = bounds.Min;
                
                var point1 = new Vector3(point7.x, point0.y, point0.z);
                var point2 = new Vector3(point0.x, point7.y, point0.z);
                var point3 = new Vector3(point0.x, point0.y, point7.z);
                var point4 = new Vector3(point0.x, point7.y, point7.z);
                var point5 = new Vector3(point7.x, point0.y, point7.z);
                var point6 = new Vector3(point7.x, point7.y, point0.z);
                

                Gizmos.DrawLine(point0, point1);
                Gizmos.DrawLine(point0, point2);
                Gizmos.DrawLine(point0, point3);
                Gizmos.DrawLine(point7, point4);
                Gizmos.DrawLine(point7, point5);
                Gizmos.DrawLine(point7, point6);
                Gizmos.DrawLine(point1, point5);
                Gizmos.DrawLine(point2, point6);
                Gizmos.DrawLine(point1, point6);
                Gizmos.DrawLine(point2, point4);
                Gizmos.DrawLine(point3, point4);
                Gizmos.DrawLine(point3, point5);
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

        protected abstract void OnDrawGizmos();
    }
}