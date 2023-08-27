using Trees.Runtime.QuadTrees;
using Trees.Runtime.QuadTrees.View;
using Unity.Profiling;
using UnityEngine;

namespace Trees.Examples.QuadTrees
{
    public class Field : MonoBehaviour
    {
        [SerializeField] private Vector2 _startPosition = Vector2.zero;
        [SerializeField] private Vector2 _size = new Vector2(100, 100);
        [SerializeField] private int _pointsAmount = 100;
        [SerializeField] private IntView _view;
        [SerializeField] private int _count;
        [SerializeField] private Vector2 _areaSize = new Vector2(10, 10);
        [SerializeField] private LineRenderer _areaRenderer;
        [SerializeField] private bool _displayDebug = true;

        private Vector3 _areaPosition;
        private bool _movingArea = false;
        private QuadTree<int> _quadTree;
        private readonly System.Random _random = new();

        private static readonly ProfilerMarker QuadTreeQuery = new(nameof(QuadTreeQuery));

        private void Awake()
        {
            _quadTree = new QuadTree<int>(4, new Rectangle(_startPosition, _size));
            InsertPoints(_pointsAmount);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                InsertPoints(_pointsAmount);
            }

            var area = new Rectangle(_areaPosition, _areaSize);
            
            //Draw area
            var halfExtents = area.HalfExtents;

            var point0 = area.Position + new Vector2(-halfExtents.x, halfExtents.y);
            var point1 = area.Position + new Vector2(halfExtents.x, halfExtents.y);
            var point2 = area.Position + new Vector2(halfExtents.x, -halfExtents.y);
            var point3 = area.Position + new Vector2(-halfExtents.x, -halfExtents.y);

            _areaRenderer.positionCount = 5;
            _areaRenderer.SetPositions(new Vector3[]
            {
                point0,
                point1,
                point2,
                point3,
                point0
            });

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
                if (area.Contains(mousePosition))
                {
                    _movingArea = true;
                }
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                _movingArea = false;
            }

            if (_movingArea)
            {
                var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                _areaPosition = mousePosition;
            }

            QuadTreeQuery.Begin();
            var pointsInArea = _quadTree.Query(area);
            QuadTreeQuery.End();

            foreach (var (point, _) in pointsInArea)
            {
                Debug.DrawLine(area.Position, point);
            }
            
            if(_displayDebug)
                _quadTree.Visualize(_view);
        }

        private void InsertPoints(int amount)
        {
            var min = _startPosition - _size * 0.5f;
            var max = _startPosition + _size * 0.5f;
            
            for (var i = 0; i < amount; i++)
            {
                var x = _random.Next((int)min.x, (int)max.x);
                var y = _random.Next((int)min.y, (int)max.y);


                _quadTree.Insert(new Vector2(x, y), _count);
                _count++;
            }
        }
    }
}