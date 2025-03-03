# About the Oculus XR Plugin

The Oculus XR Plugin enables you to build applications for a variety of Oculus devices including the Rift, Rift S, Quest 2, Quest Pro, Quest 3, and Quest 3S.

## Supported XR plugin subsystems

### Display

The display subsystem provides stereo rendering support for the XR Plugin. It supports the following graphics APIs:

* Windows
    * Direct3D 11
* Android
    * OpenGL ES 3.0
    * Vulkan

### Input

The input subsystem provides controller support, haptics, and tracking for the controllers and head-mounted display (HMD).

## XR Management support

Integration with XR Management isn't required to use the Oculus XR Plugin, but it provides for a simpler and easier way of using this and other Providers within Unity. The Oculus XR Plugin package ships with built-in XR Management support. For more information, refer to [XR Management Documention](https://docs.unity3d.com/Packages/com.unity.xr.management@latest)

The Oculus XR Plugin integration with XR Management provides the following functionality:

* **Runtime Settings** - Configure runtime settings such as rendering modes, depth buffer sharing, Dash support, etc.
* **Lifecycle Management** - The Oculus XR Plugin ships with a default XR Plugin loader implementation that handles subsystem lifecycle such as application initialization, shutdown, pausing, and resuming.

### Windows standalone settings

* **Stereo Rendering Mode** - You can select *Multi Pass* or *Single Pass Instanced* stereo rendering mode.
    * *Multi Pass* - Unity renders each eye independently by making two passes across the scene graph. Each pass has its own eye matrices and render target. Unity draws everything twice, which includes setting the graphics state for each pass. This is a slow and simple rendering method which doesn't require any special modification to shaders.
    * *Single Pass Instanced* - Unity uses a texture array with two slices, and uses instanced draw calls (converting non-instanced draw calls to instanced versions when necessary) to direct rendering to the appropriate texture slice. Modify custom shaders for rendering in this mode. Use Unity's [XR shader macros](xref:SinglePassInstancing) to simplify authoring custom shaders.
* **Shared Depth Buffer** - Enable or disable support for using a shared depth buffer. This allows Unity and Oculus to use a common depth buffer, which enables Oculus to composite the Oculus Dash and other utilities over the Unity application.
* **Dash Support** - Enable or disable Dash support. This initializes the Oculus Plugin with Dash support, which enables the Oculus Dash to composite over the Unity application.

### Android settings

* **Stereo Rendering Mode** - You can select *Multi Pass* or *Multiview* stereo rendering mode.
    * *Multi Pass* - Unity renders each eye independently by making two passes across the scene graph. Each pass has its own eye matrices and render target. Unity draws everything twice, which includes setting the graphics state for each pass. This is a slow and simple rendering method which doesn't require any special modification to shaders.
    * *Multiview* - Multiview is essentially the same as the *Single Pass Instanced* option described above, except the graphics driver does the draw call conversion, requiring less work from the Unity engine. As with *Single Pass Instanced*, you need to author shaders to enable Multiview. Using Unity's [XR shader macros](xref:SinglePassInstancing) will simplify custom shader development.
* **Low Overhead Mode** - If enabled, the GLES graphics driver will bypass validation code, potentially running faster. Disable this if you experience graphics instabilities. GLES only.
* **Optimize Buffer Discards** - If enabled, the depth buffer contents will be discarded rather than resolved and the MSAA color buffer will be resolved rather than stored after rendering. This is a performance optimization that can possibly break rendering effects that sample from the depth buffer, such as camera stacking. Vulkan only.
* **Symmetric Projection** - If enabled, this allows the application to render with symmetric projection matrices. This can improve GPU performance when using multiview due to more common workloads between the left and right eye. Supported when using Vulkan and Multiview.
* **Subsampled Layout** - If enabled, the eye textures will use a subsampled layout. When using Fixed Foveated Rendering (FFR), the subsampled layout will improve app GPU performance and reduce FFR-related visual artifacts. However, this feature will slightly increase the GPU cost of Timewarp. Therefore, enable it only if the app is using FFR level 2 or higher, in which case, the app GPU performance improvement should outweigh the extra Timewarp cost. Vulkan only.
* **Foveated Rendering Method** - Choose which foveated rendering method is used when foveation is enabled.
    * *Fixed Foveated Rendering* - Foveates the image based on a fixed pattern.
    * *Eye Tracked Foveated Rendering* - Foveates the image using eye tracking. Only supported on Quest Pro with proper permissions and when using Vulkan, Multiview, and ARM64.
    * *Fixed Foveated Rendering Using Unity API For URP* - Foveates the image based on a fixed pattern. This option will use Unity's common Foveated Rendering API that allows improved support for URP. This option does not work with the Built-In Render Pipeline.
    * *Eye Tracked Foveated Rendering Using Unity API For URP* - Foveates the image using eye tracking. Only supported on Quest Pro with proper permissions and when using Vulkan, Multiview, and ARM64. This option will use Unity's common Foveated Rendering API that allows improved support for URP. This option does not work with the Built-In Renderer Pipeline.
