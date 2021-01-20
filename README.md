BoltKlak
========

**BoltKlak** is an extension for Unity's Bolt visual scripting system that
provides miscellaneous math functions useful for creating procedural behaviors.

At the moment, it provides the following units:

- **XXHash** (deterministic random number generator)
- **ExpTween**/**CdsTween** (generic interpolator)
- **Noise Source** (fractional Brownian motion)
- **On Keyboard Input** (keyboard input with the new Input System)

XXHash
------

![gif](https://i.imgur.com/evtniEQ.gif)

![XXHash](https://i.imgur.com/B1RNUow.png)

**[XXHash]** is a lightweight non-cryptographic hash function with which you
can generate pseudo-random number sequences in a deterministic manner.

[XXHash]: https://github.com/Cyan4973/xxHash

The XXHash unit generates a random number based on the two inputs -- *Seed* and
*Data*. In our use case, *Data* simply works as a secondary random seed.

There are variants for different output types:

- **XXHashInt**
- **XXHashFloat**
- **XXHashVector3**
- **XXHashDirection** (uniformly distributed random points on a unit sphere)
- **XXHashRotation** (quaternion representing a random rotation)

ExpTween
--------

![gif](https://i.imgur.com/mY2y641.gif)

![ExpTween](https://i.imgur.com/CGquPm7.png)

The **ExpTween** unit implements an exponential interpolation function that is
useful for creating ease-out animation.

A unique thing about ExpTween is that it doesn't require a control-flow
connection. It's a stateless function, so that it works only with value
connections.

There are variants for different types: **ExpTweenFloat**,
**ExpTweenVector3**, and **ExpTweenQuaternion**.

CdsTween
--------

![gif](https://i.imgur.com/tFgUGrs.gif)

![CdsTween](https://i.imgur.com/JEvzQHC.png)

The **CdsTween** unit implements a [critically damped spring smoothing function]
that is useful for creating smooth ease-in/out animation.

[critically damped spring smoothing function]:
  http://mathproofs.blogspot.com/2013/07/critically-damped-spring-smoothing.html

There are variants for different types: **CdsTweenFloat**,
**CdsTweenVector3**, and **CdsTweenQuaternion**.

NoiseSource
-----------

![gif](https://i.imgur.com/ogxO8vQ.gif)

![NoiseSource](https://i.imgur.com/tp6i0hS.png)

The **NoiseSource** unit implements a fractional Brownian motion (fBm) using a
simplex noise function. It's useful to create natural wavy motion or
undulation/vibration.

There are variants for different output types: **NoiseSourceFloat**,
**NoiseSourceVector3**, **NoiseSourceQuaternion**.

OnKeyboardInput
---------------

![OnKeyboardInput](https://i.imgur.com/om5JWHi.png)

The **OnKeyboardInput** unit is a simple replacement of the standard
OnKeyboardInput unit but with [the new Input System].

[the new Input System]:
  https://docs.unity3d.com/Packages/com.unity.inputsystem@latest

How to try the sample project
-----------------------------

This repository doesn't contain the Bolt assets due to the license restriction.
You have to import [Bolt via Asset Store] manually.

[Bolt via Asset Store]:
  https://assetstore.unity.com/packages/tools/visual-scripting/bolt-163802

You can't use the "Install Bolt" tool due to compilation errors caused by
missing file references. You have to manually double-click the
`Bolt_1_4_X_NET4.unitypackage` file in the "Install Bolt" directory instead.

![unitypackage](https://i.imgur.com/cNxH458.png)

After importing the unitypackage file, it automatically opens the Bolt Setup
Wizard.

![wizard](https://i.imgur.com/wxlvRh7.png)

On the Assembly Options page, add `Bolt.Addons.Klak.Base.Runtime` to the
assembly list.

![assembly options](https://i.imgur.com/cVwqK2m.png)

How to install the add-on to an existing project
------------------------------------------------

### Installing the package via Package Manager

This package uses the [scoped registry] feature to resolve package
dependencies. Please add the following sections to the manifest file
(Packages/manifest.json).

[scoped registry]: https://docs.unity3d.com/Manual/upm-scoped.html

To the `scopedRegistries` section:

```
{
  "name": "Keijiro",
  "url": "https://registry.npmjs.com",
  "scopes": [ "jp.keijiro" ]
}
```

To the `dependencies` section:

```
"jp.keijiro.bolt.klak.base": "1.0.0"
```

After changes, the manifest file should look like below:

```
{
  "scopedRegistries": [
    {
      "name": "Keijiro",
      "url": "https://registry.npmjs.com",
      "scopes": [ "jp.keijiro" ]
    }
  ],
  "dependencies": {
    "jp.keijiro.bolt.klak.base": "1.0.0",
    ...
```

### Adding the add-on assembly

Navigate to "Tools" > "Bolt" > "Unit Options Wizard".

![assembly options](https://i.imgur.com/cVwqK2m.png)

Add `Bolt.Addons.Klak.Base.Runtime` to the assembly list. Then press "Next" and
"Generate."
