# VirtuLearn (VR as an environment for learning)
Unity project for experimenting with VR ideas designed for learning. [![Build Status](https://travis-ci.org/scscgit/VR-as-an-environment-for-learning.svg?branch=master)](https://travis-ci.org/scscgit/VirtuLearn) (Unity 5 build)

The project:
* Is migrated to Unity 2019 after being created in [Unity Personal version 5.4.4f1](https://unity3d.com/unity/whats-new/unity-5.4.4) [(Archive link)](https://unity3d.com/get-unity/download/archive) (Unity is not implicitly forward compatible).
* Uses [Google VR SDK version 0.6 (also called Google Cardboard SDK)](https://github.com/googlevr/gvr-unity-sdk/releases/tag/v0.6).
* Supports minimum Android API level 16+ - though it was only available under [older Unity (tag v1.0)](https://github.com/scscgit/VirtuLearn/tree/v1.0), as it now implicitly enforces 19+ by overriding the manifest (a newer Cardboard SDK would also require higher version) - maybe something like IPostGenerateGradleAndroidProject could work with the new Unity if anyone's interested in trying. Other platforms weren't tested.
* Contains a custom Head-Tilt based movement input implementation.
* Includes a small mathematical game of sorting numbered boxed, interacting using gaze only.
* Implements three input modes (Non-VR keyboard, Non-VR touch phone, VR head-tilt) accessible from both the menu, and an in-game console.