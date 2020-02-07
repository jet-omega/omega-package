using System;
using Omega.Tools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omega.Routines.HighLoad
{
    public static class ObjectRoutine
    {
        public static Routine<GameObject[]> Instantiate(GameObjectFactory factory, int count,
            float timePerFrame = 0.02f,
            int measureFrequency = 10)
        {
            var result = new GameObject[count];
            var routine = new ApportionedRoutine(i =>
                {
                    result[i] = factory.Build();
                }, count, timePerFrame, measureFrequency);
            
            return Routine.WaitOne(routine, () => result);
        }
        
        public static Routine<T[]> Instantiate<T>(T original, int count, float timePerFrame = 0.02f,
            int measureFrequency = 10)
            where T : Object
        {
            var result = new T[count];
            var routine = new ApportionedRoutine(i =>
                {
                    result[i] = Object.Instantiate(original);
                }, count, timePerFrame, measureFrequency);

            return Routine.WaitOne(routine, () => result);
        }

        public static Routine<T[]> Instantiate<T>(T original, Action<T, int> initialization, int count,
            float timePerFrame = 0.02f, int measureFrequency = 10)
            where T : Object
        {
            var result = new T[count];
            var routine = new ApportionedRoutine(i =>
            {
                var instance = Object.Instantiate(original);
                initialization(instance, i);
                result[i] = instance;
            }, count, timePerFrame, measureFrequency);

            return Routine.WaitOne(routine, () => result);
        }
    }
}