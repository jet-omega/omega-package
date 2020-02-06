using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omega.Package.Internal
{
    public sealed class ObjectUtilities
    {
        internal ObjectUtilities()
        {
        }

        /// <summary>
        /// Уничтожает объект с помощью Object.Destroy если выполнено условие Application.isPlaying
        /// иначе объект будет уничтожен с помощью Object.DestroyImmediate
        /// </summary>
        /// <param name="obj">Объект который необходимо уничтожить</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MissingReferenceException"></exception>
        public void AutoDestroy(Object obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));
            if (!obj)
                throw new MissingReferenceException(nameof(obj));

            AutoDestroyWithoutChecks(obj);
        }

        public void AutoDestroy(Object obj, bool useDestroyImmediate)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));
            if (!obj)
                throw new MissingReferenceException(nameof(obj));

            AutoDestroyWithoutChecks(obj, useDestroyImmediate);
        }

        public void AutoDestroy(params Object[] objs)
        {
            if (objs == null)
                throw new ArgumentNullException(nameof(objs));

            for (int i = 0; i < objs.Length; i++)
                AutoDestroy(objs[i]);
        }

        public void AutoDestroy(bool useDestroyImmediate, params Object[] objs)
        {
            if (objs == null)
                throw new ArgumentNullException(nameof(objs));

            for (int i = 0; i < objs.Length; i++)
                AutoDestroy(objs[i], useDestroyImmediate);
        }

#if !UNITY_EDITOR
        [System.Runtime.CompilerServices.MethodImpl(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
        internal static void AutoDestroyWithoutChecks(Object obj)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                Object.DestroyImmediate(obj, false);
            else Object.Destroy(obj);
#else
            Object.Destroy(obj);
#endif
        }

#if !UNITY_EDITOR
        [System.Runtime.CompilerServices.MethodImpl(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
        internal static void AutoDestroyWithoutChecks(Object obj, bool useDestroyImmediate)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || useDestroyImmediate)
                Object.DestroyImmediate(obj, false);
            else Object.Destroy(obj);
#else
            if (useDestroyImmediate)
                Object.DestroyImmediate(obj, false);
            else Object.Destroy(obj);
#endif
        }
    }
}