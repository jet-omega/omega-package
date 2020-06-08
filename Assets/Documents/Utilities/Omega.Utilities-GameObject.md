# Инструменты для GameObject

`T MissingComponent<T>([NotNull] GameObject gameObject) where T : Component` - Возвращает компонент, прикрепленный к объекту. Если экземпляр компонента заданного типа отсутствует на объект, то он будет добавлен к объекту.

`Component MissingComponent([NotNull] GameObject gameObject, [NotNull] Type componentType)` - Возвращает компонент, прикрепленный к объекту. Если экземпляр компонента заданного типа отсутствует на объекте, то он будет добавлен к объекту

`bool TryGetComponent<T>([NotNull] GameObject gameObject, [CanBeNull] out T component)` - Пытается найти объект на компоненте, если компонент найден вернет true, а компонент будет указывать на найденный объект в противном случае, вернет false, а компонент будет указывать на null

`bool ContainsComponent<T>([NotNull] GameObject gameObject)` - Проверяет содержит ли объект компонент заданного типа

`Component GetComponentInDirectChildren([NotNull] GameObject gameObject, [NotNull] Type componentType, bool searchInRoot = false)` - Пытается найти компонент заданного типа среди объектов-потомков указанного объект. В отличии от GetComponentInChildren, проверка идет только по прямым потомкам объекта, не выполняя поиск по всему дереву потомков

`Component[] GetComponentsInDirectChildren([NotNull] GameObject gameObject, [NotNull] Type componentType, bool searchInRoot = false)` - Пытается найти компоненты заданного типа среди объектов-потомков указанного объекта. В отличии от GetComponentsInChildren, проверка идет только по прямым потомкам объекта, не выполняя поиск по всему дереву потомков

`T GetComponentInDirectChildren<T>([CanBeNull] GameObject gameObject, bool searchInRoot = false)` - Пытается найти компонент заданного типа среди объектов-потомков указанного объекта. В отличии от GetComponentInChildren, проверка идет только по прямым потомкам объекта, не выполняя поиск по всему дереву потомков