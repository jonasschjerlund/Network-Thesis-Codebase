# Network Codebase
Contains the codebase developed to implement a room-scale VR SteamVR-based UNET network application in Unity.

This code was developed for my MSc Computer Science thesis, "The effects of height and scale in room-scale VR network applications". Note that the repository does not contain the full Unity project; unfortunately, licensing issues with certain assets prevent me from publishing the project. The code is all here, however.

Note that I use the HTC Vive as my room-scale VR system. Specifically, I use the Interaction System which comes as part of Valve's SteamVR SDK, which is freely available on the Unity asset store: https://assetstore.unity.com/packages/tools/network/steamvr-network-essentials-63969

There is a lot of code here, and not all of it is core. Using the knowledge gained from my development process, I plan to release a condensed and refactored version of core code assets required to create a UNET networking application that integrates with the SteamVR Interaction System. In other words, for those developers who have been disappointed in the absence of tutorials that can get you started with multiplayer VR in Unity, you have something to look forward to. That release will come with a full Unity project, and will be available free of charge. Keep an eye on this repository for updates.

Here follows a short overview of select classes and code structures that might be of particular interest to readers. It will not cover those classes and systems that are already described in detail in the thesis. The thesis will be linked from here once the library of my university publishes it.

**Note for readers unfamiliar with the Unity project structure:** A class in a Unity context is known by several names: a component, a script component, a script, or a class. These terms will be used interchangably here.

All Unity assets in a Unity project, including folders as well as script components, have a .meta file associated with it. This meta-file serves various Unity-specific purposes, such as pointing and settings management. You can just peruse the .cs files for the actual code, and ignore the .meta files.

## Recurring code patterns

### The SerializeField attribute
A recurring pattern in the code will be the use of the `[SerializeField]` attribute, which is used to force Unity to serialize a private field. By default, Unity only serializes public fields. Since only serialized fields show up in the Unity editor, the SerializeField attribute is convenient for development purposes without exposing too many variables to external access from other classes in the codebase. This external exposure can still be achieved by writing a public property for the variable. 

Note for C# developers: `[SerializeField]` is an internal Unity attribute and is seperate from ordinary C# serializaton known from e.g. .NET's XML serialization.

### Custom editors
Certain included files (e.g. [DynamicHeightAdjuster](https://github.com/jonasschjerlund/Network-Thesis-Codebase/blob/master/Scripts/DynamicHeightAdjuster.cs) or [DataSystemController](https://github.com/jonasschjerlund/Network-Thesis-Codebase/blob/master/Scripts/Data%20Logging%20System/System%20Core/DataSystemController.cs) include a seperate class tagged with the `[UnityEditor.CustomEditor]` attribute. These are special classes used to define a custom appearance for a script component in the Unity editor. As such, they serve the purpose of aiding developers in using the component, and will not be visible to the end user. I use preprocessor directives to avoid that they get compiled in actual builds of the application.

### Coroutines
Whenever a method in this codebase returns an IEnumerator, it is a coroutine. This is a Unity-specific structure that behaves like a method, but can yield control to Unity and resume at a later frame based on a yield instruction. This is useful for e.g. timed effects or effects that have to routinely recur, but not every frame.
 
## Components

### Network

#### NetworkedAvatar
This script handles the networked representation of a user's avatar and is the main implementation of the avatar pattern (see thesis). The `OnStartLocalPlayer()` method I override here is invoked on each local client, ensuring that the avatar's renderers are only disabled for the local user. Then, in the `Update()` method which runs every frame, we set the position and orientation of the avatar game objects to equal the corresponding positions of the Vive objects. It should be noted, though, that the network synchronization is limited by the update rate of the avatar's Unity-provided NetworkTransform components.

#### NetworkedRole
This component designates a role to a user based on the order in which they connect to the server. This is useful for assigning different functionalities to different users. The functionality derived from these roles is then derived from corresponding script components (i.e. [PlayerOneRole](https://github.com/jonasschjerlund/Network-Thesis-Codebase/blob/master/Scripts/Avatar/Role%20Designation/PlayerOneRole.cs) and [PlayerTwoRole](https://github.com/jonasschjerlund/Network-Thesis-Codebase/blob/master/Scripts/Avatar/Role%20Designation/PlayerTwoRole.cs). Note that while only two roles have been implemented, this role designation pattern could be expanded to any number of users permitted by UNET.

#### LoadServerSceneOnKeyPress
This component allows the host (i.e. the server owner) to invoke a server-wide scene change for all clients by pressing a specific key. During the transition, it also resets key local player objects so we do not have to do that manually elsewhere with every scene change.

#### NetworkedHandAnmation
This component currently handles animation for both hands across the network. It faced a number of development issues and had to be structured this way due to UNET limitations. A cleaner solution would have a component handle animations for a single hand, and then having two of those components, one for each hand. However, this structure lead to unintended behaviour from UNET (e.g. the network updates for the two scripts would conflict). While this version is functional, future development could see work to resolve this issue and restructure the avatar game object's transform hierarchy to facilitate the hands working in this manner.

### Local

#### TeleportationCounter
This component keeps track of how many teleports the user has used throughout the length of the component's existence. It accomplishes this by subscribing to an event implemented in the Teleport singleton from the Interaction System, which is invoked every time the user teleports. Right now, it does not do much apart from keeping track of the teleports, which arguably could have been stored as an int in the Teleport singleton itself; however, it showcases a good pattern for event handling in Unity and is useful as an expandable base towards that purpose.

#### DatasetFactory
Factory implementation for the dataset system. Contains useful methods for creating and destroying datasets cleanly and ordering them in a sensible transform hierarchy.

#### HighFive
This component registers a collision between two hand objects, and also invokes an event to the fact.

#### HandAnimation
This component handles animation for a single hand as the UNET restrictions faced with the networked version of the component are not present here. While this version is functional, future development and a cleaner implementation of this component would see a base class define the animations and derived classes extend and implement the animations for local and network use, respectively.
