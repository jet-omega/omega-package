using System;
using JetBrains.Annotations;
using Omega.Package;
using Omega.Tools;
using Omega.Package.Internal;
using UnityEngine;

/// <summary>
/// Класс содержащий расширения для GameObject`ов
/// </summary>
public static class GameObjectExtensions
{
    /// <summary>
    /// Возвращает компонент, прикрепленный к объекту. Если экземпляр компонента заданного типа отсутствует на объекте
    /// то он будет добавлен к объекту. 
    /// </summary>
    /// 
    /// <param name="gameObject">Игровой объект</param>
    /// 
    /// <typeparam name="T">Тип компонента</typeparam>
    /// 
    /// <returns>Экземпляр компонента</returns>
    /// 
    /// <exception cref="ArgumentNullException">Параметр <param name="gameObject"/>>указывает на null</exception>
    /// <exception cref="MissingReferenceException">Параметр <param name="gameObject"/>>указывает на уничтоженный объект</exception>
    [NotNull]
    public static T MissingComponent<T>([NotNull] this GameObject gameObject) where T : Component
    {
        if (gameObject is null)
            throw new NullReferenceException(nameof(gameObject));
        if (!gameObject)
            throw new MissingReferenceException(nameof(gameObject));

        return GameObjectUtilities.MissingComponentWithoutChecks<T>(gameObject);
    }

    /// <summary>
    /// Пытается найти объект на компоненте, если компонент найден вернет true а <param name="component"/>>будет указывать на найденный объект,
    /// в противном случае, вернет false, а <param name="component"/>>будет указывать на null
    /// </summary>
    /// 
    /// <param name="gameObject">Игровой объект</param>
    /// <param name="component">Ссылка на найденный объект (null, если объект не найден)</param>
    /// 
    /// <typeparam name="T">Тип компонента</typeparam>
    /// 
    /// <returns>true - компонент найден, false - объект не найден</returns>
    /// 
    /// <exception cref="ArgumentNullException">Параметр <param name="gameObject"/>>указывает на null</exception>
    /// <exception cref="MissingReferenceException">Параметр <param name="gameObject"/>>указывает на уничтоженный объект</exception>
    public static bool TryGetComponent<T>([NotNull] this GameObject gameObject, [CanBeNull] out T component)
    {
        if (gameObject is null)
            throw new ArgumentNullException(nameof(gameObject));
        if (!gameObject)
            throw new MissingReferenceException(nameof(gameObject));

        return GameObjectUtilities.TryGetComponentWithoutChecks(gameObject, out component);
    }

    /// <summary>
    /// Возвращает всех потомков для игрового объекта
    /// </summary>
    /// 
    /// <returns>Массив потомков</returns>
    /// 
    /// <exception cref="NullReferenceException">Параметр <param name="gameObject"/>>указывает на null</exception>
    /// <exception cref="MissingReferenceException">Параметр <param name="gameObject"/>>указывает на уничтоженный объект</exception>
    public static Transform[] GetChilds(this GameObject gameObject)
    {
        if (gameObject is null)
            throw new NullReferenceException(nameof(gameObject));
        if (!gameObject)
            throw new MissingReferenceException(nameof(gameObject));

        return TransformUtilities.GetChildsWithoutChecks(gameObject.transform);
    }

    [CanBeNull]
    public static T GetComponentInDirectChildren<T>(this GameObject gameObject)
    {
        if (gameObject is null)
            throw new NullReferenceException(nameof(gameObject));
        if (!gameObject)
            throw new MissingReferenceException(nameof(gameObject));

        return GameObjectUtilities.GetComponentDirectWithoutChecks<T>(gameObject);
    }
    
    [NotNull]
    public static T[] GetComponentsInDirectChildren<T>(this GameObject gameObject, bool searchInRoot)
    {
        if (gameObject is null)
            throw new NullReferenceException(nameof(gameObject));
        if (!gameObject)
            throw new MissingReferenceException(nameof(gameObject));

        var result = ListPool<T>.Rent();
        GameObjectUtilities.GetComponentsDirectWithoutChecks(gameObject, result, searchInRoot);

        var resultArray = result.Count == 0 ? Array.Empty<T>() : result.ToArray();
        ListPool<T>.ReturnInternal(result);

        return resultArray;
    }

    /// <summary>
    /// Устанавливает себя в качестве потомка для transform и возвращает его
    /// </summary>
    /// 
    /// <param name="attachTo">Новый родитель</param>
    /// <param name="transform">Объект для которого нужно установить нового родителя</param>
    /// 
    /// <returns>Возвращает аргумент <param name="transform"/></returns>
    /// 
    /// <exception cref="NullReferenceException">Параметр <param name="attachTo"/> указывает на null</exception>
    /// <exception cref="MissingReferenceException">Параметр <param name="attachTo"/> или <param name="transform"/> указывает на уничтоженный объект</exception>
    /// <exception cref="ArgumentNullException">Параметр <param name="transform"/> указывает на null</exception>
    /// <exception cref="ArgumentException">transform и attachTo указывают на один и тот же компонент</exception>
    [NotNull]
    public static Transform Attach(this GameObject attachTo, [NotNull] Transform transform)
    {
        if (attachTo is null)
            throw new NullReferenceException(nameof(attachTo));
        if (!attachTo)
            throw new MissingReferenceException(nameof(attachTo));

        if (transform is null)
            throw new ArgumentNullException(nameof(transform));
        if (!transform)
            throw new MissingReferenceException(nameof(transform));

        var attachToTransform = attachTo.transform;

        // transform и attachTo указывают на один и тот же компонент
        if (transform == attachToTransform)
            throw new ArgumentException(
                $"{nameof(transform)} and {nameof(attachTo.transform)} point to the same component");

        transform.parent = attachToTransform;

        return transform;
    }

    /// <summary>
    /// Устанавливает себя в качестве потомка для gameObject и возвращает его
    /// </summary>
    /// 
    /// <param name="attachTo">Новый родитель</param>
    /// <param name="gameObject">Объект для которого нужно установить нового родителя</param>
    /// 
    /// <returns>Возвращает аргумент <param name="gameObject"/></returns>
    /// 
    /// <exception cref="NullReferenceException">Параметр <param name="attachTo"/> указывает на null</exception>
    /// <exception cref="MissingReferenceException">Параметр <param name="attachTo"/> или <param name="gameObject"/> указывает на уничтоженный объект</exception>
    /// <exception cref="ArgumentNullException">Параметр <param name="gameObject"/> указывает на null</exception>
    [NotNull]
    public static GameObject Attach(this GameObject attachTo, [NotNull] GameObject gameObject)
    {
        if (attachTo is null)
            throw new NullReferenceException(nameof(attachTo));
        if (!attachTo)
            throw new MissingReferenceException(nameof(attachTo));

        if (gameObject is null)
            throw new ArgumentNullException(nameof(gameObject));
        if (!gameObject)
            throw new MissingReferenceException(nameof(gameObject));

        var attachToTransform = attachTo.transform;
        var gameObjectTransform = gameObject.transform;

        // gameObject.transform и attachTo указывают на один и тот же компонент
        if (gameObjectTransform == attachToTransform)
            throw new ArgumentException(
                $"{nameof(gameObject.transform)} and {nameof(attachTo.transform)} point to the same component");

        gameObjectTransform.parent = attachToTransform;

        return gameObject;
    }
}