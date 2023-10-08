using System;
using System.Collections.Generic;
using Trees.Runtime.QuadTrees.View;
using UnityEngine;

namespace Trees.Runtime.QuadTrees
{
    public class DynamicQuadTree<T>
    {
        private readonly int _capacity;
        private readonly TreeItem<T>[] _elements;
        private readonly DynamicQuadTree<T> _parent;
        private readonly bool _isRoot;
        private DynamicQuadTree<T> _northWest;
        private DynamicQuadTree<T> _northEast;
        private DynamicQuadTree<T> _southWest;
        private DynamicQuadTree<T> _southEast;

        private Rectangle _rectangle;
        private bool _divided;
        private int _count;

        private static readonly Dictionary<TreeItem<T>, DynamicQuadTree<T>> Items = new();

        public DynamicQuadTree(Rectangle rectangle, int capacity, DynamicQuadTree<T> parent) : this(rectangle, capacity)
        {
            _isRoot = false;
            _parent = parent;
        }

        public DynamicQuadTree(Rectangle rectangle, int capacity = 4)
        {
            _elements = new TreeItem<T>[capacity];
            _capacity = capacity;
            _rectangle = rectangle;
            _isRoot = true;
        }

        public int Count()
        {
            if (_divided == false)
                return _count;
            
            return _count + _northEast.Count() + _northWest.Count() + _southEast.Count() + _southWest.Count();
        }

        public bool Insert(TreeItem<T> item)
        {
            if (_rectangle.Contains(item.Position) == false)
                return false;

            if (_count < _capacity)
            {
                _elements[_count] = item;
                _count++;
                Items.Add(item, this);
                return true;
            }

            if (_divided == false)
            {
                Divide();
            }

            if (_northWest.Insert(item))
                return true;
            
            if (_northEast.Insert(item))
                return true;
            
            if (_southWest.Insert(item))
                return true;
            
            if (_southEast.Insert(item))
                return true;

            return false;
        }

        public void Remove(TreeItem<T> item)
        {
            for (var i = 0; i < _count; i++)
            {
                var element = _elements[i];

                if (element == item)
                {
                    _elements[i] = null;
                    Items.Remove(item);
                    _count--;
                    ShiftItems(i);
                    break;
                }
            }
        }

        public DynamicQuadTree<T> Locate(TreeItem<T> item)
        {
            return Items[item];
        }

        public void LocateAndRemove(TreeItem<T> item)
        {
            var qt = Locate(item);
            qt.Remove(item);
        }

        public void Update()
        {
            for (var i = 0; i < _count; i++)
            {
                var element = _elements[i];

                if (_rectangle.Contains(element.Position) == false)
                {
                    if(_isRoot)
                        continue;
                    
                    Remove(element);

                    InsertBackRecursively(element);
                }
            }

            if (_divided == false) 
                return;

            _northWest.Update();
            _northEast.Update();
            _southWest.Update();
            _southEast.Update();
        }

        public IEnumerable<TreeItem<T>> Query(Rectangle range)
        {
            var total = new List<TreeItem<T>>();

            if (_rectangle.Intersect(range) == false)
                return total;

            for (var i = 0; i < _count; i++)
            {
                if (range.Contains(_elements[i].Position))
                {
                    total.Add(_elements[i]);
                }
            }

            if (_divided)
            {
                total.AddRange(_northWest.Query(range));
                total.AddRange(_northEast.Query(range));
                total.AddRange(_southWest.Query(range));
                total.AddRange(_southEast.Query(range));
            }

            return total;
        }
        
        public IEnumerable<TreeItem<T>> Query(Circle range)
        {
            var total = new List<TreeItem<T>>();

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

            total.AddRange(_northWest.Query(range));
            total.AddRange(_northEast.Query(range));
            total.AddRange(_southWest.Query(range));
            total.AddRange(_southEast.Query(range));

            return total;
        }
        
        public bool Query(Vector3 point, ref TreeItem<T> closest, float previousDistance = float.MaxValue)
        {
            if (_rectangle.Contains(point) == false)
                return false;

            for (var i = 0; i < _count; i++)
            {
                var distance = (_elements[i].Position - point).sqrMagnitude;
                if (distance < previousDistance)
                {
                    previousDistance = distance;
                    closest = _elements[i];
                }
            }

            if (_divided == false)
            {
                return true;
            }

            _northWest.Query(point, ref closest, previousDistance);
            _northEast.Query(point, ref closest, previousDistance);
            _southWest.Query(point, ref closest, previousDistance);
            _southEast.Query(point, ref closest, previousDistance);


            return true;
        }
        
        public void Visualize(IQuadTreeView<T> view)
        {
            view.DrawBounds(_rectangle);
            view.DrawElementsCount(Count());
            for(var i = 0; i < _count; i++)
            {
                view.DrawElement(_elements[i].Position, _elements[i].Value);
            }

            if (_divided == false)
            {
                return;
            }

            _northWest.Visualize(view);
            _northEast.Visualize(view);
            _southWest.Visualize(view);
            _southEast.Visualize(view);
        }

        private void InsertBackRecursively(TreeItem<T> item)
        {
            var inserted = Insert(item);
            
            if (_isRoot)
            {
                return;
            }
            
            if (inserted == false)
            {
                _parent.InsertBackRecursively(item);
            }
        }

        private void ShiftItems(int startIndex)
        {
            if (startIndex == _capacity - 1)
            {
                _elements[startIndex] = null;
                return;
            }
            
            Array.Copy(_elements, startIndex + 1, _elements, startIndex, _capacity - startIndex - 1);
            _elements[^1] = null;
        }


        private void Divide()
        {
            if (_divided)
                return;

            _northWest = new DynamicQuadTree<T>(
                new Rectangle(
                    new Vector3(_rectangle.Position.x - _rectangle.Size.x * 0.25f,
                        _rectangle.Position.y + _rectangle.Size.y * 0.25f),
                    _rectangle.Size * 0.5f),
                _capacity,
                this);

            _northEast = new DynamicQuadTree<T>(
                new Rectangle(
                    new Vector3(_rectangle.Position.x + _rectangle.Size.x * 0.25f,
                        _rectangle.Position.y + _rectangle.Size.y * 0.25f),
                    _rectangle.Size * 0.5f),
                _capacity,
                this);

            _southWest = new DynamicQuadTree<T>(
                new Rectangle(
                    new Vector3(_rectangle.Position.x - _rectangle.Size.x * 0.25f,
                        _rectangle.Position.y - _rectangle.Size.y * 0.25f),
                    _rectangle.Size * 0.5f),
                _capacity,
                this);

            _southEast = new DynamicQuadTree<T>(
                new Rectangle(
                    new Vector3(_rectangle.Position.x + _rectangle.Size.x * 0.25f,
                        _rectangle.Position.y - _rectangle.Size.y * 0.25f),
                    _rectangle.Size * 0.5f),
                _capacity,
                this);

            _divided = true;
        }
    }
}