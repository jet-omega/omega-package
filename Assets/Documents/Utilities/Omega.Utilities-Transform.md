﻿# Инструменты для Transform

`void DestroyChildren(Transform root)` - Уничтожает всех потомков переданного трансформа

`bool IsChildOf(Transform transform, [CanBeNull] Transform parent)` - Проверяет является ли `transform` потомком `parent`

`int GetAllChildsCount(Transform root)` - Возвращает количество детей переданного трансформа

`Transform[] GetChildren(Transform root)` - Возвращает всех потомков переданного трансформа

Аналогичные методы без выделения памяти

`void GetChildren(Transform root, List<Transform> result)`

`void GetAllChilds(Transform root, List<Transform> result)`