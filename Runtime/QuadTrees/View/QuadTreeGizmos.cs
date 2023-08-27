using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Trees.Runtime.QuadTrees.View
{
    public abstract class QuadTreeGizmos<T> : MonoBehaviour, IQuadTreeView<T>
    {
        [SerializeField] private Color _boundsColor = Color.green;
        [SerializeField] private Color _pointColor = Color.red;
        [SerializeField] private Color _textColor = Color.yellow;
        [SerializeField] private float _pointSize = 0.5f;
        [SerializeField] private float _textOffset = 0.5f;
        
        private readonly Queue<Rectangle> _boundsQueue = new();
        private readonly Queue<Vector2> _pointsQueue = new();
        private readonly Queue<T> _items = new();

        public void DrawBounds(Rectangle rectangle)
        {
            _boundsQueue.Enqueue(rectangle);
        }

        public void DrawPoints(IEnumerable<Vector2> points)
        {
            foreach (var point in points)
            {
                _pointsQueue.Enqueue(point);
            }
        }

        public void DrawItems(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                _items.Enqueue(item);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _boundsColor;
            
            while (_boundsQueue.Count > 0)
            {
                var bounds = _boundsQueue.Dequeue();

                var halfExtents = bounds.HalfExtents;

                var point0 = bounds.Position + new Vector2(-halfExtents.x, halfExtents.y);
                var point1 = bounds.Position + new Vector2(halfExtents.x, halfExtents.y);
                var point2 = bounds.Position + new Vector2(halfExtents.x, -halfExtents.y);
                var point3 = bounds.Position + new Vector2(-halfExtents.x, -halfExtents.y);

                Gizmos.DrawLine(point0, point1);
                Gizmos.DrawLine(point1, point2);
                Gizmos.DrawLine(point2, point3);
                Gizmos.DrawLine(point3, point0);
            }

            Gizmos.color = _pointColor;
            Handles.color = _textColor;
            
            while (_pointsQueue.Count > 0)
            {
                var point = _pointsQueue.Dequeue();
                var item = _items.Dequeue();

                Gizmos.DrawSphere(point, _pointSize);
                Handles.Label(point + new Vector2(0, _textOffset), item.ToString());
            }
        }
    }
}