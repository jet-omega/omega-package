# Расширения для GameObject

`T MissingComponent<T>(this GameObject gameObject)` - Возвращает компонент, прикрепленный к объекту. Если экземпляр компонента заданного типа отсутствует на объекте, то он будет добавлен к объекту

`bool TryGetComponent<T>(this GameObject gameObject, out T component)` - Пытается найти компонент типа T на объекте. Если компонент присутствует, то значением параметра `component` станет ссылка, указывающая на компонент, а метод вернет `true`. В противном случае `component` будет `null`, а возвращаемое значение - `false`

`Transform[] GetChilds(this GameObject gameObject)` - Возвращает всех потомков `GameObject`

`T GetComponentInDirectChildren<T>(this GameObject gameObject)` - Возвращает компонент потомка первого уровня `GameObject`

`T[] GetComponentsInDirectChildren<T>(this GameObject gameObject, bool searchInRoot)` - Возвращает компоненты всех потомков первого уровня `GameObject`

`Transform Attach(this GameObject attachTo, Transform transform)` - Устанавливает себя в качестве потомка `attachTo` и возвращает `transform`

`GameObject Attach(this GameObject attachTo, GameObject gameObject)` - Устанавливает себя в качестве потомка `attachTo` и возвращает `gameObject`