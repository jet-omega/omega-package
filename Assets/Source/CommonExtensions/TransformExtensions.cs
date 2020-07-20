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

    public static bool ToRectTransform(this Transform self, out RectTransform rectTransform)
    {
        rectTransform = self as RectTransform;
        return rectTransform != null;
    }
    
    [Obsolete("Use GetChildren")]
    public static Transform[] GetChilds(this Transform transform) => transform.GetChildren();
    
    [Obsolete("Use GetChildren")]
    public static void GetChilds(this Transform transform, List<Transform> result) => transform.GetChildren(result);
    
    /// <summary>
    /// TryGetComponent that searches component in children
    /// </summary>
    /// <param name="transform">Transform search started from</param>
    /// <param name="component">Result component</param>
    /// <typeparam name="T">Component type</typeparam>
    /// <returns>True if found</returns>
    /// <exception cref="NullReferenceException">Transform is null</exception>
    /// <exception cref="MissingReferenceException">Transform was destroyed</exception>
    public static bool TryGetComponentInChildren<T>([NotNull] this Transform transform, out T component) where T : Component
    {
        if (transform is null)
            throw new NullReferenceException(nameof(transform));
        if (!transform)
            throw new MissingReferenceException(nameof(transform));
        
        component = transform.GetComponentInChildren<T>();
        return component != null;
    }
    
    /// <summary>
    /// TryGetComponent that searches component in children
    /// </summary>
    /// <param name="transform">Transform search started from</param>
    /// <param name="componentType">Component type-object</param>
    /// <param name="component">Result component</param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException">Transform is null</exception>
    /// <exception cref="MissingReferenceException">Transform was destroyed</exception>
    public static bool TryGetComponentInChildren([NotNull] this Transform transform, Type componentType, out Component component)
    {
        if (transform is null)
            throw new NullReferenceException(nameof(transform));
        if (!transform)
            throw new MissingReferenceException(nameof(transform));
        
        component = transform.GetComponentInChildren(componentType);
        return component != null;
    }

    /// <summary>
    /// TryGetComponent that searches component in parents
    /// </summary>
    /// <param name="transform">Transform search started from</param>
    /// <param name="component">Result component</param>
    /// <typeparam name="T">Component type</typeparam>
    /// <returns>True if found</returns>
    /// <exception cref="NullReferenceException">Transform is null</exception>
    /// <exception cref="MissingReferenceException">Transform was destroyed</exception>
    public static bool TryGetComponentInParent<T>([NotNull] this Transform transform, out T component) where T : Component
    {
        if (transform is null)
            throw new NullReferenceException(nameof(transform));
        if (!transform)
            throw new MissingReferenceException(nameof(transform));
        
        component = transform.GetComponentInParent<T>();
        return component != null;
    }
    
    /// <summary>
    /// TryGetComponent that searches component in parents
    /// </summary>
    /// <param name="transform">Transform search started from</param>
    /// <param name="componentType">Component type-object</param>
    /// <param name="component">Result component</param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException">Transform is null</exception>
    /// <exception cref="MissingReferenceException">Transform was destroyed</exception>
    public static bool TryGetComponentInParent([NotNull] this Transform transform, Type componentType, out Component component)
    {
        if (transform is null)
            throw new NullReferenceException(nameof(transform));
        if (!transform)
            throw new MissingReferenceException(nameof(transform));
        
        component = transform.GetComponentInParent(componentType);
        return component != null;
    }
}