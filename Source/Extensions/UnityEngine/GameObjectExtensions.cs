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
    /// Возвращает компонент, прикрепленный к объекту. Если экземпляр компонента заданного типа отсутствует на объекте
    /// то он будет добавлен к объекту. 
    /// </summary>
    /// 
    /// <param name="gameObject">Игровой объект</param>
    /// 
    /// <param name="componentType">Объект-тип компонента</typeparam>
    /// 
    /// <returns>Экземпляр компонента</returns>
    /// 
    /// <exception cref="ArgumentNullException">Параметр <param name="gameObject"/>>указывает на null</exception>
    /// <exception cref="MissingReferenceException">Параметр <param name="gameObject"/>>указывает на уничтоженный объект</exception>
    [NotNull]
    public static Component MissingComponent([NotNull] this GameObject gameObject, Type componentType)
    {
        if (gameObject is null)
            throw new NullReferenceException(nameof(gameObject));
        if (!gameObject)
            throw new MissingReferenceException(nameof(gameObject));

        return GameObjectUtilities.MissingComponentWithoutChecks(gameObject, componentType);
    }

    /// <summary>
    /// TryGetComponent that searches component in children
    /// </summary>
    /// <param name="gameObject">GameObject search started from</param>
    /// <param name="component">Result component</param>
    /// <typeparam name="T">Component type</typeparam>
    /// <returns>True if found</returns>
    /// <exception cref="NullReferenceException">GameObject is null</exception>
    /// <exception cref="MissingReferenceException">GameObject was destroyed</exception>
    public static bool TryGetComponentInChildren<T>([NotNull] this GameObject gameObject, out T component) where T : Component
    {
        if (gameObject is null)
            throw new NullReferenceException(nameof(gameObject));
        if (!gameObject)
            throw new MissingReferenceException(nameof(gameObject));
        
        component = gameObject.GetComponentInChildren<T>();
        return component != null;
    }
    
    /// <summary>
    /// TryGetComponent that searches component in children
    /// </summary>
    /// <param name="gameObject">GameObject search started from</param>
    /// <param name="componentType">Component type-object</param>
    /// <param name="component">Result component</param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException">GameObject is null</exception>
    /// <exception cref="MissingReferenceException">GameObject was destroyed</exception>
    public static bool TryGetComponentInChildren([NotNull] this GameObject gameObject, Type componentType, out Component component)
    {
        if (gameObject is null)
            throw new NullReferenceException(nameof(gameObject));
        if (!gameObject)
            throw new MissingReferenceException(nameof(gameObject));
        
        component = gameObject.GetComponentInChildren(componentType);
        return component != null;
    }

    /// <summary>
    /// TryGetComponent that searches component in parents
    /// </summary>
    /// <param name="gameObject">GameObject search started from</param>
    /// <param name="component">Result component</param>
    /// <typeparam name="T">Component type</typeparam>
    /// <returns>True if found</returns>
    /// <exception cref="NullReferenceException">GameObject is null</exception>
    /// <exception cref="MissingReferenceException">GameObject was destroyed</exception>
    public static bool TryGetComponentInParent<T>([NotNull] this GameObject gameObject, out T component) where T : Component
    {
        if (gameObject is null)
            throw new NullReferenceException(nameof(gameObject));
        if (!gameObject)
            throw new MissingReferenceException(nameof(gameObject));
        
        component = gameObject.GetComponentInParent<T>();
        return component != null;
    }
    
    /// <summary>
    /// TryGetComponent that searches component in parents
    /// </summary>
    /// <param name="gameObject">GameObject search started from</param>
    /// <param name="componentType">Component type-object</param>
    /// <param name="component">Result component</param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException">GameObject is null</exception>
    /// <exception cref="MissingReferenceException">GameObject was destroyed</exception>
    public static bool TryGetComponentInParent([NotNull] this GameObject gameObject, Type componentType, out Component component)
    {
        if (gameObject is null)
            throw new NullReferenceException(nameof(gameObject));
        if (!gameObject)
            throw new MissingReferenceException(nameof(gameObject));
        
        component = gameObject.GetComponentInParent(componentType);
        return component != null;
    }

    /// <summary>
    /// Возвращает всех потомков для игрового объекта
    /// </summary>
    /// 
    /// <returns>Массив потомков</returns>
    /// 
    /// <exception cref="NullReferenceException">Параметр <param name="gameObject"/>>указывает на null</exception>
    /// <exception cref="MissingReferenceException">Параметр <param name="gameObject"/>>указывает на уничтоженный объект</exception>
    public static Transform[] GetChildren(this GameObject gameObject)
    {
        if (gameObject is null)
            throw new NullReferenceException(nameof(gameObject));
        if (!gameObject)
            throw new MissingReferenceException(nameof(gameObject));

        return TransformUtilities.GetChildrenWithoutChecks(gameObject.transform);
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

        var result = ListPool<T>.InternalShared.Get();
        GameObjectUtilities.GetComponentsDirectWithoutChecks(gameObject, result, searchInRoot);

        var resultArray = result.Count == 0 ? Array.Empty<T>() : result.ToArray();
        ListPool<T>.InternalShared.Return(result);

        return resultArray;
    }
}
