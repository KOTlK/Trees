using UnityEditor;
using UnityEngine;

namespace Trees.Runtime.QuadTrees.View
{
    public class IntView : QuadTreeGizmos<int>
    {
        [SerializeField] private Color _textColor = Color.yellow;
        [SerializeField] private float _textOffset = 0.5f;
        
        protected override void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            DisplayBounds();

            Gizmos.color = PointColor;
            Handles.color = _textColor;
            
            while (PointsQueue.Count > 0)
            {
                var point = PointsQueue.Dequeue();
                var item = ItemsQueue.Dequeue();

                Gizmos.DrawSphere(point, PointSize);
                Handles.Label(point + new Vector3(0, _textOffset), item.ToString());
            }
#endif
        }
    }
}