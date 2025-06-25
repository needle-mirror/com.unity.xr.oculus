# Migrate to OpenXR

Understand how to migrate your project from the Oculus plugin to OpenXR Meta.

From [OpenXR 1.14](https://docs.unity3d.com/Packages/com.unity.xr.openxr@1.14), the [Unity OpenXR: Meta 2.1](https://docs.unity3d.com/Packages/com.unity.xr.meta-openxr@2.1) package is at feature parity with the Oculus plugin. This means that you can use OpenXR in your Meta Quest project without losing functionality.

Migrating to OpenXR has additional benefits including ongoing support, an active development roadmap, and cross-platform development.

The following sections describe how to migrate your project from the Oculus plugin to Unity OpenXR: Meta 2.1.

## Choose your OpenXR packages

For full feature parity, use the [Unity OpenXR: Meta](https://docs.unity3d.com/Packages/com.unity.xr.meta-openxr@2.1) package. The OpenXR Meta package isn't a requirement if you don’t need the additional features it provides. If you don't need these features, you can use the [OpenXR plugin](https://docs.unity3d.com/Packages/com.unity.xr.openxr@1.14) with [Meta Quest Support](https://docs.unity3d.com/Packages/com.unity.xr.meta-openxr@2.1/manual/features/features/meatquest.html).

To understand the features each package provides, refer to the OpenXR [Features](https://docs.unity3d.com/Packages/com.unity.xr.openxr@1.14/manual/features.html) and OpenXR Meta [Features](https://docs.unity3d.com/Packages/com.unity.xr.meta-openxr@2.1/manual/features/features.html) documentation respectively.

## OpenXR Meta requirements

To use the Unity OpenXR: Meta package, your project must meet the following requirements:

* Unity 6000.0.0f1 or newer
* [OpenXR 1.14](https://docs.unity3d.com/Packages/com.unity.xr.openxr@1.14) or newer
* [Unity OpenXR: Meta 2.1](https://docs.unity3d.com/Packages/com.unity.xr.meta-openxr@2.1) or newer

## Install packages

Unity OpenXR: Meta is an official Unity package available from the [Package Manager](https://docs.unity3d.com/6000.0/Documentation/Manual/upm-ui.html). To understand how to install a package via the Package Manager, refer to [Install a UPM package from a registry](https://docs.unity3d.com/6000.0/Documentation/Manual/upm-ui-install.html).

>[!NOTE]
> When you install the Unity OpenXR: Meta package from the Package Manager, Unity automatically installs the OpenXR Plug-in as a required dependency.

## Configure project settings

To use OpenXR Meta, you need to configure your project for OpenXR Meta features to function. This includes enabling OpenXR Meta features in [XR Plug-in Management](https://docs.unity3d.com/Packages/com.unity.xr.management@4.5/manual/index.html).

To understand how to set up your project for OpenXR Meta, refer to [Configure project settings](https://docs.unity3d.com/Packages/com.unity.xr.meta-openxr@2.1/manual/get-started/project-settings.html) in the OpenXR Meta documentation.

You can also [Set up your scene](https://docs.unity3d.com/Packages/com.unity.xr.meta-openxr@2.1/manual/get-started/scene-setup.html) and [Optimize graphics settings](https://docs.unity3d.com/Packages/com.unity.xr.meta-openxr@2.1/manual/get-started/graphics-settings.html) to adjust your project for OpenXR Meta.

### Configure interaction profiles

The recommended interaction profile for OpenXR Meta is the [Oculus Touch controller Profile](https://docs.unity3d.com/Packages/com.unity.xr.openxr@1.14/manual/features/oculustouchcontrollerprofile.html). You can add this in the provider settings as outlined in [Configure project settings]([https://docs.unity3d.com/Packages/com.unity.xr.meta-openxr@2.1/manual/get-started/project-settings.html](https://docs.unity3d.com/Packages/com.unity.xr.meta-openxr@2.1/manual/get-started/project-settings.html)).

## Understand OpenXR features

The following table outlines how specific Oculus features correspond to OpenXR Meta features:

| **Oculus feature**               | **OpenXR Meta feature**                  |
| :------------------------------- | :--------------------------------------- |
| Software Dynamic Resolution      | [Dynamic resolution](https://developers.meta.com/horizon/documentation/unity/dynamic-resolution-unity/) (Meta developer documentation) |
| Occlusion                        | [Occlusion](https://docs.unity3d.com/Packages/com.unity.xr.meta-openxr@2.1/manual/features/occlusion.html)  |
| Meta Quest Touch Plus Controller | [Meta Quest Touch Plus Controller Profile](https://docs.unity3d.com/Packages/com.unity.xr.openxr@1.14/manual/features/metaquesttouchpluscontrollerprofile.html) |
| Quest Link support               | [Meta Quest Link](https://docs.unity3d.com/Packages/com.unity.xr.meta-openxr@2.1/manual/get-started/link.html) |

Refer to the documentation for each feature to learn about specific requirements.

## Additional resources

* [OpenXR Parity with Oculus XR Plugin](https://discussions.unity.com/t/openxr-parity-with-oculus-xr-plugin/1617008) (Unity discussions)
* [Develop for Meta Quest workflow](xref:um-xr-meta-quest-develop) (Unity Manual)
