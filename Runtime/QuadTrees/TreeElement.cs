using UnityEngine;

namespace Trees.Runtime.QuadTrees
{
    public struct TreeElement<T>
    {
        public Vector3 Position;
        public T Value;

        public TreeElement(Vector3 position, T value)
        {
            Position = position;
            Value = value;
        }
    }
}