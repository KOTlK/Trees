using UnityEngine;

namespace Trees.Runtime.QuadTrees
{
    public class TreeItem<T>
    {
        public T Value;
        public Vector3 Position;

        public TreeItem(T value, Vector3 position)
        {
            Value = value;
            Position = position;
        }
    }
}