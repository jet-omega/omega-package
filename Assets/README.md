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
 `MissingComponent` for GameObject
 ```csharp
 var gameObject = Instantiate(prefab);

var controller = gameObject.MissingComponent<ControlScript>();
 ```
 
 `TryGetComponent` for GameObject
 ```csharp
 var gameObject = Find("MyGameObject");

if(gameObject.TryGetComponent<WeaponController>(out var weaponController))
    print($"Current weapon: {weaponController.weapon.name}")
```

`GetChilds` for Transform and GameObject
```csharp
var gameObject = Instantiate(prefab);

var childs = gameObject.GetChilds();
print(childs.Length);
```

`Attach` for Transform and GameObject 
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
![image](https://user-images.githubusercontent.com/10897900/65597589-28941480-dfa2-11e9-8d43-b2ae5f335e42.png)
3. In the window that opens, click on the button with the Git icon (it is located near the button with a plus sign)
![image](https://user-images.githubusercontent.com/10897900/65597637-482b3d00-dfa2-11e9-908c-4a1426f91a5f.png)
4. In the menu that opens, enter the address to the repository ( https://github.com/omega-vr-ar/unity-tools.git ), then click the "FindVersions" button, then select the latest version (or one of the branches with the suffix "upm")
![image](https://user-images.githubusercontent.com/10897900/65597688-642ede80-dfa2-11e9-9af1-e49a5d12e270.png)
5. The last step is to click "Install Package"
![image](https://user-images.githubusercontent.com/10897900/65597714-70b33700-dfa2-11e9-90ae-0b5977f6277b.png)
