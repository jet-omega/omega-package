using System;
using System.Collections;
using System.Collections.Generic;
using Omega.Tools;
using UnityEngine;

public static class TransformExtensions
{
    /// <summary>
    /// Возвращает всех потомков 
    /// </summary>
    /// <returns>Массив потомков</returns>
    /// <exception cref="NullReferenceException">Параметр <param name="root"/>>указывает на null</exception>
    /// <exception cref="MissingReferenceException">Параметр <param name="root"/>>указывает на уничтоженный объект</exception>
    public static Transform[] GetChildes(this Transform transform)
    {
        if (ReferenceEquals(transform, null))
            throw new NullReferenceException(nameof(transform));
        if (!transform)
            throw new MissingReferenceException(nameof(transform));
        
        return TransformUtility.GetChildesWithoutChecks(transform);
    }
}
