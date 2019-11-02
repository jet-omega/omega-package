using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Omega.Tools.Experimental.UtilitiesAggregator
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
            if (ReferenceEquals(obj, null))
                throw new ArgumentNullException(nameof(obj));
            if (!obj)
                throw new MissingReferenceException(nameof(obj));

            AutoDestroyWithoutChecks(obj);
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
    }
}