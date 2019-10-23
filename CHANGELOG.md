# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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