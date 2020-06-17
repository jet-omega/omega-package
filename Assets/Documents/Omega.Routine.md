# Описание

`Coroutine` (далее "Корутина") в Unity — аналог функции, способный прерываться во время выполнения и возвращать контроль Unity. Задаются корутины с помощью `IEnumerator` - точки в которых нужно вернуть контроль определяются оператором с контекстным словом: `yield return`

Routine` (далее "Рутина") реализует схожую логику, но с некоторыми отличиями

Корутина Unity переходит к следующему шагу и исполняется каждый кадр под управлением Unity. Для создания вложенности корутин необходимо чтобы значение `Current` было `IEnumerator`, т.е. вернуть перечислитель через `yield return`. При этом вызов `MoveNext` не выполняет вход во вложенную корутину. То есть ручное управление корутиной наиболее вероятно сломает ее. 

Рутина выполняется независимо от Unity, а вызов `MoveNext` производит вызов `MoveNext` у всех вложенных рутин, если текущее значение `Current` ею является. 

**Например:** ожидание 5 секунд и вывод сообщения

Отметим, что `WaitForSeconds` является вложенной рутиной

```csharp
IEnumerator Enumerator()
{
    // Ждем когда пройдет 5 секунд 
    yield return new WaitForSeconds(5);
    Debug.Log("Complete!")
}
  ```      

Если использовать этот Enumerator как корутину Unity, вызвав `StartCoroutine(Enumerator())`, то как и ожидается, через 5 секунд будет выведено сообщение "Complete!" 

Однако попытки вручную выполнить этот код приведут к тому что вложенная рутина (ожидание 5 секунд) не выполнится

```csharp
         var enumerator = Enumerator();
         while(enumerator.MoveNext())
         { }
```     
         
Цикл прошел одну итерацию и сразу вывелось сообщение о завершении
         
Попробуем сделать то же самое с помощью рутин (`Omega.Routine`): 

```csharp         
         var routine = Routine.ByEnumerator(Enumerator());
         var enumerator = routine as IEnumerator;
         while(enumerator.MoveNext())
         { }
```         
         
В таком случае ожидание 5 секунд произошло, поскольку был рутина произвела выполнение вложенных рутин
         
Поскольку вручную управлять выполнением корутин невозможно, то `IEnumerator.Current` должен всегда быть `null`, чтобы Unity обрабатывала верхнюю рутину, а не вложенную.

# Виды рутин

`ApportionedRoutine` - позволяет выполнять распределённую работу, удерживая необходимое время кадра

**Пример:**

```csharp
    var result = new GameObject[count];
    var routine = new ApportionedRoutine(i =>
    {
        result[i] = factory.Build();
    }, count, timePerFrame, measureFrequency);
```

`Routine.ByAction` - рутины выполняющие действие

`Routine.ByEnumerator` - возвращают Routine'у из Enumerator'а

**Пример:**

```csharp
    return Routine.ByEnumerator<string>(Enumerator);
```

`Routine.Delay` - возвращает рутину, которая выполняется с задержкой 

`Routine.FromComplited` - возвращает завершённую рутину

`Routine.FromResult` - возвращает рутину имеющую результат

`Routine.Task` - возвращает рутину, выполняющую действие согласно TaskScheduler

**Пример:** 

```csharp
    Routine WriteAllTextRoutine(string path, string text)
        => Routine.Task(() => File.WriteAllText(path, text));

    Routine<string> ReadAllTextRoutine(string path)
        => Routine.Task(() => File.ReadAllText(path));
```

`Routine.WaitOne` - возвращает рутину с ожиданием завершения хотя бы одного действия

**Пример:**

```csharp
    return Routine.WaitOne(routine,
        () => new DownloadRawFileResponseParams {File = routine.GetResult()});
```

`Routine.WhenAll` - возвращает рутину с ожиданием завершения всех действий 
