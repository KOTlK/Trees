﻿using System;
using System.Collections.Generic;
using TMPro;
using Trees.Runtime.QuadTrees;
using UnityEngine;

namespace Trees.Examples.QuadTrees
{
    public class DynamicField : MonoBehaviour
    {
        [SerializeField] private TMP_Text _debugInfo;
        [SerializeField] private Vector3 _startPosition = Vector3.zero;
        [SerializeField] private Vector3 _size = new Vector3(100, 100);
        [SerializeField] private int _pointsAmount = 100;
        [SerializeField] private float _pointsSpeed = 5f;
        [SerializeField] private PointView _view;
        [SerializeField] private int _count;
        [SerializeField] private Vector3 _areaSize = new Vector3(10, 10);
        [SerializeField] private AreaType _areaType = AreaType.Rectangle;
        [SerializeField] private float _circleAreaRadius = 10f;
        [SerializeField] private int _circleSegmentsCount = 30;
        [SerializeField] private LineRenderer _areaRenderer;
        [SerializeField] private bool _displayDebug = true;
        
        private DynamicQuadTree<Point> _quadTree;
        private Vector3 _areaPosition;
        private bool _movingArea = false;
        private readonly System.Random _random = new();
        private TreeItem<Point>[] _points;
        
        private void Awake()
        {
            _quadTree = new DynamicQuadTree<Point>(new Rectangle(_startPosition, _size));
            InsertPoints(_pointsAmount);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                InsertPoints(_pointsAmount);
            }

            var rectangleArea = new Rectangle(_areaPosition, _areaSize);
            var circleArea = new Circle(_areaPosition, _circleAreaRadius);

            switch (_areaType)
            {
                case AreaType.Circle:
                    var angle = 0f;
                    var anglePerSegment = 180f / _circleSegmentsCount;
                    _areaRenderer.positionCount = _circleSegmentsCount;
                    for (var i = 0; i < _circleSegmentsCount; i++)
                    {
                        _areaRenderer.SetPosition(i, _areaPosition + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * circleArea.Radius);
                        angle += anglePerSegment;
                    }
                    
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        mousePosition.z = 0;
                
                        if (circleArea.Contains(mousePosition))
                        {
                            _movingArea = true;
                        }
                    }
                    break;
                case AreaType.Rectangle:
                    //Draw area
                    var halfExtents = rectangleArea.HalfExtents;

                    var point0 = rectangleArea.Position + new Vector3(-halfExtents.x, halfExtents.y);
                    var point1 = rectangleArea.Position + new Vector3(halfExtents.x, halfExtents.y);
                    var point2 = rectangleArea.Position + new Vector3(halfExtents.x, -halfExtents.y);
                    var point3 = rectangleArea.Position + new Vector3(-halfExtents.x, -halfExtents.y);

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
                        mousePosition.z = 0;
                
                        if (rectangleArea.Contains(mousePosition))
                        {
                            _movingArea = true;
                        }
                    }
                    break;
                case AreaType.Point:
                    _areaRenderer.positionCount = 0;
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        _movingArea = true;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                _movingArea = false;
            }

            if (_movingArea)
            {
                var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;

                _areaPosition = mousePosition;
            }
            
            if (_areaType == AreaType.Point)
            {
                Debug.DrawRay(_areaPosition, Vector3.up, Color.cyan);
            }
            
            //Move points
            var max = (_startPosition + _size * 0.5f) - Vector3.one * 5f;
            var min = (_startPosition - _size * 0.5f) + Vector3.one * 5f;

            for (var i = 0; i < _points.Length; i++)
            {
                var position = _points[i].Value.Position;

                if (position.x <= min.x ||
                    position.x >= max.x ||
                    position.y <= min.y ||
                    position.y >= max.y)
                {
                    _points[i].Value.Direction *= -1;
                }

                _points[i].Value.Position += _points[i].Value.Direction * (_pointsSpeed * Time.deltaTime);
                _points[i].Position = _points[i].Value.Position;
            }

            _quadTree.Update();
            
            IEnumerable<TreeItem<Point>> pointsInArea = new TreeItem<Point>[1];
            var closestPoint = new TreeItem<Point>(new Point(), Vector3.negativeInfinity);
            
            switch (_areaType)
            {
                case AreaType.Circle:
                    pointsInArea = _quadTree.Query(circleArea);
                    break;
                case AreaType.Rectangle:
                    pointsInArea = _quadTree.Query(rectangleArea);
                    break;
                case AreaType.Point:
                    _quadTree.Query(_areaPosition, ref closestPoint);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_areaType == AreaType.Point)
            {
                Debug.DrawLine(_areaPosition, closestPoint.Position);
            }
            else
            {
                foreach (var element in pointsInArea)
                {
                    Debug.DrawLine(_areaPosition, element.Position);
                }
            }

            if(_displayDebug)
                _quadTree.Visualize(_view);

            _debugInfo.text = $"Items Count: {_count.ToString()}\n" +
                              $"Frame Time: {Time.deltaTime.ToString()}\n" +
                              $"FPS: {(1 / Time.unscaledDeltaTime).ToString()}" +
                              $"Items Inside QT: {_quadTree.Count().ToString()}";
        }
        
        private void InsertPoints(int amount)
        {
            if (_points == null)
            {
                _points = new TreeItem<Point>[amount];
            }
            else
            {
                Array.Resize(ref _points, _points.Length + amount);
            }


            var min = _startPosition - _size * 0.5f + Vector3.one * 5f;
            var max = _startPosition + _size * 0.5f - Vector3.one * 5f;
            
            for (var i = 0; i < amount; i++)
            {
                var x = _random.Next((int)min.x, (int)max.x);
                var y = _random.Next((int)min.y, (int)max.y);

                var direction = UnityEngine.Random.insideUnitCircle.normalized;
                
                var point = new Point()
                {
                    Position = new Vector2(x, y),
                    Direction = direction
                };

                _points[_count] = new TreeItem<Point>(point, point.Position);
                _quadTree.Insert(_points[_count]);
                _count++;
            }
        }
    }
}