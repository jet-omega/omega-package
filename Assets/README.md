# omega-package

**omega-package** is a assembly of tools and systems for working with scripting in the Unity

## Contents

- [Motivation](#Motivation)
- [Examples](#Examples)
- [Installation](#Installation)

## Motivation

- Facilitate and speed up the development process
- Improving Unity asynchronous programming capabilities
- Creating a global event system
- Create a single assembly for extensions

## Examples

### Extensions
 
`MissingComponent` for `GameObject`
 
```csharp
var gameObject = Instantiate(prefab);

var controller = gameObject.MissingComponent<ControlScript>();
 ```
 
`TryGetComponent` for `GameObject`

```csharp
var gameObject = Find("MyGameObject");

if(gameObject.TryGetComponent<WeaponController>(out var weaponController))
    print($"Current weapon: {weaponController.weapon.name}")
```

`GetChilds` for `Transform` and `GameObject`

```csharp
var gameObject = Instantiate(prefab);

var childs = gameObject.GetChilds();
print(childs.Length);
```

`Attach` for `Transform` and `GameObject` 

```csharp
var gameObject1 = Instantiate(prefab1);
var gameObject2 = Instantiate(prefab2);

// gameObject2 will become a child for gameObject1
gameObject1.Attach(gameObject2);
```

### GameObjectFactory

A factory is needed to facilitate the creation of new GameObjects. See more [here](https://github.com/omega-vr-ar/unity-tools/wiki/GameObjectFactory)

```csharp
// Factory creation
var factory = GameObjectFactory.New().SetName("Created by Factory");

// An object named "Created by Factory" is created.
var oneInstance = factory.Build(); 

// 10 objects will be created with the name "Created by Factory"
var tenInstance = factory.Build(10); 

// An object named "Created by Factory" is created and the `Transform` component is obtained from this object
var oneInstanceToTransfrom = factory.Build<Transfron>();

// 10 objects with the name "Created by Factory" will be created and the `Transfrom` component will be obtained from each object.
var tenInstanceToTransfrom = factory.Build<Transfron>(10);
```

### Routines

Routines make it easy and efficient to organize asynchronous code in your Unity project

### Events

Events allow you to organize communication between components / systems within the application

## Installation

1. You will need [UpmGitExtension](https://github.com/mob-sakai/UpmGitExtension) to work with OmegaTools as with UnityPackage.
2. Open in Unity **Window>PackageManager** 
![package manager](https://user-images.githubusercontent.com/48410898/85272127-6e73ff80-b484-11ea-9c29-d5907be84034.png)
3. In the window that opens, click on the button with the Git icon (it is located near the button with a plus sign)
![git icon](https://user-images.githubusercontent.com/48410898/85277808-a54e1380-b48c-11ea-95af-a6e0e0b9af01.png)
4. In the menu that opens, enter the address to the repository ( https://github.com/ltd-profit/omega-package.git ), then click the "FindVersions" button, then select the latest version (or one of the branches with the suffix "upm")
![find version button](https://user-images.githubusercontent.com/48410898/85278724-fad6f000-b48d-11ea-95cb-7d7524cdab2a.png)
5. The last step is to click "Install Package"
![install package button](https://user-images.githubusercontent.com/48410898/85279692-84d38880-b48f-11ea-973e-676b78cb0a62.png)