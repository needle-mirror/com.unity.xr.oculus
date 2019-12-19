# About the Oculus XR Plugin :

The Oculus XR Plugin enables you to build applications for a variety of Oculus devices including the Rift, Rift S, Quest and GO.

# Supported XR Plugin Subsystems:

## Display 

The display subsystem provides stereo rendering support for the XR Plugin.

### Graphics API Support
*Windows (Rift, Rift S)* 
- DX11

*Android (Quest, GO)*
- GLES3.0
- Vulkan

## Input 

The input subsystem provides controller support, haptics, and input tracking for the HMD.

# XR Management Support
While not required to use the _Oculus_ XR Plugin, integration with XR Management provides for a simpler and easier way of using this (and other) providers within Unity. This package provides the following XR Management support: 
- *Runtime Settings* - Easily configure runtime settings such as rendering modes, depth buffer sharing, dash support, and others.
- *Lifecycle Management* - This package ships with a default XR Plugin loader implementation that handles subsystem lifecycle such as application initialization, shutdown, pausing and resuming.

## Windows Standalone Settings (Rift, Rift S)

- *Shared Depth Buffer* - Enable or disable support for using a shared depth buffer. This allows Unity and Oculus to use a common depth buffer which enables Oculus to composite the Oculus Dash and other utilities over the Unity application. 

- *Dash Support* - Enable or disable dash support. This inintializes the Oculus Plugin with dash support which enables the Oculus Dash to composite over the Unity application.

- *Stereo Rendering Mode*:
	- *Multi Pass* - Unity makes two passes across the scene graph, each one entirely indepedent of the other.  Each pass has its own eye matrices, and render target. Unity will draw everything twice, which includes setting the graphics state for each pass. This is a slow and simple rendering method which doesn't require any special modification to shaders. 

	- *Single Pass Instanced* - Unity uses a single texture array with two elements. Unity converts each call into an instanced draw call. Shaders need to be aware of this. Unity's shader macros handle the situation. 

## Android (Quest, Go)

- *V2Signing* - Enable this if you are building for Quest. This enables application signing with the Android Package (APK) Signature Scheme v2. Disable v2 signing if building for Oculus Go.

- *Stereo Rendering Mode*:
	- *Multi Pass* - Unity makes two passes across the scene graph, each one entirely indepedent of the other.  Each pass has its own eye matrices, and render target. Unity will draw everything twice, which includes setting the graphics state for each pass. This is a slow and simple rendering method which doesn't require any special modification to shaders. 

	- *Multiview* - Multiview is essentially the same as Single Pass Instanced which is described above. The only difference is that the _driver_ does the draw call conversion so it requires less work on Unity's side. As with Single Pass Instanced, shaders need to be aware of the Multiview setting.  

# Technical details
## Requirements
This version of Oculus XR Plugin is compatible with the following versions of the Unity Editor:
 - 2019.3.0f2 
 - 2020.1.0b1
