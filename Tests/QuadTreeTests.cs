﻿using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Trees.Runtime.QuadTrees;
using UnityEngine;

namespace Trees.Tests
{
    public class QuadTreeTests
    {
        [Test]
        public void InsertPointsInRange()
        {
            var range = new Vector3(100f, 100f);
            var quadTree = new QuadTree<int>(
                4, 
                new Rectangle(
                    new Vector3(0, 0), 
                    range
                )
            );

            quadTree.Insert(new Vector3(-10, 10), 0);
            quadTree.Insert(new Vector3(10, 10), 1);
            quadTree.Insert(new Vector3(10, -10), 2);
            quadTree.Insert(new Vector3(-10, -10), 3);
            quadTree.Insert(new Vector3(100, 100), 4); 
            quadTree.Insert(new Vector3(-100, 100), 5);
            quadTree.Insert(new Vector3(-50, 50), 6);
            quadTree.Insert(new Vector3(50, 50), 7);
            quadTree.Insert(new Vector3(50, -50), 8);
            quadTree.Insert(new Vector3(-50, -50), 9);

            var view = new QuadTreeTestView();

            quadTree.Visualize(view);

            Assert.True(view.Points.Count() == 8);
            Assert.True(view.Items.Count() == 8);
        }

        [Test]
        public void DivideTree()
        {
            var range = new Vector3(100f, 100f);
            var quadTree = new QuadTree<int>(
                4, 
                new Rectangle(
                    new Vector3(0, 0), 
                    range
                )
            );
            
            quadTree.Insert(new Vector3(-10, 10), 0);
            quadTree.Insert(new Vector3(10, 10), 1);
            quadTree.Insert(new Vector3(10, -10), 2);
            quadTree.Insert(new Vector3(-10, -10), 3);

            var divided = (bool)quadTree.GetType().GetField("_divided", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(quadTree);

            Assert.True(divided == false);
            
            quadTree.Insert(new Vector3(-10, -10), 4);
            
            divided = (bool)quadTree.GetType().GetField("_divided", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(quadTree);

            Assert.True(divided);
        }

        [Test]
        public void FindPointsInRectangle()
        {
            var range = new Vector3(100f, 100f);
            var quadTree = new QuadTree<int>(
                4, 
                new Rectangle(
                    new Vector3(0, 0), 
                    range
                )
            );
            
            quadTree.Insert(new Vector3(-25, 25), 0);
            quadTree.Insert(new Vector3(25, 25), 1);
            quadTree.Insert(new Vector3(25, -25), 2);
            quadTree.Insert(new Vector3(-25, -25), 3);

            //find points in range x(-50, 50) y(0, 50)
            var points = quadTree.Query(new Rectangle(
                new Vector3(0, 50),
                new Vector3(100, 50))).ToArray();


            Assert.True(points.Length == 2);
            Assert.True(points[0] == (new Vector3(-25, 25), 0));
            Assert.True(points[1] == (new Vector3(25, 25), 1));
        }

        [Test]
        public void FindPointsInCircle()
        {
            var range = new Vector3(100f, 100f);
            var quadTree = new QuadTree<int>(
                4, 
                new Rectangle(
                    new Vector3(0, 0), 
                    range
                )
            );
            
            quadTree.Insert(new Vector3(10, 0), 0);
            quadTree.Insert(new Vector3(25, 25), 1);
            quadTree.Insert(new Vector3(25, -25), 2);
            quadTree.Insert(new Vector3(-10, 0), 3);

            var points = quadTree.Query(new Circle(
                Vector3.zero,
                10f)).ToArray();

            Assert.True(points.Length == 2);
            Assert.True(points[0] == (new Vector3(10, 0), 0));
            Assert.True(points[1] == (new Vector3(-10, 0), 3));
        }
    }
}