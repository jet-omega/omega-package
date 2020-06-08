﻿# Инструмены для RectTransform

`RectTransform[] GetChilds([NotNull] RectTransform root)` - Возвращает всех потомков указанного трансформа, если потомок этого трансформа не кастится к RectTransfrom, то он не будет добавлен в конечный массив

`void SetRect([NotNull] RectTransform rectTransform, Rect rect)` - Устанавливает позицию и размер исходя из `Rect`