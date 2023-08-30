using System.Collections.Generic;
using Trees.Runtime.OcTrees.View;
using UnityEngine;

namespace Trees.Runtime.OcTrees
{
    public struct OcTree<T>
    {
        private readonly OcTree<T>[] _child;
        private readonly TreeElement<T>[] _elements;
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

        public OcTree(AABB aabb, int capacity = 8)
        {
            _capacity = capacity;
            _aabb = aabb;
            _child = new OcTree<T>[8];
            _elements = new TreeElement<T>[capacity];
            _divided = false;
            _count = 0;
        }

        public bool Insert(TreeElement<T> element)
        {
            if (_aabb.Contains(element.Position) == false)
            {
                return false;
            }
            
            if (_count < _capacity)
            {
                _elements[_count] = element;
                _count++;
                return true;
            }

            if (_divided == false)
            {
                Divide();
            }

            if (_child[FUL].Insert(element))
                return true;
            if (_child[FUR].Insert(element))
                return true;
            if (_child[FDL].Insert(element))
                return true;
            if (_child[FDR].Insert(element))
                return true;
            if (_child[BUL].Insert(element))
                return true;
            if (_child[BUR].Insert(element))
                return true;
            if (_child[BDL].Insert(element))
                return true;
            if (_child[BDR].Insert(element))
                return true;

            return false;
        }

        public IEnumerable<TreeElement<T>> Query(AABB range)
        {
            var total = new List<TreeElement<T>>();

            if (_aabb.Intersect(range) == false)
                return total;

            for (var i = 0; i < _count; i++)
            {
                if (range.Contains(_elements[i].Position))
                {
                    total.Add(_elements[i]);
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

        public IEnumerable<TreeElement<T>> Query(Sphere range)
        {
            var total = new List<TreeElement<T>>();

            if (_aabb.Intersect(range) == false)
                return total;

            for (var i = 0; i < _count; i++)
            {
                if (range.Contains(_elements[i].Position))
                {
                    total.Add(_elements[i]);
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

            _child[FUL] = new OcTree<T>(new AABB(
                new Vector3(
                    _aabb.Position.x - _aabb.Size.x * 0.25f, 
                    _aabb.Position.y + _aabb.Size.y * 0.25f,
                    _aabb.Position.z + _aabb.Size.z * 0.25f),
                _aabb.Size * 0.5f), _capacity);
            _child[FUR] = new OcTree<T>(new AABB(
                new Vector3(
                    _aabb.Position.x + _aabb.Size.x * 0.25f, 
                    _aabb.Position.y + _aabb.Size.y * 0.25f,
                    _aabb.Position.z + _aabb.Size.z * 0.25f),
                _aabb.Size * 0.5f), _capacity);
            _child[FDL] = new OcTree<T>(new AABB(
                new Vector3(
                    _aabb.Position.x - _aabb.Size.x * 0.25f, 
                    _aabb.Position.y - _aabb.Size.y * 0.25f,
                    _aabb.Position.z + _aabb.Size.z * 0.25f),
                _aabb.Size * 0.5f), _capacity);
            _child[FDR] = new OcTree<T>(new AABB(
                new Vector3(
                    _aabb.Position.x + _aabb.Size.x * 0.25f, 
                    _aabb.Position.y - _aabb.Size.y * 0.25f,
                    _aabb.Position.z + _aabb.Size.z * 0.25f),
                _aabb.Size * 0.5f), _capacity);
            _child[BUL] = new OcTree<T>(new AABB(
                new Vector3(
                    _aabb.Position.x - _aabb.Size.x * 0.25f, 
                    _aabb.Position.y + _aabb.Size.y * 0.25f,
                    _aabb.Position.z - _aabb.Size.z * 0.25f),
                _aabb.Size * 0.5f), _capacity);
            _child[BUR] = new OcTree<T>(new AABB(
                new Vector3(
                    _aabb.Position.x + _aabb.Size.x * 0.25f, 
                    _aabb.Position.y + _aabb.Size.y * 0.25f,
                    _aabb.Position.z - _aabb.Size.z * 0.25f),
                _aabb.Size * 0.5f), _capacity);
            _child[BDL] = new OcTree<T>(new AABB(
                new Vector3(
                    _aabb.Position.x - _aabb.Size.x * 0.25f, 
                    _aabb.Position.y - _aabb.Size.y * 0.25f,
                    _aabb.Position.z - _aabb.Size.z * 0.25f),
                _aabb.Size * 0.5f), _capacity);
            _child[BDR] = new OcTree<T>(new AABB(
                new Vector3(
                    _aabb.Position.x + _aabb.Size.x * 0.25f, 
                    _aabb.Position.y - _aabb.Size.y * 0.25f,
                    _aabb.Position.z - _aabb.Size.z * 0.25f),
                _aabb.Size * 0.5f), _capacity);

            _divided = true;
        }

        public void Visualize(IOcTreeView<T> view)
        {
            view.DrawBounds(_aabb);

            for (var i = 0; i < _count; i++)
            {
                view.DrawElement(_elements[i]);
            }

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