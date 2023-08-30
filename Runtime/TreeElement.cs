using UnityEngine;

namespace Trees.Runtime
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