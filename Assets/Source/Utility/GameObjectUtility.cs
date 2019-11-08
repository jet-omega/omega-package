﻿using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace Omega.Tools
{
    public static class GameObjectUtility
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
        public static T MissingComponent<T>([NotNull] GameObject gameObject) where T : Component
        {
            if (ReferenceEquals(gameObject, null))
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
        public static Component MissingComponent([NotNull] GameObject gameObject, [NotNull] Type componentType)
        {
            if (ReferenceEquals(gameObject, null))
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
        public static bool TryGetComponent<T>([NotNull] GameObject gameObject, [CanBeNull] out T component)
            where T : Component
        {
            if (ReferenceEquals(gameObject, null))
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
        public static bool ContainsComponent<T>([NotNull] GameObject gameObject) where T : Component
        {
            if (ReferenceEquals(gameObject, null))
                throw new ArgumentNullException(nameof(gameObject));
            if (!gameObject)
                throw new MissingReferenceException(nameof(gameObject));

            return ContainsComponentWithoutChecks<T>(gameObject);
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
            where T : Component
        // Проверка на null выбрана намеренно, так как GetComponent никогда не вернет уничтоженный объект
        // https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html
            => (component = gameObject.GetComponent<T>()) != null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryGetComponentWithoutChecks([NotNull] GameObject gameObject, [NotNull] Type componentType,
                [CanBeNull] out Component component)
            // Проверка на null выбрана намеренно, так как GetComponent никогда не вернет уничтоженный объект
            // https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html
            => (component = gameObject.GetComponent(componentType)) != null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool ContainsComponentWithoutChecks<T>([NotNull] GameObject gameObject) where T : Component
        // Проверка на null выбрана намеренно, так как GetComponent никогда не вернет уничтоженный объект
        // https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html
            => gameObject.GetComponent<T>() != null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool ContainsComponentWithoutChecks([NotNull] GameObject gameObject,
                [NotNull] Type componentType)
            // Проверка на null выбрана намеренно, так как GetComponent никогда не вернет уничтоженный объект
            // https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html
            => gameObject.GetComponent(componentType) != null;
    }
}