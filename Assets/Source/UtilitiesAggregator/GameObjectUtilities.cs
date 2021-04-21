using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Omega.Package;
using UnityEngine;

namespace Omega.Package.Internal
{
    public sealed class GameObjectUtilities
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T MissingComponentWithoutChecks<T>(GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponent<T>();
            return component ? component : gameObject.AddComponent<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [NotNull]
        internal static Component MissingComponentWithoutChecks([NotNull] GameObject gameObject,
            [NotNull] Type componentType)
            => gameObject.GetComponent(componentType) ?? gameObject.AddComponent(componentType);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool ContainsComponentWithoutChecks<T>([NotNull] GameObject gameObject)
            // Проверка на null выбрана намеренно, так как GetComponent никогда не вернет уничтоженный объект
            // https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html
            // Здесь нельзя использовать "is null" так как GetComponent возвращает объект типа T, который, с точки зрения
            // компилятора может быть как ссылочным типом так и значимым, поэтому левый операнд приводится к object
            => !((object) gameObject.GetComponent<T>() is null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool ContainsComponentWithoutChecks([NotNull] GameObject gameObject,
                [NotNull] Type componentType)
            // Проверка на null выбрана намеренно, так как GetComponent никогда не вернет уничтоженный объект
            // https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html
            => !(gameObject.GetComponent(componentType) is null);

        [CanBeNull]
        internal static Component GetComponentDirectWithoutChecks([NotNull] GameObject gameObject,
            [NotNull] Type componentType, bool searchInRoot = false)
        {
            if (searchInRoot)
                if (gameObject.TryGetComponent(componentType, out var component))
                    return component;

            var transform = gameObject.transform;
            for (int i = 0; i < transform.childCount; i++)
            {
                var childGameObject = transform.GetChild(i).gameObject;
                if (childGameObject.TryGetComponent(componentType, out var component))
                    return component;
            }

            return default;
        }

        internal static void GetComponentsDirectWithoutChecks([NotNull] GameObject gameObject,
            [NotNull] Type componentType, [NotNull] List<Component> result, bool searchInRoot = false)
        {
            if (searchInRoot)
                if (gameObject.TryGetComponent(componentType, out var component))
                    result.Add(component);

            var transform = gameObject.transform;
            
            using (ListPool<Component>.InternalShared.Use(out var tempList))
                for (int i = 0; i < transform.childCount; i++)
                {
                    var childGameObject = transform.GetChild(i).gameObject;
                    childGameObject.GetComponents(componentType, tempList);
                    result.AddRange(tempList);
                }
        }

        [CanBeNull]
        internal static T GetComponentDirectWithoutChecks<T>([NotNull] GameObject gameObject, bool searchInRoot = false)
        {
            if (searchInRoot && gameObject.TryGetComponent<T>(out var selfComponent))
                return selfComponent;

            var transform = gameObject.transform;
            var transformChildCount = transform.childCount;

            for (int i = 0; i < transformChildCount; i++)
            {
                var childGameObject = transform.GetChild(i).gameObject;
                if (childGameObject.TryGetComponent<T>(out var component))
                    return component;
            }

            return default;
        }

        internal static void GetComponentsDirectWithoutChecks<T>([NotNull] GameObject gameObject,
            [NotNull] List<T> result,
            bool searchInRoot = false)
        {
            if (searchInRoot)
                gameObject.GetComponents(result);

            var transform = gameObject.transform;

            using (ListPool<T>.InternalShared.Use(out var tempList))
                for (int i = 0; i < transform.childCount; i++)
                {
                    var childGameObject = transform.GetChild(i).gameObject;
                    childGameObject.GetComponents(tempList);
                    result.AddRange(tempList);
                }
        }
    }
}