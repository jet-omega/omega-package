# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.10.4] - 2020-02-19
### Fixed
- Fix incorrect concatenation routines

## [0.10.3] - 2020-02-17
### Added
- Add IsChildOf method extension for transform ([d667e91](https://github.com/ltd-profit/omega-package/commit/d667e91))
- Now you can provide progress for `Routine By Enumerator` via RoutineControl ([7a816ee](https://github.com/ltd-profit/omega-package/commit/7a816ee))
- Now you can handle force completion and cancellation for `Routine By Enumerator` via RoutineControl ([7a816ee](https://github.com/ltd-profit/omega-package/commit/7a816ee))

### Improved
- Improve stacktrace for TaskRoutines ([513c1cc](https://github.com/ltd-profit/omega-package/commit/513c1cc))
- Prelude execution routine for background routines ([bf54510](https://github.com/ltd-profit/omega-package/commit/bf54510)) 

### Changed
- Now RoutineWorker not support EndOfFrame execution order ([c30aff8](https://github.com/ltd-profit/omega-package/commit/c30aff8)) 
- Now nested routines may be canceled or contain an error ([8cdedcf](https://github.com/ltd-profit/omega-package/commit/8cdedcf))

## [0.10.2] - 2020-02-11
### Added
- Add cancellation routines. Lets you cancel processing routines
    ```csharp
    //                         Examples                         //
    // -------------------------------------------------------- //
    
    IEnumerator OperationWithTimeOut(Routine operation, TimeSpan timeout)
    {
        Routine.Delay(timeout)
            .GetSelf(out var timeoutRoutine)
            .Callback(() =>
            {
                operation.Cancel(); // cancel source operation
                Debug.LogError("operation timeout!")
            }).InBackground()
        
        yield return operation;
        timeoutRoutine.Cancel(); // cancel timeout
    }
    
    // -------------------------------------------------------- //
    //  You can define behavior of the routine when canceling   //
    
    class MyRoutine : Routine
    {
        DatabaseAccess db;
    
        // invoked when routine canceling
        protected override void OnCancel()
        {
            db.Dispose();
        }
    }
    
    // -------------------------------------------------------- //
    ```
    
- You can define cancellation logic in `TaskRoutine`
    ```csharp
    //                         Example                          //
    // -------------------------------------------------------- //
    
    Routine.Task(cancel => 
    {
        while(true)
            if(cancel.IsCancellationRequested)
                break;
    }).GetSelf(out var routine).InBackground();
              
    routine.Cancel();
        
    // -------------------------------------------------------- //
    ```
    
## Improved
- If nested routine have error or canceled then upper routine will throw exception  

## Fixed 
- Fix multiple invocation callback when you try process completed routine

## [0.10.1] - 2020-02-07
### Added
- Add extension method overload `AsRoutine` for `AsyncOperation` without `out` arg
- Add extension method  `CanBeForceComplete` for `AsyncOperation`. Lets you to determine whether the AsyncOperation can be completed synchronously

### Fixes
- Change assembly for `ObjectRoutine` class to Omega.Tools.Runtime
- Remove internal usage `Omega.Experimental.Utilities` 

## [0.10.0] - 2020-02-06
### Added
- Add extension method `ToRectTransform` for Transform. Lets you easy cast Transform to RectTransform 
    ```csharp
    //                        Examples                          //
    // -------------------------------------------------------- //

        transform.ToRectTransform(out var rectTransform);
        rectTransform.sizeDelta = Vector2.zero;
        
    // -------------------------------------------------------- //
        
        if(transform.ToRectTransform(out var rectTransform));
           rectTransform.SetRect(somethingRect);
        else throw new InvalidOperationException(); 
    
    // -------------------------------------------------------- //
    ```
- Add `ApportionedRoutine`. Lets you to distribute the big task into frames
- Add `ObjectRoutines`. Lets you create routines for async instantiation objects
    ```csharp
    //                        Example                          //
    // -------------------------------------------------------- //
    
    var factory = GameObjectFactory.Prefab(prefabItem)
        .SetParent(transform)
        .Custom(go => go.GetComponent<Item>().Init()) 
    
    // instantiate 10 000 GameObject by factory spending no more than 0.01 seconds per frame
    yield return ObjectRoutine.Instantiate(factory, 10_000, 0.01f);
    
    // -------------------------------------------------------- //
    ```
- Add `GetAllChilds` method in `TransformUtilities`. Lets you get all childs in hierarchy relative of the transform instance  
- Add utilities for `UnityEngine.Rect`
- Add extension method `SetRect` for `RectTransform`
##### Routines
- Add `WaitRoutine` (Routine.WaitOne). Lets wait the completion of the routine and provides the result 
- Add unity background worker for routines and extension method `InBackground`. Lets you start routine in background. (You can use it instead `StartCoroutine`)
    ```csharp
    //                          Example                         //
    // -------------------------------------------------------- //

        var pathToImage = ...;
        yield return Api.DownloadImage(pathToImage)
            .Result(out var imageResult);
            
        Texture2D image = imageResult;
        
        // caching image in background
        Routine.Task(()=>
        {
            var imageRaw = image.EncodeToJPG();
            File.WriteAllBytes(pathToImage, imageRaw)
        }).InBackground();
        
        itemInMenu.Image = image;
    
    // -------------------------------------------------------- //
    ```

### Improved
##### Routines
- Now all routines have virtual method `OnForcedComplete`. Lets to notify the routine of forced completion
- When the `DelayRoutine` is forcibly completion, the flow may go to thread Sleep
- When you try to forcefully complete an nested AsyncOperation in a routine that cannot be resolved synchronously, an exception will be thrown **(to avoid deadlock)**
- Add implicit operator for `ResultContainer`
    ```csharp
    //                        Example                           //
    // -------------------------------------------------------- //
    
        yield return Routine.FromResult(2020).Result(out var result);
        int year = result; // implicit cast ResultContainer<int> to int
        
    // -------------------------------------------------------- //
    ```  

### Fixed
- Fix exception throwing in `TaskRoutine` inside `Task`

### Changed
- `Omega.Experimental.Utilities` is obsolete. Now you should use `Omega.Package.Utilities`
- Rename `RoutineProgress` to `RoutineProgressHandler`
- `GetRoutine` extension method is obsolete. Now you should use `GetSelf`
- `OnChangeProgress` extension method is obsolete. Now you should use `OnProgress`

### Removed
- Remove legacy static utility classes (Replaced by `Omega.Package.Utilities`)

## [0.9.4] - 2020-01-20
### Added
- Add `GetChilds` method overload for `TransformUtilities` and transform extensions. Lets get childs without allocations
- Add `GetField` method overload for `TypeHelper`. Lets quickly find one field
- Add methods for getting attributes on types in `TypeHelper`.

## [0.9.3] - 2019-12-31
### Added
- Add `InstanceFactory`. Lets instantiate objects of the type, its work very quickly with value types
- Add `TypeHelper`. Lets fast get fields and get generic types

### Improved
##### ListPool
- Method `Push` is obsolete, use `Return`
- Add read-only property `PoolSize`. Lets get pool size
- Add method `Flush`. Lets clean pool
- Add method `ReturnToArray`. Lets return list to pool and create list copy by array
- Add method `Get`. Lets use directive `using` for list from pool
- Value of const `DefaultListCapacity` were downgrade from 20 to 10

## [0.9.2] - 2019-12-08
### Added
- New `ListPool`. Lets rent lists from pool

## [0.9.1] - 2019-11-25
### Fixes
- Fix incorrect work routine with nested AsyncOperation ([40fd2c0](https://github.com/ltd-profit/omega-package/commit/40fd2c0bdd2af34501e033c645a706174c20a369))

## [0.9.0] - 2019-11-24
### Added
- Add overload method Complete for Routine with timeout argument. Lets setup timeout for some routine
- Add extension methods for `AsyncOperation`. :
  - `GetSelf` method lets get out async operation instance
  - `AsRoutine` method lets create routine from async operation
  - `AsRoutine` method overload with `Func<TAsyncOperation, TResult>` argument
   lets create routine with result from specific async operation
- Add progress for routines. Lets you to get the routine progress in float format from 0 to 1
- Add `GetComponentInDirectChildren` extension methods for `GameObject` 

   
### Removed 
- Remove routine utilities from utilities aggregator

### Improved
- Now the routines themselves determine the sequence of execution of the
 *routines / enumerators / async operations* embedded in it. So to update the routine just call the `MoveNext` method
- New internal ListPool. Lets create tools that create fewer allocations
- Optimized `GetChilds` method for `RectTransform` *(by ListPool)*
- Optimized `ClearChilds` method for `Transform` *(by ListPool)*
- Optimized routine concatenations

### Fixes
- Incorrect processing of nested *routines / enumerators / async operations* that are simultaneously update from outside

## [0.8.3] - 2019-11-19
### Added
- New `ByAction` static methods in `Routine`. Lets create routines by delegates `Action` and `Func<T>` 
- New `Empty` static methods in `Routine`. Lets create empty uncompleted routines
- New `FromCompleted<T>` static methods in `Routine`. Lets create completed routines with result

## [0.8.2] - 2019-11-18
### Fixes 
- Fix callback invocation when routine enumerator is null
- Fix callback invocation from `FromResultRoutine` (#86)

### Improved
- Now if you try to add a callback to a completed or error routine, an exception will be thrown 

## [0.8.1] - 2019-11-14
### Added 
- Assembly attributes for `Omega.Tools.Runtime`
- Additional information about the web request when an error occurs in `WebRequestRoutine`
- New `GetComponentsInDirectChildren` methods. Lets find components in direct childs in `GameObject`

### Improved
- Removed restrictions for type generic argument in method `TryGetComponent`
- Change equals operator from UnityObject.Object to System.Object for internal methods in GameObjectUtilities

## [0.8.0] - 2019-11-11
### Added 
- New web routines. Lets use `UnityWebRequest` as routine
- New convertions routines. Lets convert result from `Routine<T1>` to `Routine<T2> `

### Fixes
- Fix infinity loop in `DelayRoutine` (#67)
- Solve compilation warnings in unit-test assemblies 

### Deleted
- Delete `CREATION_STACK_TRACE` word. Now you use only `CreationStackTrace` method in routine

## [0.7.4] - 2019-11-11
### Fiexes
- Fix ignoring AsyncOperation nested in routine (#69)  

## [0.7.3] - 2019-11-09
### Added
- New extension method `Complete` for `Routine`. Lets you to expect the execution of a routine in synchronous
- New `WaitResult` method for `Routine<T>`. Lets you to expect the result of a routine in synchronous
- New `FromCompleted` static method for Routine. Lets you create completed routines

### Improved
- New RoutineError ad RoutineNotComplete exceptions

## [0.7.2] - 2019-11-09
### Fixes
- Fix `GroupRoutine` (`WhenAll` method). Now `GroupRoutine` process nested routines

## [0.7.1] - 2019-11-06 
### Fixes
- Fix build error in `Assets/Source/Experimental/Event/Internals/EventManagers/SceneEventManager.cs`

## [0.7.0] - 2019-11-02
### Added
- New `ByEnumerator` methods. For creating routine from Unity coroutine
- New `FromResultRoutine`. For creating routines with results.
- Custom pipeline elements for `GameObjectFactories`
- New `Utilities`. For aggregate utilities in your project in one place

### Improved
- Refactoring routine classes
- Refactoring tests
- Now you can routine can remember the place in which it was created. (to improve the debugging process)

### Changed
- Change url. From github/omega-vr-ar/unity-tools to github/ltd-profit/omega-package

### Deleted
- delete class `EventManagerUtility`
- delete class `ComponentUtility`
- delete class `ReflectionUtility`

### Fixes
- Fix error when call `ParallelsRoutines` in `GroupRoutine` *(#49)*

## [0.6.0] - 2019-10-27
### Added
- New `GroupRoutine`. Allows you to group several routines into one
- New `DelayRoutine`. Lets make a delay *(based on Time.unscaledTime)*.
- New `StartTask` in TaskRoutine. This method immediately starts the task.
- The ability to groups routines by plus operator

### Improved
- Implicit statement overridden. Allows you to determine if the task is not processing (completed or error)
- `EventAsync` method in `EventAggregator` support Routines

### Changed
- Rename `OtherThreadRoutine` to `TaskRoutine`

### Remove
- `GetChildes` methods

## [0.5.0] - 2019-10-23
### Added
- Routines
- Routines for System.IO
### Improved
- Improve messages in exceptions and logs
### Changed
- Rename functions `GetChildes` to `GetChilds`   
- Namespace refactoring
- Rename `IEventHandler<TEvent>.Execute` to `IEventHandler<TEvent>.OnEvent`
##### ProjectSettings
- Rename package name (from omega.tools to com.ltd-profit.unity.omega-package)
- Rename company name 

## [0.4.0] - 2019-10-15
### Added
- Runtime unit-tests
- UnityHandlerAdapter 
- GameObjectFactory
- HandlerRunners
- Documents

### Changed
- Rename InvocationConvention to InvocationPolicy
- HandlerAdapters
- EventManager sending logic 
- Omega.Events can be running only in runtime
- Unit-tests for Omega.Events moved to runtime tests
- Coverage Events Attributes

## [0.3.0] - 2019-10-11
### Added
##### EventSystem (Experimental)
- EventAggregator
- GlobalEventAttribute
- SceneEventAttribute
- ActionHandlerAdapterBuilder
- ActionHandlerAdapter
- ActionHandlerUnityAdapter
- EventHandlerAttribute
- InvocationConvention
- EventScheduler
- IEventHandler
- IEventManager

##### ReflectionUtility
- ContainsInterfaceDefinition
- GetGenericArgumentsOfDefinitionInterface

### Changed
- All runtime unit-tests were converted to edittime tests
- ReflectionUtiltiyTests 

## [0.2.0] - 2019-10-03
### Added
##### RectTransform Tools
- GetChildes in RectTransformUtility
- GetChildes in RectTransformExtensions *(base on RectTransformUtility)*
##### BoxCollider Tools
- SetBounds(Bounds bounds) in BoxColliderExtensions *(based on BoxColliderUtility)*
- SetBounds(BoxCollider boxCollider, Bounds bounds) in BoxColliderUtility
##### Other
- Attach methods in GameObjectExtensions
- Attach methods in TransformExtensions

### Changes
- Improve GetChildes int TransformUtility
- Reformat comments in GameObjectExtensions and TransformExtensions

### Fixes
- Destroy garbage after running tests

## [0.1.0] - 2019-09-25
### Added
- TransformUtility
- TransformExtensions
- TransformUtilityTests
- Add GetChildes from TransformUtility function in GameObjectExtension
### Fixes
- Compile fix in file GameObjectExtension.cs
- Fix name testing game object in
 GameObjectUtilityTests.MissingComponentShouldThrowMissingReferenceExceptionWhenParameterIsDestroyedGameObjectTest

## [0.0.1] - 2019-09-25
- Initial Release