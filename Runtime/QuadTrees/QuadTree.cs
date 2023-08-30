﻿using System.Collections.Generic;
using Trees.Runtime.QuadTrees.View;
using UnityEngine;

namespace Trees.Runtime.QuadTrees
{
    public struct QuadTree<T>
    {
        private readonly int _capacity;
        private readonly Vector3[] _positions;
        private readonly QuadTree<T>[] _child;
        private readonly T[] _items;

        private Rectangle _rectangle;
        private bool _divided;
        private int _count;

        private const int NW = 0;
        private const int NE = 1;
        private const int SW = 2;
        private const int SE = 3;

        public QuadTree(int capacity, Rectangle rectangle)
        {
            _capacity = capacity;
            _rectangle = rectangle;
            _positions = new Vector3[_capacity];
            _count = 0;
            _child = new QuadTree<T>[4];
            _items = new T[4];
            _divided = false;
        }

        public bool Insert(Vector3 point, T item)
        {
            if (_rectangle.Contains(point) == false)
                return false;

            if (_count < _capacity)
            {
                _positions[_count] = point;
                _items[_count] = item;
                _count++;
                return true;
            }
            
            if (_divided == false)
                Divide();

            if (_child[NW].Insert(point, item))
                return true;

            if(_child[NE].Insert(point, item))
                return true;

            if(_child[SW].Insert(point, item))
                return true;

            if(_child[SE].Insert(point, item))
                return true;
            

            return false;
        }

        public IEnumerable<(Vector3, T)> Query(Rectangle range)
        {
            var total = new List<(Vector3, T)>();

            if (_rectangle.Intersect(range) == false)
                return total;

            for (var i = 0; i < _positions.Length; i++)
            {
                if (range.Contains(_positions[i]))
                {
                    total.Add((_positions[i], _items[i]));
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

        public IEnumerable<(Vector3, T)> Query(Circle range)
        {
            var total = new List<(Vector3, T)>();

            if (_rectangle.Intersect(range) == false)
                return total;

            for (var i = 0; i < _positions.Length; i++)
            {
                if (range.Contains(_positions[i]))
                {
                    total.Add((_positions[i], _items[i]));
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

            _child[NW] = new QuadTree<T>(_capacity, new Rectangle(
                new Vector3(_rectangle.Position.x - _rectangle.Size.x * 0.25f, _rectangle.Position.y + _rectangle.Size.y * 0.25f),
                _rectangle.Size * 0.5f));
            _child[NE] = new QuadTree<T>(_capacity, new Rectangle(
                new Vector3(_rectangle.Position.x + _rectangle.Size.x * 0.25f, _rectangle.Position.y + _rectangle.Size.y * 0.25f),
                _rectangle.Size * 0.5f));
            _child[SW] = new QuadTree<T>(_capacity, new Rectangle(
                new Vector3(_rectangle.Position.x - _rectangle.Size.x * 0.25f, _rectangle.Position.y - _rectangle.Size.y * 0.25f),
                _rectangle.Size * 0.5f));
            _child[SE] = new QuadTree<T>(_capacity, new Rectangle(
                new Vector3(_rectangle.Position.x + _rectangle.Size.x * 0.25f, _rectangle.Position.y - _rectangle.Size.y * 0.25f),
                _rectangle.Size * 0.5f));
            
            _divided = true;
        }

        public void Visualize(IQuadTreeView<T> view)
        {
            view.DrawBounds(_rectangle);
            view.DrawPoints(_positions);
            view.DrawItems(_items);
            
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