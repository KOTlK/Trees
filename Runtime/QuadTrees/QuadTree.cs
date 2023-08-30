using System.Collections.Generic;
using Trees.Runtime.QuadTrees.View;
using UnityEngine;

namespace Trees.Runtime.QuadTrees
{
    public struct QuadTree<T>
    {
        private readonly int _capacity;
        private readonly TreeElement<T>[] _elements;
        private readonly QuadTree<T>[] _child;

        private Rectangle _rectangle;
        private bool _divided;
        private int _count;

        private const int NW = 0;
        private const int NE = 1;
        private const int SW = 2;
        private const int SE = 3;

        public QuadTree(Rectangle rectangle, int capacity = 4)
        {
            _capacity = capacity;
            _rectangle = rectangle;
            _count = 0;
            _child = new QuadTree<T>[4];
            _divided = false;
            _elements = new TreeElement<T>[_capacity];
        }

        public bool Insert(TreeElement<T> element)
        {
            if (_rectangle.Contains(element.Position) == false)
                return false;

            if (_count < _capacity)
            {
                _elements[_count] = element;
                _count++;
                return true;
            }
            
            if (_divided == false)
                Divide();

            if (_child[NW].Insert(element))
                return true;

            if(_child[NE].Insert(element))
                return true;

            if(_child[SW].Insert(element))
                return true;

            if(_child[SE].Insert(element))
                return true;
            

            return false;
        }

        public IEnumerable<TreeElement<T>> Query(Rectangle range)
        {
            var total = new List<TreeElement<T>>();

            if (_rectangle.Intersect(range) == false)
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

        public IEnumerable<TreeElement<T>> Query(Circle range)
        {
            var total = new List<TreeElement<T>>();

            if (_rectangle.Intersect(range) == false)
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

            _child[NW] = new QuadTree<T>(new Rectangle(
                new Vector3(_rectangle.Position.x - _rectangle.Size.x * 0.25f, _rectangle.Position.y + _rectangle.Size.y * 0.25f),
                _rectangle.Size * 0.5f),
                _capacity);
            _child[NE] = new QuadTree<T>(new Rectangle(
                new Vector3(_rectangle.Position.x + _rectangle.Size.x * 0.25f, _rectangle.Position.y + _rectangle.Size.y * 0.25f),
                _rectangle.Size * 0.5f),
                _capacity);
            _child[SW] = new QuadTree<T>(new Rectangle(
                new Vector3(_rectangle.Position.x - _rectangle.Size.x * 0.25f, _rectangle.Position.y - _rectangle.Size.y * 0.25f),
                _rectangle.Size * 0.5f),
                _capacity);
            _child[SE] = new QuadTree<T>(new Rectangle(
                new Vector3(_rectangle.Position.x + _rectangle.Size.x * 0.25f, _rectangle.Position.y - _rectangle.Size.y * 0.25f),
                _rectangle.Size * 0.5f),
                _capacity);
            
            _divided = true;
        }

        public void Visualize(IQuadTreeView<T> view)
        {
            view.DrawBounds(_rectangle);
            for(var i = 0; i < _count; i++)
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