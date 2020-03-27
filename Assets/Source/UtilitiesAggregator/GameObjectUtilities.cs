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
        /// <summary>
        /// Возвращает компонент, прикрепленный к объекту. Если экземпляр компонента заданного типа отсутствует на объекте
        /// то он будет добавлен к объекту.
        /// </summary>
        /// <param name="gameObject">Игровой объект</param>
        /// <typeparam name="T">Тип компонента</typeparam>
        /// <returns>Экземпляр компонента</returns>
        /// <exception cref="ArgumentNullException">Параметр <param name="gameObject"/>> указывает на null</exception>
        /// <exception cref="MissingReferenceException">Параметр <param name="gameObject"/>> указывает на уничтоженный объект</exception>
        [NotNull]
        public T MissingComponent<T>([NotNull] GameObject gameObject) where T : Component
        {
            if (gameObject is null)
                throw new ArgumentNullException(nameof(gameObject));
            if (!gameObject)
                throw new MissingReferenceException(nameof(gameObject));

            return MissingComponentWithoutChecks<T>(gameObject);
        }

        /// <summary>
        /// Возвращает компонент, прикрепленный к объекту. Если экземпляр компонента заданного типа отсутствует на объекте
        /// то он будет добавлен к объекту. 
        /// </summary>
        /// <param name="gameObject">Игровой объект</param>
        /// <param name="componentType">Объект-тип компонента</param>
        /// <returns>Экземпляр компонента</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="MissingReferenceException">Один из переданных параметров указывает на null</exception>
        /// <exception cref="ArgumentException"><param name="componentType"/>указывает на тип который не унаследован от типа UnityEngine.Component</exception>
        [NotNull]
        public Component MissingComponent([NotNull] GameObject gameObject, [NotNull] Type componentType)
        {
            if (gameObject is null)
                throw new ArgumentNullException(nameof(gameObject));
            if (!gameObject)
                throw new MissingReferenceException(nameof(gameObject));

            if (componentType == null)
                throw new ArgumentNullException(nameof(componentType));
            if (!componentType.IsSubclassOf(typeof(Component)))
                //В качестве параметра componentType был задан тип не унаследованный от UnityEngine.Component
                throw new ArgumentException(
                    $"As a {nameof(componentType)} parameter, a type not inherited from {typeof(Component).FullName} was set");

            return MissingComponentWithoutChecks(gameObject, componentType);
        }

        /// <summary>
        /// Пытается найти объект на компоненте, если компонент найден вернет true а <param name="component"/>>будет указывать на найденный объект,
        /// в противном случае, вернет false, а <param name="component"/>>будет указывать на null
        /// </summary>
        /// <param name="gameObject">Игровой объект</param>
        /// <param name="component">Ссылка на найденный объект (null, если объект не найден)</param>
        /// <typeparam name="T">Тип компонента</typeparam>
        /// <returns>true - компонент найден, false - объект не найден</returns>
        /// <exception cref="ArgumentNullException">Параметр <param name="gameObject"/>> указывает на null</exception>
        /// <exception cref="MissingReferenceException">Параметр <param name="gameObject"/>> указывает на уничтоженный объект</exception>
        public bool TryGetComponent<T>([NotNull] GameObject gameObject, [CanBeNull] out T component)
        {
            if (gameObject is null)
                throw new ArgumentNullException(nameof(gameObject));
            if (!gameObject)
                throw new MissingReferenceException(nameof(gameObject));

            return TryGetComponentWithoutChecks(gameObject, out component);
        }

        /// <summary>
        /// Проверяет содержит ли объект компонент заданного типа 
        /// </summary>
        /// <param name="gameObject">Игровой объект</param>
        /// <typeparam name="T">Тип компонента</typeparam>
        /// <returns>true - компонент с заданном типом присутствует на объекте, иначе false</returns>
        /// <exception cref="ArgumentNullException">Параметр <param name="gameObject"/>>указывает на null</exception>
        /// <exception cref="MissingReferenceException">Параметр <param name="gameObject"/>>указывает на уничтоженный объект</exception>
        public bool ContainsComponent<T>([NotNull] GameObject gameObject)
        {
            if (gameObject is null)
                throw new ArgumentNullException(nameof(gameObject));
            if (!gameObject)
                throw new MissingReferenceException(nameof(gameObject));

            return ContainsComponentWithoutChecks<T>(gameObject);
        }

        /// <summary>
        /// Пытается найти компонент заданного типа среди объектов-потомков указанного объекта.
        /// В отличии от GetComponentInChildren, проверка идет только по прямым потомкам объекта,
        /// не выполняя поиск по всему дереву потомков.
        /// </summary>
        /// <param name="gameObject">Объект относительно которого нужно выполнить поиск</param>
        /// <param name="componentType">Тип компонента</param>
        /// <param name="searchInRoot">Нужно ли выполнить поиск внутри самого объекта gameObject</param>
        /// <returns>Если компонент указанного типа найден то вернется экземпляр этого компонента, в противном случае - null</returns>
        /// <exception cref="ArgumentNullException">gameObject или componentType указывают на null</exception>
        /// <exception cref="MissingReferenceException">gameObject указывает на уничтоженный объект</exception>
        [CanBeNull]
        public Component GetComponentInDirectChildren([NotNull] GameObject gameObject, [NotNull] Type componentType,
            bool searchInRoot = false)
        {
            if (ReferenceEquals(gameObject, null))
                throw new ArgumentNullException(nameof(gameObject));
            if (componentType == null)
                throw new ArgumentNullException(nameof(componentType));
            if (!gameObject)
                throw new MissingReferenceException(nameof(gameObject));

            return GetComponentDirectWithoutChecks(gameObject, componentType, searchInRoot);
        }

        /// <summary>
        /// Пытается найти компоненты заданного типа среди объектов-потомков указанного объекта.
        /// В отличии от GetComponentsInChildren, проверка идет только по прямым потомкам объекта,
        /// не выполняя поиск по всему дереву потомков.
        /// </summary>
        /// <param name="gameObject">Объект относительно которого нужно выполнить поиск</param>
        /// <param name="componentType">Тип компонента</param>
        /// <param name="searchInRoot">Нужно ли выполнить поиск внутри самого объекта gameObject</param>
        /// <returns>Массив найденных компонентов заданного типа, если ни одного объекта не найдено - пустой массив</returns>
        /// <exception cref="ArgumentNullException">gameObject или componentType указывают на null</exception>
        /// <exception cref="MissingReferenceException">gameObject указывает на уничтоженный объект</exception>
        [NotNull]
        public Component[] GetComponentsInDirectChildren([NotNull] GameObject gameObject, [NotNull] Type componentType,
            bool searchInRoot = false)
        {
            if (ReferenceEquals(gameObject, null))
                throw new ArgumentNullException(nameof(gameObject));
            if (componentType == null)
                throw new ArgumentNullException(nameof(componentType));
            if (!gameObject)
                throw new MissingReferenceException(nameof(gameObject));

            var result = ListPool<Component>.Rent();

            GetComponentsDirectWithoutChecks(gameObject, componentType, result, searchInRoot);
            
            var resultArray = result.Count == 0
                ? Array.Empty<Component>()
                : result.ToArray();

            ListPool<Component>.ReturnInternal(result);

            return resultArray;
        }

        /// <summary>
        /// Пытается найти компонент заданного типа среди объектов-потомков указанного объекта.
        /// В отличии от GetComponentInChildren, проверка идет только по прямым потомкам объекта,
        /// не выполняя поиск по всему дереву потомков.
        /// </summary>
        /// <param name="gameObject">Объект относительно которого нужно выполнить поиск</param>
        /// <param name="searchInRoot">Нужно ли выполнить поиск внутри самого объекта gameObject</param>
        /// <typeparam name="T">Тип компонента</typeparam>
        /// <returns>Если компонент указанного типа найден то вернется экземпляр этого компонента, в противном случае - null</returns>
        /// <exception cref="ArgumentNullException">gameObject или componentType указывают на null</exception>
        /// <exception cref="MissingReferenceException">gameObject указывает на уничтоженный объект</exception>
        [CanBeNull]
        public T GetComponentInDirectChildren<T>([CanBeNull] GameObject gameObject, bool searchInRoot = false)
        {
            if (ReferenceEquals(gameObject, null))
                throw new ArgumentNullException(nameof(gameObject));
            if (!gameObject)
                throw new MissingReferenceException(nameof(gameObject));

            return GetComponentDirectWithoutChecks<T>(gameObject, searchInRoot);
        }

        /// <summary>
        /// Пытается найти компоненты заданного типа среди объектов-потомков указанного объекта.
        /// В отличии от GetComponentsInChildren, проверка идет только по прямым потомкам объекта,
        /// не выполняя поиск по всему дереву потомков.
        /// </summary>
        /// <param name="gameObject">Объект относительно которого нужно выполнить поиск</param>
        /// <param name="searchInRoot">Нужно ли выполнить поиск внутри самого объекта gameObject</param>
        /// <typeparam name="T">Тип компонента</typeparam>
        /// <returns>Массив найденных компонентов заданного типа, если ни одного объекта не найдено - пустой массив</returns>
        /// <exception cref="ArgumentNullException">gameObject или componentType указывают на null</exception>
        /// <exception cref="MissingReferenceException">gameObject указывает на уничтоженный объект</exception>
        [NotNull]
        public T[] GetComponentsInDirectChildren<T>([CanBeNull] GameObject gameObject, bool searchInRoot = false)
        {
            if (ReferenceEquals(gameObject, null))
                throw new ArgumentNullException(nameof(gameObject));
            if (!gameObject)
                throw new MissingReferenceException(nameof(gameObject));

            var result = ListPool<T>.Rent();
            GetComponentsDirectWithoutChecks(gameObject, result, searchInRoot);

            var resultArray = result.Count == 0
                ? Array.Empty<T>()
                : result.ToArray();

            ListPool<T>.ReturnInternal(result);

            return resultArray;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T MissingComponentWithoutChecks<T>(GameObject gameObject) where T : Component
            => gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [NotNull]
        internal static Component MissingComponentWithoutChecks([NotNull] GameObject gameObject,
            [NotNull] Type componentType)
            => gameObject.GetComponent(componentType) ?? gameObject.AddComponent(componentType);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryGetComponentWithoutChecks<T>([NotNull] GameObject gameObject,
                [CanBeNull] out T component)
            // Проверка на null выбрана намеренно, так как GetComponent никогда не вернет уничтоженный объект
            // https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html
            // Здесь нельзя использовать "is null" так как GetComponent возвращает объект типа T, который, с точки зрения
            // компилятора может быть как ссылочным типом так и значимым, поэтому левый операнд приводится к object
            => !((object) (component = gameObject.GetComponent<T>()) is null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryGetComponentWithoutChecks([NotNull] GameObject gameObject, [NotNull] Type componentType,
                [CanBeNull] out Component component)
            // Проверка на null выбрана намеренно, так как GetComponent никогда не вернет уничтоженный объект
            // https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html
            => !((component = gameObject.GetComponent(componentType)) is null);

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
                if (TryGetComponentWithoutChecks(gameObject, componentType, out var component))
                    return component;

            var transform = gameObject.transform;
            for (int i = 0; i < transform.childCount; i++)
            {
                var childGameObject = transform.GetChild(i).gameObject;
                if (TryGetComponentWithoutChecks(childGameObject, componentType, out var component))
                    return component;
            }

            return default;
        }

        internal static void GetComponentsDirectWithoutChecks([NotNull] GameObject gameObject,
            [NotNull] Type componentType, [NotNull] List<Component> result, bool searchInRoot = false)
        {
            if (searchInRoot)
                if (TryGetComponentWithoutChecks(gameObject, componentType, out var component))
                    result.Add(component);

            var transform = gameObject.transform;
            var tempList = ListPool<Component>.Rent();
            for (int i = 0; i < transform.childCount; i++)
            {
                var childGameObject = transform.GetChild(i).gameObject;
                childGameObject.GetComponents(componentType, tempList);
                result.AddRange(tempList);
            }
            
            ListPool<Component>.ReturnInternal(tempList);
        }

        [CanBeNull]
        internal static T GetComponentDirectWithoutChecks<T>([NotNull] GameObject gameObject, bool searchInRoot = false)
        {
            if (searchInRoot && TryGetComponentWithoutChecks<T>(gameObject, out var selfComponent))
                return selfComponent;

            var transform = gameObject.transform;
            var transformChildCount = transform.childCount;
            
            for (int i = 0; i < transformChildCount; i++)
            {
                var childGameObject = transform.GetChild(i).gameObject;
                if (TryGetComponentWithoutChecks<T>(childGameObject, out var component))
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
            var tempList = ListPool<T>.Rent();
            for (int i = 0; i < transform.childCount; i++)
            {
                var childGameObject = transform.GetChild(i).gameObject;
                childGameObject.GetComponents(tempList);
                result.AddRange(tempList);
            }

            ListPool<T>.ReturnInternal(tempList);
        }
    }
}