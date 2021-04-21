using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Omega.Package;
using UnityEngine;

namespace Omega.Tools.Tests
{
    public sealed class TransformUtilityTests
    {
        [Test]
        public void GetChildrenShouldReturnEmptyArrayFromSystemArrayEmpty()
        {
            var gameObjectInstance = new GameObject();

            var children = gameObjectInstance.transform.GetChildren();

            Assert.NotNull(children);
            Assert.Zero(children.Length);

            Assert.AreEqual(children, Array.Empty<Transform>());
            
            Utilities.Object.AutoDestroy(gameObjectInstance);
        }
        

        [Test]
        public void GetChildrenShouldThrowMissingReferenceExceptionWhenParameterWereDestroyed()
        {
            var gameObjectInstance = new GameObject();

            Utilities.Object.AutoDestroy(gameObjectInstance);

            Assert.Throws<MissingReferenceException>(() =>
                gameObjectInstance.transform.GetChildren());
        }
        
        [Test]
        public void GetAllChildrenShouldReturnAllChildrenTest()
        {
            int goCount = 50; 
            
            var root = new GameObject("root").transform;
            var circuitParent = root;
            var complexHierarchyObjects = GameObjectFactory.New()
                .Custom(go => go.transform.parent = circuitParent)
                .Custom(go => circuitParent = go.transform)
                .Build<Transform>(goCount);

            var result = new List<Transform>(goCount);
            Utilities.Transform.GetAllChildren(root, result);
            
            Assert.Zero(complexHierarchyObjects.Except(result).Count());
            
            Utilities.Object.AutoDestroy(root.gameObject);
        }
        
        [Test]
        public void GetAllChildrenCountShouldReturnCountAllChildrenTest()
        {
            int goCount = 50; 
            
            var root = new GameObject("root").transform;
            var circuitParent = root;
            GameObjectFactory.New()
                .Custom(go => go.transform.parent = circuitParent)
                .Custom(go => circuitParent = go.transform)
                .Build<Transform>(goCount);

            var result = Utilities.Transform.GetAllChildrenCount(root);
            
            Assert.AreEqual(goCount, result);
            
            Utilities.Object.AutoDestroy(root.gameObject);
        }

        [Test]
        public void IsChildOfShouldReturnTrueWhenChildIsDeepTest()
        {
            int goCount = 50; 
            
            var root = new GameObject("root").transform;
            var circuitParent = root;
            GameObjectFactory.New()
                .Custom(go => go.transform.parent = circuitParent)
                .Custom(go => circuitParent = go.transform)
                .Build<Transform>(goCount);

            var result = circuitParent.IsChildOf(root);
            Assert.True(result);
            
            Utilities.Object.AutoDestroy(root.gameObject);
        }
    }
}