# Обработчики событий
Событие одно - обработчиков события может быть много. Обработчики должны содержать логику обработки события соответствующего типа. Каждый обработчик должен наследовать и реализовывать интерфейс `IEventHandler<TEvent>`
## Создание обработчика события
### Реализация интерфейса `IEventHandler<TEvent>`
Самый прямой способ создать обработчик события это реализовать интерфейс `IEventHandler<TEvent>` в пользовательском типе. Например так:
```csharp
class SomeEventHandler : IEventHandler<SomeEvent>
{
   void Execute(SomeEvent arg)
       => printf($"Hello from handler. Arg: {arg}");
}
```
`SomeEventHandler` является обработчиком события SomeEvent.
Теперь чтобы этот обработчик смог получать уведомления о событиях, необходимо сделать экземпляр этого обработчика, а после подписать этот экземпляр на уведомления с помощью класса `EventAggreagator`. Сделаем это внутри компонента:
```csharp
class SomeMonoBehaviour : MonoBehaviour
{
    private IEventHandler<SomeEvent> _currentHandler;
    
    private void Awake()
        => _currentHandler = new SomeEventHandler();

    private void OnEnable()
        => EventAggregator.AddHandler(_currentHandler);
    private void OnDisable()
        => EventAggregator.RemoveHandler(_currentHandler);
} 
```
В примере выше мы создаем экземпляр обработчика в методе `Awake`, после чего в методе `OnEnable` мы подписываем обработчика на уведомления о событиях. `OnDisable` содержит отписку от уведомлений о событии

### Использование методов в качестве обработчиков
Создавать обработчики для каждого события и со своей уникальной логикой обработки крайне ресурсозатратное и утомительное дело, и в большинстве случаев такой подход будет излишним.
Перепишем прошлый пример так, чтобы не было нужны создавать свой обработчик а использовать обычный метод прямо внури компонента `SomeMonoBehaviour`
 
```csharp
class SomeMonoBehaviour : MonoBehaviour
{
    private void SomeEventHandler(SomeEvent arg)
        => printf($"Hello from handler. Arg: {arg}");

    private void OnEnable()
        => EventAggregator.AddHandler(SomeEventHandler);
    private void OnDisable()
        => EventAggregator.RemoveHandler(SomeEventHandler);
} 
```
Такой подход гораздо более интуитивный и не требует большого кол-ва времени для его реализации. Более того, такой подход является более безопасным с точки зрения логики приложения, поскольку, если объект который содержит в себе метод, являющийся обработчиком будет уничтожен, то будет выброшено исключение, таким образом  не позволяя вызывать методы уничтоженных объектов. Хотя если такое поведение задумано и разрешено разработчиком то он может более тонко настроить это поведение с помощью атрибута EventHandler и его поля InvocationPolicy.

Используем предыдущий пример для демонстрации этого поведения. Предположим что разработчик создавший компонент `SomeMonoBehaviour` забыл реализовать удаление обработчика в `OnDisable`.
```csharp
class SomeMonoBehaviour : MonoBehaviour
{
    private void SomeEventHandler(SomeEvent arg)
        => printf($"Hello from handler. Arg: {arg}");

    private void OnEnable()
        => EventAggregator.AddHandler(SomeEventHandler);
} 
```
Таким образом если будет исполнен этот код, то на 3 строчке будет выброшено исключение.
```csharp
var gameObject = new GameObject().AddComponent<SomeMonoBehaviour>();
Object.DestroyImmediate(gameObject);
EventAggregator.Event(new SomeEvent());
```
Для того чтобы разрешить вызвать методы из уничтоженных объектов можно использовать атрибут `EventHandlerAttribute` и его поле InvocationPolicyтип которого это перечисление с 3 возможными значениями:
1. `PreventInvocationFromDestroyedObject` _(default)_ - При попытке вызывать метод у уничтоженного объекта будет выброшено исключение
2. `AllowInvocationFromDestroyedObjectButLogWarning` - Разрешает вызывать метод у уничтоженного объекта, но будет записан лог.
3. `AllowInvocationFromDestroyedObject` - Разрешает вызывать метод у уничтоженного объекта

При такой реализации `SomeMonoBehaviour` исключения выброшено не будет.
```csharp
class SomeMonoBehaviour : MonoBehaviour
{
    private void SomeEventHandler(SomeEvent arg)
        => printf($"Hello from handler. Arg: {arg}");

    [EventHandler(InvocationPolicy.AllowInvocationFromDestroyedObject)]
    private void OnEnable()
        => EventAggregator.AddHandler(SomeEventHandler);
} 
```
