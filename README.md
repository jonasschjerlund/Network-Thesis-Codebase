# Network Codebase
Contains the codebase developed to implement a room-scale VR SteamVR-based UNET network application in Unity.

This code was developed for my MSc Computer Science thesis, "The effects of height and scale in room-scale VR network applications". Note that the repository does not contain the full Unity project; unfortunately, licensing issues with certain assets prevent me from publishing the project. The code is all here, however.

Note that I use the HTC Vive as my room-scale VR system. Specifically, I use the Interaction System which comes as part of Valve's SteamVR SDK, which is freely available on the Unity asset store: https://assetstore.unity.com/packages/tools/network/steamvr-network-essentials-63969

There is a lot of code here, and not all of it is core. Using the knowledge gained from my development process, I plan to release a condensed and refactored version of core code assets required to create a UNET networking application that integrates with the SteamVR Interaction System. In other words, for those developers who have been disappointed in the absence of tutorials that can get you started with multiplayer VR in Unity, you have something to look forward to. That release will come with a full Unity project, and will be available free of charge. Keep an eye on this repository for updates.

Here follows a short overview of the code that might be of particular interest to readers.

**Note for readers unfamiliar with the Unity project structure:** All Unity assets, including so-called script components that in common CS terminology are the C# classes you are probably interested in, have a so-called meta-file associated with it. This meta-file serves various Unity-specific purposes, such as files pointing and asset settings management. You can just peruse the .cs files for the actual code, and ignore the .meta files.

TODO: Write code guide
