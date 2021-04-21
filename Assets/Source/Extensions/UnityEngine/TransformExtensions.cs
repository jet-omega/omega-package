using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Omega.Tools;
using Omega.Package.Internal;
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
    public static Transform[] GetChildren(this Transform transform)
    {
        if (transform is null)
            throw new NullReferenceException(nameof(transform));
        if (!transform)
            throw new MissingReferenceException(nameof(transform));

        return TransformUtilities.GetChildrenWithoutChecks(transform);
    }

    public static void GetChildren(this Transform transform, List<Transform> result)
    {
        if (transform is null)
            throw new NullReferenceException(nameof(transform));
        if (!transform)
            throw new MissingReferenceException(nameof(transform));

        if (result is null)
            throw new ArgumentNullException(nameof(result));

        TransformUtilities.GetChildrenWithoutChecks(transform, result);
    }

    public static bool IsChildOf(this Transform self, Transform parent)
    {
        if(self is null)
            throw new NullReferenceException(nameof(self));
        if (!self)
            throw new MissingReferenceException(nameof(self));

        return TransformUtilities.IsChildOfWithoutChecks(self, parent);
    }

    public static void SetRect(this RectTransform rectTransform, Rect rect)
    {
        if (rectTransform is null)
            throw new NullReferenceException(nameof(rectTransform));
        if (!rectTransform)
            throw new MissingReferenceException(nameof(rectTransform));

        RectTransformUtilities.SetRectWithoutChecks(rectTransform, rect);
    }
}