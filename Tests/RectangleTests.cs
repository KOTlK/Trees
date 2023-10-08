using NUnit.Framework;
using Trees.Runtime.QuadTrees;
using UnityEngine;

namespace Trees.Tests
{
    public class RectangleTests
    {
        [Test]
        public void RectangleContainsPoint()
        {
            var rect = new Rectangle(Vector3.zero, Vector3.one);

            Assert.True(rect.Contains(new Vector3(0.5f, 0)));
            Assert.True(rect.Contains(new Vector3(-0.5f, 0)));
            Assert.True(rect.Contains(new Vector3(0, 0.5f)));
            Assert.True(rect.Contains(new Vector3(0, -0.5f)));
            Assert.True(rect.Contains(new Vector3(0.5f, 0.5f)));
            Assert.True(rect.Contains(new Vector3(0.5f, -0.5f)));
            Assert.True(rect.Contains(new Vector3(-0.5f, 0.5f)));
            Assert.True(rect.Contains(new Vector3(-0.5f, -0.5f)));
            Assert.True(rect.Contains(new Vector3(0, 0)));
        }
    }
}