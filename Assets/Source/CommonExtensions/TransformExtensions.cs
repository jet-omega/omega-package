using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Omega.Tools;
using Omega.Tools.Experimental.UtilitiesAggregator;
using UnityEngine;

public static class TransformExtensions
{
    /// <summary>
    /// Возвращает всех потомков 
    /// </summary>
    /// 
    /// <returns>Массив потомков</returns>
    /// 
    /// <exception cref="NullReferenceException">Параметр <param name="root"/>>указывает на null</exception>
    /// <exception cref="MissingReferenceException">Параметр <param name="root"/>>указывает на уничтоженный объект</exception>
    public static Transform[] GetChilds(this Transform transform)
    {
        if (transform is null)
            throw new NullReferenceException(nameof(transform));
        if (!transform)
            throw new MissingReferenceException(nameof(transform));

        return TransformUtilities.GetChildsWithoutChecks(transform);
    }

    public static void GetChilds(this Transform transform, List<Transform> result)
    {
        if (transform is null)
            throw new NullReferenceException(nameof(transform));
        if (!transform)
            throw new MissingReferenceException(nameof(transform));

        if(result is null)
            throw new ArgumentNullException(nameof(result));
        
        TransformUtilities.GetChildsWithoutChecks(transform, result);
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
    public static GameObject Attach(this Transform attachTo, [NotNull] GameObject gameObject)
    {
        if (attachTo is null)
            throw new NullReferenceException(nameof(attachTo));
        if (!attachTo)
            throw new MissingReferenceException(nameof(attachTo));

        if (gameObject is null)
            throw new ArgumentNullException(nameof(gameObject));
        if (!gameObject)
            throw new MissingReferenceException(nameof(gameObject));

        var gameObjectTransform = gameObject.transform;

        // gameObject.transform и attachTo указывают на один и тот же компонент
        if (gameObjectTransform == attachTo)
            throw new ArgumentException(
                $"{nameof(gameObject.transform)} and {nameof(attachTo)} point to the same component");

        gameObjectTransform.parent = attachTo;

        return gameObject;
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
    public static Transform Attach(this Transform attachTo, [NotNull] Transform transform)
    {
        if (attachTo is null)
            throw new NullReferenceException(nameof(attachTo));
        if (!attachTo)
            throw new MissingReferenceException(nameof(attachTo));

        if (transform is null)
            throw new ArgumentNullException(nameof(transform));
        if (!transform)
            throw new MissingReferenceException(nameof(transform));

        // transform и attachTo указывают на один и тот же компонент
        if (transform == attachTo)
            throw new ArgumentException(
                $"{nameof(transform)} and {nameof(attachTo)} point to the same component");

        transform.parent = attachTo;

        return transform;
    }
}