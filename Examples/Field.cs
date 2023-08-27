using Trees.Runtime.QuadTrees;
using Trees.Runtime.QuadTrees.View;
using UnityEngine;

namespace Trees.Example
{
    public class Field : MonoBehaviour
    {
        [SerializeField] private Vector2 _startPosition = Vector2.zero;
        [SerializeField] private Vector2 _size = new Vector2(100, 100);
        [SerializeField] private int _pointsAmount = 100;
        [SerializeField] private IntView _view;
        [SerializeField] private int _count;

        private QuadTree<int> _quadTree;
        private readonly System.Random _random = new();

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