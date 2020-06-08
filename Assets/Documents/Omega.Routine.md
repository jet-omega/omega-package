# Описание

`Корутины (Coroutines, сопрограммы) в Unity` — простой и удобный способ запускать функции, которые должны работать параллельно в течение некоторого времени.

`Routine` отличается от корутины Unity тем что рутина выполняется самостоятельно, то есть, всю рутину можно полностью выполнить вызовами MoveNext при этом все вложенные рутины также будут выполнены в случае с корутинами Unity все немного сложнее, у вас нет гарантий что ваши вызовы `MoveNext` не сломают вложенность корутины, так как все вложенные корутины, а так же вложенные асинхронные операции решает сама Unity (внутри `StartCoroutine`).

Допустим у нас есть такой `Enumerator`:

```csharp
IEnumerator Enumerator()
{
    // Ждем когда пройдет 5 секунд 
    yield return new WaitForSeconds(5);
    Debug.Log("Complete!")
}
  ```      

Если мы будем использовать этот Enumerator как корутину Unity 
и сделаем вызов `StartCoroutine(Enumerator())`, то как и ожидается, через 5 секунд будет залоггировано "Complete!" 
Однако если мы уберем знание о том что это корутина и сделаем что-то такое: 

```csharp
         var enumerator = Enumerator();
         while(enumerator.MoveNext())
         { }
```     
         
То тогда мы также увидим сообщение "Complete!", однако 5 секунд не пройдет, так как никто их не подождал.  
то есть в цикле приведенном выше будет всего одна итерация (так как у нас один `yield return` внутри `Enumerator`) 
         
Попробуем сделать то же самое с помощью рутин (`Omega.Routine`): 

```csharp         
         var routine = Routine.ByEnumerator(Enumerator());
         var enumerator = routine as IEnumerator;
         while(enumerator.MoveNext())
         { }
```         
         
Теперь мы получим задержку в заветные 5 секунд и только после этого увидим сообщение
         
Unity внутри `StartCoroutine` сама обрабатывает все вложенные `IEnumerator`-ы и мы никак не можем на это повлиять, поэтому `IEnumerator.Current` должна всегда возвращать null, чтобы Unity всегда обрабатывала верхнюю рутину а не внутреннею.

# Виды рутин

`ApportionedRoutine` - позволяет выполнять распределённую работу, не выходя за определённое время за кадр

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

`Routine.FromComplited` - создать завершённую рутину

`Routine.FromResult` - создает рутину по завершению задачи

`Routine.Task` - возвращает рутину, выполняющую действие согласно TaskScheduler.

**Пример:** 

```csharp
    Routine WriteAllTextRoutine(string path, string text)
        => Routine.Task(() => File.WriteAllText(path, text));

    Routine<string> ReadAllTextRoutine(string path)
        => Routine.Task(() => File.ReadAllText(path));
```

`Routine.WaitOne` - возвращает рутину с ожиданием завершения действия

**Пример:**

```csharp
    return Routine.WaitOne(routine,
        () => new DownloadRawFileResponseParams {File = routine.GetResult()});
```

`Routine.WhenAll` - вызов рутины после завершения группы рутин