* **Enable TrackingOrigin Stage Mode** - If you enable **TrackingOrigin Stage Mode** when the `TrackingOriginMode` of your `XR Origin` component is set to `Floor`, the tracking origin won’t change with a system recenter.
* **Depth Submission** - Enables support for submitting the depth buffer on mobile. This enables depth testing between layers on Oculus mobile platforms.
* **System Splash Screen** - You can add a PNG file under the Assets folder as the system splash screen image. If set, the OS will display the system splash screen as a high quality compositor layer as soon as the app is starting to launch until the app submits the first frame.
* **Late Latching** - This feature reduces tracked rendering latency by updating head and controller poses as late as possible before rendering. Vulkan only.
* **Late Latching Debug Mode** - Enables a debug mode for Late Latching which will print information about the Late Latching system as well as any errors. This can be used to verify that Late Latching is performing correctly. Debug mode is only active in Development builds.
* **Application SpaceWarp** - Enables support for a frame synthesis technology to allow your application to render at half frame rate, while still delivering a smooth experience. Note that this currently requires a custom version of the URP provided by Oculus in order to work, and should not be enabled if you aren't using that customized Oculus URP package.
* **Optimize Multiview Render Regions (Vulkan)** - If enabled, Multiview Render Regions will prevent graphics processing outside of the user’s view. Refer to [Multiview Render Regions](https://docs.unity3d.com/6000.1/Documentation/Manual/xr-multiview-render-regions.html) to learn more about this feature.

## Technical details

### Environment Depth

For Quest 3 and Quest 3S, Environment Depth can be enabled to allow the sampling of real world depth data. This can be used to have real world objects occlude virtual objects in the application. `GetEnvironmentDepthSupported` can be used to check if the device supports Environment Depth. The Environment Depth rendering system can be started by calling `SetupEnvironmentDepth`. To check if the device or platform supports cleaner hand masking, use `GetEnvironmentDepthHandRemovalSupported`. Hand masking can be turned on by setting `removeHands` in `EnvironmentDepthCreateParams` to true and passing it into `SetupEnvironmentDepth`. Hand masking can also be toggled on and off using `SetEnvironmentDepthHandRemoval`. The depth texture can then be accessed by getting the depth texture ID using `GetEnvironmentDepthTextureId` and passing the ID to `GetRenderTexture` on the XRDisplaySubsystem. The rendering can then be enabled/disabled with `SetEnvironmentDepthRendering` and to completely free resources `ShutdownEnvironmentDepth` must be called. Extra data about the depth frame can be accessed through `GetEnvironmentDepthFrameDesc`.

### Fixed-Foveated Rendering (FFR)

Support for [fixed-foveated rendering](https://developer.oculus.com/documentation/quest/latest/concepts/mobile-ffr/) to provide better performance for [pixel-fill limited](https://en.wikipedia.org/wiki/Fillrate) applications. Controlling the level of foveation is made available through APIs in the Oculus XR Plugin.

When not using Unity's common Foveated Rendering API for URP, FFR only works when rendering directly into the *eye textures* using the [foward rendering mode](https://docs.unity3d.com/Manual/RenderTech-ForwardRendering.html).  [*Deferred rendering* mode](https://docs.unity3d.com/Manual/RenderTech-DeferredShading.html), which is characterized by rendering into an intermediate render texture, is not recommended for use with FFR. This situation arises often when using the default *Universal Rendering Pipeline*, which includes a blit operation by default at the end of the frame.

### Unity's common Foveated Rendering API for URP

This API uses the same underlying technology as FFR and ETFR, but improves the support in URP to allow the use of foveated rendering on passes that the previous API was not able to support. This allows the use of post-processing and other options that require the use of an intermediate render target, while still allowing foveated rendering to be applied.

To use foveation, the level needs to be set in a script since the default value is 0 (no foveation applied).

```c#
List<XRDisplaySubsystem> xrDisplays = new List<XRDisplaySubsystem>();
SubsystemManager.GetSubsystems(xrDisplays);
if (xrDisplays.Count == 1)
{
    // Set the level of foveation needed, 0 is disabled, 1 is the maximum foveation
    //
    // In the case of Meta Quest devices, only 4 levels are available:
    // [0] = disabled
    // (0..0.33) : Low
    // [0.33, 0.66) : Medium
    // [0.66, 1.0]: High
    xrDisplays[0].foveatedRenderingLevel = 1.0f;

    // Set this flag to request Gaze Tracking (ETFR), this will only affect Quest Pro
    // and will be ignored on devices that do not have hardware to track eye movement
    // or if the XR plugin settings don't allow eye tracking (ETFR)
    xrDisplays[0].foveatedRenderingFlags = XRDisplaySubsystem.FoveatedRenderingFlags.GazeAllowed;
}
```
