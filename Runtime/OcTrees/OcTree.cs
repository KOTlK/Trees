using System.Collections.Generic;
using Trees.Runtime.OcTrees.View;
using UnityEngine;

namespace Trees.Runtime.OcTrees
{
    public struct OcTree<T>
    {
        private readonly OcTree<T>[] _child;
        private readonly T[] _values;
        private readonly Vector3[] _points;
        private readonly int _capacity;
        
        private AABB _aabb;
        private bool _divided;
        private int _count;

        //Forward Up Left etc.
        private const int FUL = 0;
        private const int FUR = 1;
        private const int FDL = 2;
        private const int FDR = 3;
        private const int BUL = 4;
        private const int BUR = 5;
        private const int BDL = 6;
        private const int BDR = 7;

        public OcTree(int capacity, AABB aabb)
        {
            _capacity = capacity;
            _aabb = aabb;
            _child = new OcTree<T>[8];
            _values = new T[capacity];
            _points = new Vector3[capacity];
            _divided = false;
            _count = 0;
        }

        public bool Insert(Vector3 point, T value)
        {
            if (_aabb.Contains(point) == false)
            {
                return false;
            }
            
            if (_count < _capacity)
            {
                _points[_count] = point;
                _values[_count] = value;
                _count++;
                return true;
            }

            if (_divided == false)
            {
                Divide();
            }

            if (_child[FUL].Insert(point, value))
                return true;
            if (_child[FUR].Insert(point, value))
                return true;
            if (_child[FDL].Insert(point, value))
                return true;
            if (_child[FDR].Insert(point, value))
                return true;
            if (_child[BUL].Insert(point, value))
                return true;
            if (_child[BUR].Insert(point, value))
                return true;
            if (_child[BDL].Insert(point, value))
                return true;
            if (_child[BDR].Insert(point, value))
                return true;

            return false;
        }

        public IEnumerable<(Vector3, T)> Query(AABB range)
        {
            var total = new List<(Vector3, T)>();

            if (_aabb.Intersect(range) == false)
                return total;

            for (var i = 0; i < _points.Length; i++)
            {
                if (range.Contains(_points[i]))
                {
                    total.Add((_points[i], _values[i]));
                }
            }

            if (_divided == false)
                return total;

            foreach (var child in _child)
            {
                total.AddRange(child.Query(range));
            }

            return total;
        }

        public IEnumerable<(Vector3, T)> Query(Sphere range)
        {
            var total = new List<(Vector3, T)>();

            if (_aabb.Intersect(range) == false)
                return total;

            for (var i = 0; i < _points.Length; i++)
            {
                if (range.Contains(_points[i]))
                {
                    total.Add((_points[i], _values[i]));
                }
            }

            if (_divided == false)
                return total;

            foreach (var child in _child)
            {
                total.AddRange(child.Query(range));
            }

            return total;
        }

        public void Divide()
        {
            if (_divided)
                return;

            _child[FUL] = new OcTree<T>(_capacity, new AABB(
                new Vector3(
                    _aabb.Position.x - _aabb.Size.x * 0.25f, 
                    _aabb.Position.y + _aabb.Size.y * 0.25f,
                    _aabb.Position.z + _aabb.Size.z * 0.25f),
                _aabb.Size * 0.5f));
            _child[FUR] = new OcTree<T>(_capacity, new AABB(
                new Vector3(
                    _aabb.Position.x + _aabb.Size.x * 0.25f, 
                    _aabb.Position.y + _aabb.Size.y * 0.25f,
                    _aabb.Position.z + _aabb.Size.z * 0.25f),
                _aabb.Size * 0.5f));
            _child[FDL] = new OcTree<T>(_capacity, new AABB(
                new Vector3(
                    _aabb.Position.x - _aabb.Size.x * 0.25f, 
                    _aabb.Position.y - _aabb.Size.y * 0.25f,
                    _aabb.Position.z + _aabb.Size.z * 0.25f),
                _aabb.Size * 0.5f));
            _child[FDR] = new OcTree<T>(_capacity, new AABB(
                new Vector3(
                    _aabb.Position.x + _aabb.Size.x * 0.25f, 
                    _aabb.Position.y - _aabb.Size.y * 0.25f,
                    _aabb.Position.z + _aabb.Size.z * 0.25f),
                _aabb.Size * 0.5f));
            _child[BUL] = new OcTree<T>(_capacity, new AABB(
                new Vector3(
                    _aabb.Position.x - _aabb.Size.x * 0.25f, 
                    _aabb.Position.y + _aabb.Size.y * 0.25f,
                    _aabb.Position.z - _aabb.Size.z * 0.25f),
                _aabb.Size * 0.5f));
            _child[BUR] = new OcTree<T>(_capacity, new AABB(
                new Vector3(
                    _aabb.Position.x + _aabb.Size.x * 0.25f, 
                    _aabb.Position.y + _aabb.Size.y * 0.25f,
                    _aabb.Position.z - _aabb.Size.z * 0.25f),
                _aabb.Size * 0.5f));
            _child[BDL] = new OcTree<T>(_capacity, new AABB(
                new Vector3(
                    _aabb.Position.x - _aabb.Size.x * 0.25f, 
                    _aabb.Position.y - _aabb.Size.y * 0.25f,
                    _aabb.Position.z - _aabb.Size.z * 0.25f),
                _aabb.Size * 0.5f));
            _child[BDR] = new OcTree<T>(_capacity, new AABB(
                new Vector3(
                    _aabb.Position.x + _aabb.Size.x * 0.25f, 
                    _aabb.Position.y - _aabb.Size.y * 0.25f,
                    _aabb.Position.z - _aabb.Size.z * 0.25f),
                _aabb.Size * 0.5f));

            _divided = true;
        }

        public void Visualize(IOcTreeView<T> view)
        {
            view.DrawBounds(_aabb);
            view.DrawPoints(_points);
            view.DrawValues(_values);

            if (_divided == false)
            {
                return;
            }
            
            foreach (var child in _child)
            {
                child.Visualize(view);
            }
        }
    }
}