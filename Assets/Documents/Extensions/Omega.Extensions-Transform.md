# Расширения для Transform

`Transform[] GetChilds(this Transform transform)` - Возвращает всех потомков

`bool IsChildOf(this Transform self, Transform parent)` - Проверяет является ли `self` потомком `parent`

`GameObject Attach(this Transform attachTo, GameObject gameObject)` - Устанавливает себя в качестве потомка для gameObject и возвращает его

`Transform Attach(this Transform attachTo, Transform transform)` - Устанавливает себя в качестве потомка для `transform` и возвращает его

`void SetRect(this RectTransform rectTransform, Rect rect)` -  Устанавливает позицию и размер исходя из `Rect`