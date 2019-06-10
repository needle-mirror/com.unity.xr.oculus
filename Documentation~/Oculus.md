# About the Oculus XR Plugin :

The Oculus XR Plugin enables you to build applications for Oculus devices. 

# Installing the Oculus XR Plugin :
The XR Management package will now serve as the main entry point for downloading the right package for each target SDK/platform and managing respective settings. 

To install the Oculus XR Plugin, do the following:
1. Install the XR Management package from [Package Manager](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@latest/index.html)
2. Once installed, the XR Management package will take you to the project settings window.
3. Click on the XR tab
	- *Note*: The XR tab will not exist in Project Settings unless the XR Management package has been installed.
4. In the XR tab window, click on the plus button to add a Plugin Loader
5. Go to Download -> Oculus Loader

The Oculus XR Plugin will automatically download via the Package Manager. 

Note that the XR settings tab now has a dropdown for "Oculus". Navigate to the XR -> Oculus settings window in Project Settings to create an Oculus XR Plugin specific settings asset. This asset is editable from the Oculus settings window and can toggle settings such as Dash Support, Shared Depth buffer support, and the Rendering Mode.

# Technical details
## Requirements
This version of Oculus XR Plugin is compatible with the following versions of the Unity Editor:
 - 2019.2
 - 2019.3

## Release Notes

## Known Issues
* Oculus Integration utilities are not currently fully supported when using the Oculus XR Plugin
* [Quest][Rift S] controllers report a "Thumbrest" usage, which they do not have.
* Oculus XR Plugin is failing some publishing validation tests
	* VRC validator test
	* TestSubmitFramesWhenNotVisible
	* TestResponseToRecenterRequest (fails with Integrated Packages as well)
	* TestAppShouldQuit
* Camera is in different position when entering playmode compared to Legacy XR when using the TPD
	* It is possible to work around this by:
 	* Create empty game object
	* Set it at 0, 1, -10 (the default camera location)
	* Make the camera a child of this game object, set camera transform to 0, 0, 0.
	* Enter playmode and camera is now in the expected location
* Changing the render viewport scale and opening the oculus dash while depth and dash support are enabled causes unexpected rendering artifacts
* Render viewport scale is broken when HDR is off (this affects Integrated XR as well)
* XRDevice API values are not populated (Documentation)
* Rift S is not fully supported
* View is not landscape locked when deploying to GearVR in developer mode
* Known issues with Single Pass Insancing on PC:
    * Shadows may have issues when using the LWRP (Lightweight Render Pipeline)
    * Depth Sharing doesn't currently work with Single Pass Instancing

## Document revision history
|Date|Reason|
|---|---|
|June 10, 2019|First official preview version of package.|
