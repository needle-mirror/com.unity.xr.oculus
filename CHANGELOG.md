# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.1.0-preview] - 2019-10-17
### Fixes
- semver to reflect new backwards compatible functionality (color scale API, input subsystem layouts)

## [1.0.3-preview.1] - 2019-10-15
### Fixes
- thread safe color scale
- screenshot artifacts with SPI

## [1.0.3-preview] - 2019-09-27
### Fixes
- Fixed msaa issues on quest
- Fixed side-by-side screenshot functionality
### Changes
- Disables main framebuffer flag to save memory (~36MB on Quest)
- Input subsystem layouts to package
### Adds
- Color scale and offset api and helper class
- More oculus statistics (accessible via display subsystem api)
- User presence usage when using new input system 

## [1.0.2] - 2019-09-03
### Changes
- XR Plugin Management dependency

## [1.0.1] - 2019-08-28
### Fixes
- Input bugs
  - GO reported sceondary button when it should have reported a menu button
  - Quest and Rift S reported a thumbrest when they did not have one
  - Oculus Remote would never connect
- Timing issues upon pausing/resuming app on standalone HMDs
- V2signing checkbox for properly signed apks on quest

## [1.0.0] - 2019-07-10
### Changes
- Removed preview tag
- Update com.unity.xr.management dependency version
- Migrate away from experience subsystem
- Update Boundary Points when recentering and changing the tracking space origin mode
- Fixed spatializer .meta files

## [0.9.0-preview] - 2019-06-17
### Added
- Oculus 1.37 runtime upgrade
- Oculus audio spatializer plugin

## [0.8.4-preview] - 2019-06-10
### Added
- Single Pass Instancing support on PC DX11

## [0.8.0-preview] - 2019-06-06
### Added
- Rendering and input support
- Arm64 support for mobile builds
- Depth support
- Render viewport scale
- Eye texture resolution scale
- Culling pose pullback
- Win32 compatibility
- Updates minimum unity version to 2019.2
- Input tracking reference node reporting
- Updates to oculus plugin 1.34
- XRStats support
- Device relative eye positions
- Recenter functionality
- Registration of tracking references
- Tests to package
- Haptics Functionality
