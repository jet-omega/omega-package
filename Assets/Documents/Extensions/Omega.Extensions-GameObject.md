# Расширения для GameObject

`T MissingComponent<T>(this GameObject gameObject): Component` - Возвращает компонент, прикрепленный к объекту. Если экземпляр компонента заданного типа отсутствует на объекте, то он будет добавлен к объекту

`bool TryGetComponent<T>(this GameObject gameObject, out T component)` - Пытается найти объект на компоненте, если компонент найден вернет true, а компонент будет указывать на найденный объект, в противном случае, вернет false, а компонент будет указывать на null

`Transform[] GetChilds(this GameObject gameObject)` - Возвращает всех потомков для GameObject

`T GetComponentInDirectChildren<T>(this GameObject gameObject)` - Возвращает компонент от прямого потомка GameObject

`T[] GetComponentsInDirectChildren<T>(this GameObject gameObject, bool searchInRoot)` - Возвращает компоненты от прямых потомков GameObject

`Transform Attach(this GameObject attachTo, Transform transform)` - Устанавливает себя в качестве потомка для `transform` и возвращает его

`GameObject Attach(this GameObject attachTo, GameObject gameObject)` - Устанавливает себя в качестве потомка для gameObject и возвращает его