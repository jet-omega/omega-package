using System;
using System.Collections;
using NUnit.Framework;
using Omega.Experimental;
using Omega.Routines.HighLoad;
using Omega.Tools;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using GameObjectUtility = UnityEditor.GameObjectUtility;
using Object = UnityEngine.Object;

namespace Omega.Routines.Tests.HighLoad
{
    public class ObjectRoutineTest
    {
        [UnityTest]
        [Timeout(300 * 1000)]
        public IEnumerator SimpleTest()
        {
            var instance  = new GameObject("parent");
            var targetObjectCount = 1_000; 

            var factory = GameObjectFactory.New()
                .AddComponent<BoxCollider>()
                .SetParent(instance.transform, false);

            yield return ObjectRoutine.Instantiate(factory, targetObjectCount, 0.01f);
            
            Assert.AreEqual(targetObjectCount, instance.transform.childCount);
            
            Utilities.Object.AutoDestroy(instance);
        }
    }
}