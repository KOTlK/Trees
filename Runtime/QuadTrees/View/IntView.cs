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
            
            while (ElementsQueue.Count > 0)
            {
                var point = ElementsQueue.Dequeue();

                Gizmos.DrawSphere(point.Position, PointSize);
                Handles.Label(point.Position + new Vector3(0, _textOffset), point.Value.ToString());
            }
#endif
        }
    }
}