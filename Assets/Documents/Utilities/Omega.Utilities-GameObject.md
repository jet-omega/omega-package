# Инструменты для GameObject

`T MissingComponent<T>(GameObject gameObject)` - Возвращает компонент, прикрепленный к объекту. Если экземпляр компонента заданного типа отсутствует на объект, то он будет добавлен к объекту

`Component MissingComponent(GameObject gameObject, Type componentType)` - Возвращает компонент, прикрепленный к объекту. Если экземпляр компонента заданного типа отсутствует на объекте, то он будет добавлен к объекту

`bool TryGetComponent<T>(GameObject gameObject, out T component)` - Пытается найти компонент типа T на объекте. Если компонент присутствует, то значением параметра `component` станет ссылка, указывающая на компонент, а метод вернет `true`. В противном случае `component` будет `null`, а возвращаемое значение - `false`

`bool ContainsComponent<T>(GameObject gameObject)` - Проверяет содержит ли объект компонент заданного типа

`Component GetComponentInDirectChildren(GameObject gameObject, Type componentType, bool searchInRoot = false)` - Возвращает компонент потомка первого уровня `GameObject`.

`Component[] GetComponentsInDirectChildren(GameObject gameObject, Type componentType, bool searchInRoot = false)` - Возвращает компоненты всех потомков первого уровня `GameObject`.

`T GetComponentInDirectChildren<T>(GameObject gameObject, bool searchInRoot = false)` - возвращает компоненты всех потомков первого уровня `GameObject` указанного типа.
