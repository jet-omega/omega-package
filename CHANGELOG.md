# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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
### Improves
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