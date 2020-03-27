using System;
using System.Collections;
using NUnit.Framework;
using Omega.Package;
using Omega.Routines.HighLoad;
using Omega.Tools;
using UnityEngine;
using UnityEngine.TestTools;

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