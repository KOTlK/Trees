using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Trees.Runtime;
using Trees.Runtime.OcTrees;
using Trees.Runtime.QuadTrees;
using UnityEngine;

namespace Trees.Tests
{
    public class OcTreeTests
    {
        [Test]
        public void InsertPointsInRange()
        {
            var range = new Vector3(100f, 100f, 100);
            var ocTree = new OcTree<int>(
                new AABB(
                    Vector3.zero,
                    range)
            );

            ocTree.Insert(new TreeElement<int>(new Vector3(-10, 10), 0));
            ocTree.Insert(new TreeElement<int>(new Vector3(10, 10), 1));
            ocTree.Insert(new TreeElement<int>(new Vector3(10, -10), 2));
            ocTree.Insert(new TreeElement<int>(new Vector3(-10, -10), 3));
            ocTree.Insert(new TreeElement<int>(new Vector3(100, 100), 4)); 
            ocTree.Insert(new TreeElement<int>(new Vector3(-100, 100), 5));
            ocTree.Insert(new TreeElement<int>(new Vector3(-50, 50), 6));
            ocTree.Insert(new TreeElement<int>(new Vector3(50, 50), 7));
            ocTree.Insert(new TreeElement<int>(new Vector3(50, -50), 8));
            ocTree.Insert(new TreeElement<int>(new Vector3(-50, -50), 9));

            var view = new OcTreeTestView();

            ocTree.Visualize(view);

            Assert.True(view.Elements.Count == 8);
        }

        [Test]
        public void DivideTree()
        {
            var range = new Vector3(100f, 100f, 100);
            var ocTree = new OcTree<int>(
                new AABB(
                    Vector3.zero,
                    range)
            );
            
            ocTree.Insert(new TreeElement<int>(new Vector3(-10, 10), 0));
            ocTree.Insert(new TreeElement<int>(new Vector3(10, 10), 1));
            ocTree.Insert(new TreeElement<int>(new Vector3(10, -10), 2));
            ocTree.Insert(new TreeElement<int>(new Vector3(-10, -10), 3));
            ocTree.Insert(new TreeElement<int>(new Vector3(-10, -10), 4));
            ocTree.Insert(new TreeElement<int>(new Vector3(-10, -10), 5));
            ocTree.Insert(new TreeElement<int>(new Vector3(-10, -10), 6));
            ocTree.Insert(new TreeElement<int>(new Vector3(-10, -10), 7));

            var divided = (bool)ocTree.GetType().GetField("_divided", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(ocTree);

            Assert.True(divided == false);
            
            ocTree.Insert(new TreeElement<int>(new Vector3(-10, -10), 8));
            
            divided = (bool)ocTree.GetType().GetField("_divided", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(ocTree);

            Assert.True(divided);
        }

        [Test]
        public void FindPointsInRectangle()
        {
            var range = new Vector3(100f, 100f, 100);
            var ocTree = new OcTree<int>(
                new AABB(
                    Vector3.zero,
                    range)
            );
            
            ocTree.Insert(new TreeElement<int>(new Vector3(-25, 25, 10), 0));
            ocTree.Insert(new TreeElement<int>(new Vector3(25, 25, 10), 1));
            ocTree.Insert(new TreeElement<int>(new Vector3(25, -25, -10), 2));
            ocTree.Insert(new TreeElement<int>(new Vector3(-25, -25, -10), 3));

            //find points in range x(-50, 50) y(0, 50)
            var points = ocTree.Query(new AABB(
                new Vector3(0, 50),
                new Vector3(100, 50, 100))).ToArray();


            Assert.True(points.Length == 2);
            Assert.True(points[0].Position == new Vector3(-25, 25, 10));
            Assert.True(points[1].Position == new Vector3(25, 25, 10));
        }

        [Test]
        public void FindPointsInCircle()
        {
            var range = new Vector3(100f, 100f, 100);
            var ocTree = new OcTree<int>(
                new AABB(
                    Vector3.zero,
                    range)
            );
            
            ocTree.Insert(new TreeElement<int>(new Vector3(10, 0), 0));
            ocTree.Insert(new TreeElement<int>(new Vector3(25, 25), 1));
            ocTree.Insert(new TreeElement<int>(new Vector3(25, -25), 2));
            ocTree.Insert(new TreeElement<int>(new Vector3(-10, 0), 3));

            var points = ocTree.Query(new Sphere(
                Vector3.zero,
                10f)).ToArray();

            Assert.True(points.Length == 2);
            Assert.True(points[0].Position == new Vector3(10, 0));
            Assert.True(points[1].Position == new Vector3(-10, 0));
        }

        [Test]
        public void FindClosestPoint()
        {
            var range = new Vector3(100f, 100f, 100);
            var ocTree = new OcTree<int>(
                new AABB(
                    Vector3.zero,
                    range)
            );

            ocTree.Insert(new TreeElement<int>(new Vector3(10, 0), 0));
            ocTree.Insert(new TreeElement<int>(new Vector3(15, 0), 1));
            ocTree.Insert(new TreeElement<int>(new Vector3(20, 0), 2));
            ocTree.Insert(new TreeElement<int>(new Vector3(-10, 0), 3));

            var closest = new TreeElement<int>();

            var closestFound = ocTree.Query(Vector3.zero, ref closest);

            Assert.True(closestFound);
            Assert.True(closest.Value == 0);
        }
    }
}