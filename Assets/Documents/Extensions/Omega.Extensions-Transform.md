# Расширения для Transform

`Transform[] GetChilds(this Transform transform)` - Возвращает всех потомков

`bool IsChildOf(this Transform self, Transform parent)` - Проверяет является ли `self` потомком `parent`

`GameObject Attach(this Transform attachTo, GameObject gameObject)` - Устанавливает себя в качестве потомка `attachTo` и возвращает `gameObject`

`Transform Attach(this Transform attachTo, Transform transform)` - Устанавливает себя в качестве потомка `attachTo` и возвращает `transform`

`void SetRect(this RectTransform rectTransform, Rect rect)` -  Устанавливает позицию и размер по `Rect`