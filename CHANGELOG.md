# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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