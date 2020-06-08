﻿# Инструменты для Transform

`void ClearChilds(Transform root)` - Уничтожает всех потомков переданного трансформа

`bool IsChildOf(Transform transform, [CanBeNull] Transform parent)` - Проверяет является ли `transform` потомком `parent`

`int GetAllChildsCount(Transform root)` - Возвращает количество детей переданного трансформа

`Transform[] GetChilds(Transform root)` - Возвращает всех потомков переданного трансформа

Анологичные методы без аллокации памяти

`void GetChilds(Transform root, List<Transform> result)`

`void GetAllChilds(Transform root, List<Transform> result)`