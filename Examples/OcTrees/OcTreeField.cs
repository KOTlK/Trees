using System;
using Trees.Examples.QuadTrees;
using Trees.Runtime;
using Trees.Runtime.OcTrees;
using UnityEngine;

namespace Trees.Examples.OcTrees
{
    public class OcTreeField : MonoBehaviour
    {
        [SerializeField] private Vector3 _startPosition = Vector3.zero;
        [SerializeField] private Vector3 _size = new Vector3(100, 100, 100);
        [SerializeField] private int _pointsAmount = 100;
        [SerializeField] private float _pointsSpeed = 5f;
        [SerializeField] private OcTreePointView _view;
        [SerializeField] private bool _drawDebug = true;
        [SerializeField] private Transform _range;
        [SerializeField] private float _rangeRadius = 5f;
        [SerializeField] private Vector3 _rangeSize = new Vector3(10, 10, 10);
        [SerializeField] private AreaType _areaType = AreaType.Rectangle;
        
        private OcTree<Point> _ocTree;
        private readonly System.Random _random = new();
        private Point[] _points;
        private int _count = 0;

        private void Awake()
        {
            _ocTree = new OcTree<Point>(new AABB(_startPosition, _size));
            InsertPoints(_pointsAmount);
        }

        private void Update()
        {
            var max = (_startPosition + _size * 0.5f) - Vector3.one;
            var min = (_startPosition - _size * 0.5f) + Vector3.one;

            for (var i = 0; i < _points.Length; i++)
            {
                var position = _points[i].Position;

                if (position.x <= min.x ||
                    position.x >= max.x ||
                    position.y <= min.y ||
                    position.y >= max.y)
                {
                    _points[i].Direction *= -1;
                }

                _points[i].Position += _points[i].Direction * (_pointsSpeed * Time.deltaTime);
            }
            
            _ocTree = new OcTree<Point>(new AABB(_startPosition, _size));

            foreach (var point in _points)
            {
                _ocTree.Insert(new TreeElement<Point>(point.Position, point));
            }

            var rangePosition = _range.position;

            switch (_areaType)
            {
                case AreaType.Circle:
                    var sphereRange = new Sphere(rangePosition, _rangeRadius);
                    
                    var points = _ocTree.Query(sphereRange);

                    foreach (var point in points)
                    {
                        Debug.DrawLine(rangePosition, point.Position, Color.red);
                    }
                    break;
                case AreaType.Rectangle:
                    var aabbRange = new AABB(rangePosition, _rangeSize);
                    var pointsInRange = _ocTree.Query(aabbRange);

                    foreach (var point in pointsInRange)
                    {
                        Debug.DrawLine(rangePosition, point.Position, Color.red);
                    }
                    break;
                case AreaType.Point:
                    var closestPoint = new TreeElement<Point>();

                    _ocTree.Query(rangePosition, ref closestPoint);

                    Debug.DrawLine(rangePosition, closestPoint.Position, Color.red);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if(_drawDebug)
                _ocTree.Visualize(_view);
        }

        private void InsertPoints(int amount)
        {
            if (_points == null)
            {
                _points = new Point[amount];
            }
            else
            {
                Array.Resize(ref _points, _points.Length + amount);
            }


            var min = _startPosition - _size * 0.5f;
            var max = _startPosition + _size * 0.5f;
            
            for (var i = 0; i < amount; i++)
            {
                var x = _random.Next((int)min.x, (int)max.x);
                var y = _random.Next((int)min.y, (int)max.y);
                var z = _random.Next((int)min.z, (int)max.z);

                var direction = UnityEngine.Random.onUnitSphere;
                
                var point = new Point()
                {
                    Position = new Vector3(x, y, z),
                    Direction = direction
                };

                _points[_count] = point;
                _count++;
            }
        }

        private void OnDrawGizmos()
        {
            if (_drawDebug == false)
                return;
            
            switch (_areaType)
            {
                case AreaType.Circle:
                    Gizmos.DrawSphere(_range.position, _rangeRadius);
                    break;
                case AreaType.Rectangle:
                    var range = new AABB(_range.position, _rangeSize);
                    
                    var point0 = range.Max;
                    var point7 = range.Min;
                
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
                    break;
                case AreaType.Point:
                    Gizmos.DrawSphere(_range.position, 0.1f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
