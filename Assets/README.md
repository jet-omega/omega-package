# omega-package _(preview)_

Omega packages is a assembly of tools and utils for simplify workflow with unity

## Installation
1. Go to Unity and open the Package Manager, and click `Add package from the git URL...`.
2. Insert a link to the package (_https://github.com/ltd-profit/omega-package.git#upm_), click `Add` and the package will be installed.

## Usage 

#### Extensions
|Type|Name|Description|
|---|---|---|
|`List<T>`|`TryFind`| Вернет `true`, если элемент соответствующий заданному предикату найден в в списке, иначе - `false`. Если элемент был найден то в параметр `item` будет записано найденное значение, в противном случае - значение по-умолчанию|
|`MemberInfo`|`GetReturnType`| Возвращает возвращаемый тип члена. Например если `MemberInfo` описывает поле то вернется тип этого поля, если это свойство то тип свойства, если метод то тип возвращаемого значения|
|`MemberInfo`|`SetMemberValue`|Устанавливает значение|
|`MemberInfo`|`GetMemberValue`|Получает значение|
|`GameObject`|`MissingComponent`|Возвращает компонент, прикрепленный к объекту. Если экземпляр компонента заданного типа отсутствует на объекте то он будет добавлен к объекту.
|`GameObject`|`TryGetComponentInChildren`|
|`GameObject`|`TryGetComponentInParent`|
|`GameObject`|`GetComponentInDirectChildren`|
|`GameObject`|`GetComponentsInDirectChildren`|
|`Transform`|`GetChildren`|
|`Transform`|`IsChildOf`|
|`RectTransform`|`SetRect`|

#### Utilities
|Category|Name|Description|
|---|---|---|
|`Transform`|`DestroyChildren`||
|`Transform`|`GetAllChildren`||
|`Transform`|`GetAllChildrenCount`||
|`RectTransform`|`GetChildren`||
|`Rect`|`BetweenTwoPoints`||
|`Object`|`AutoDestroy`||
|`T[]`|`Add`||
|`T[]`|`AddRange`||
|`T[]`|`Remove`||
|`T[]`|`RemoveAt`||
|`T[]`|`RemoveAll`||
|`T[]`|`Insert`||
|`T[]`|`ArrayEquals`||
|`T[]`|`ArrayReferenceEquals`||
|`T[]`|`Contains`||
|`T[]`|`Clear`||
|`T[]`|`Sort`||
|`T[]`|`BinarySearch`||
|`Layer`|`LayersInMask`||
|`XXX`|`XXX`||

#### Pools
1. interface `IPool<T>`
2. class `ListPool<TElement>`; `ListPool<TElement>.Shared`
3. struct `PoolElementUsageHandler` for pooled object handling with `using` statement

#### RichTextBuilder & RichTextFluent 
```c#
 RichTextFactory.New()
        .Bold.Color(Color.red).Text("TITLE")
        .NewLine.Default.Text("something text block")
        .NewLine.Italic.Color(Color.gray).Text("(created by omega)")
        .NewLine.DefaultStyle.Size(50).Text("lerge text")
        .NewLine.UnstyledText("text without syle")
        .ToString();
    
     ▾   ▾   ▾   ▾   ▾   ▾   ▾   ▾   ▾   ▾   ▾   ▾   ▾   ▾   ▾   ▾
  
    "<b><color=#ff0000ff>TITLE</color></b>" +
    "\nsomething text block" +
    "\n<i><color=#808080ff>(created by omega)</color></i>" +
    "\n<color=#808080ff><size=50>lerge text</size></color>" +
    "\ntext without syle"
```
#### Editor
1. Copy Scene Path in hierarchy menu.

![image](https://user-images.githubusercontent.com/10897900/115564090-d1ab0a00-a2c0-11eb-91fb-f9b88e15658c.png)
