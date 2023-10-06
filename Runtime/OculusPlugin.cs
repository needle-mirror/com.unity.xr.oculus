#if !(UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || (UNITY_ANDROID && !UNITY_EDITOR))
#define OCULUSPLUGIN_UNSUPPORTED_PLATFORM
#endif

#if (UNITY_ANDROID && !UNITY_EDITOR)
#define OCULUSPLUGIN_ANDROID_PLATFORM_ONLY
#endif

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Unity.XR.Oculus
{

    public enum SystemHeadset
    {
        None = 0,

        // Standalone headsets
        Oculus_Quest = 8,
        Oculus_Quest_2 = 9,
        Meta_Quest_Pro = 10,
        Placeholder_10 = 10,
        Meta_Quest_3 = 11,
        Placeholder_11 = 11,
        Placeholder_12,
        Placeholder_13,
        Placeholder_14,

        // PC headsets
        Rift_DK1 = 0x1000,
        Rift_DK2,
        Rift_CV1,
        Rift_CB,
        Rift_S,
        Oculus_Link_Quest,
        Oculus_Link_Quest_2,
        Meta_Link_Quest_Pro,
        PC_Placeholder_4103 = Meta_Link_Quest_Pro,
        Meta_Link_Quest_3,
        PC_Placeholder_4104 = Meta_Link_Quest_3,
        PC_Placeholder_4105,
        PC_Placeholder_4106,
        PC_Placeholder_4107
    }

    public static partial class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct UserDefinedSettings
        {
            public ushort sharedDepthBuffer;
            public ushort dashSupport;
            public ushort stereoRenderingMode;
            public ushort colorSpace;
            public ushort lowOverheadMode;
            public ushort optimizeBufferDiscards;
            public ushort phaseSync;
            public ushort symmetricProjection;
            public ushort subsampledLayout;
            public ushort lateLatching;
            public ushort lateLatchingDebug;
            public ushort enableTrackingOriginStageMode;
            public ushort spaceWarp;
            public ushort depthSubmission;
            public ushort foveatedRenderingMethod;
        }

		[StructLayout(LayoutKind.Sequential)]
		internal struct EnvironmentDepthFrameDescInternal
		{
			public bool isValid;
			public double createTime;
			public double predictedDisplayTime;
			public int swapchainIndex;
			public Vector3 createPoseLocation;
			public Vector4 createPoseRotation;
			public float fovLeftAngle;
			public float fovRightAngle;
			public float fovTopAngle;
			public float fovDownAngle;
			public float nearZ;
			public float farZ;
			public float minDepth;
			public float maxDepth;
		}

        [StructLayout(LayoutKind.Sequential)]
        internal struct EnvironmentDepthCreateParamsInternal
        {
            public bool removeHands;
        }

        internal static void SetColorScale(float x, float y, float z, float w)
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            Internal.SetColorScale(x, y, z, w);
#endif
        }

        internal static void SetColorOffset(float x, float y, float z, float w)
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            Internal.SetColorOffset(x, y, z, w);
#endif
        }

        internal static bool GetIsSupportedDevice()
        {
#if OCULUSPLUGIN_ANDROID_PLATFORM_ONLY
            return Internal.GetIsSupportedDevice();
#else
            return false;
#endif
        }

        internal static bool LoadOVRPlugin(string ovrpPath)
        {
#if OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            return false;
#else
            return Internal.LoadOVRPlugin(ovrpPath);
#endif
        }

        internal static void UnloadOVRPlugin()
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            Internal.UnloadOVRPlugin();
#endif
        }

        internal static void SetUserDefinedSettings(UserDefinedSettings settings)
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            Internal.SetUserDefinedSettings(settings);
#endif
        }

        internal static int SetCPULevel(int cpuLevel)
        {
#if OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            return -1;
#else
            return Internal.SetCPULevel(cpuLevel);
#endif
        }

        internal static int SetGPULevel(int gpuLevel)
        {
#if OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            return -1;
#else
            return Internal.SetGPULevel(gpuLevel);
#endif
        }

        internal static void GetOVRPVersion(byte[] version)
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            Internal.GetOVRPVersion(version);
#endif
        }

        internal static void EnablePerfMetrics(bool enable)
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            Internal.EnablePerfMetrics(enable);
#endif
        }

        internal static void EnableAppMetrics(bool enable)
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            Internal.EnableAppMetrics(enable);
#endif
        }

        internal static bool SetDeveloperModeStrict(bool active)
        {
#if OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            return false;
#else
            return Internal.SetDeveloperModeStrict(active);
#endif
        }

        internal static bool GetHasInputFocus()
        {
#if OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            return false;
#else
            return Internal.GetAppHasInputFocus();
#endif
        }

        internal static bool GetBoundaryConfigured()
        {
#if OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            return false;
#else
            return Internal.GetBoundaryConfigured();
#endif
        }

        internal static bool GetBoundaryDimensions(Boundary.BoundaryType boundaryType, out Vector3 dimensions)
        {
#if OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            dimensions = Vector3.zero;
            return false;
#else
            return Internal.GetBoundaryDimensions(boundaryType, out dimensions);
#endif
        }

        internal static bool GetBoundaryVisible()
        {
#if OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            return false;
#else
            return Internal.GetBoundaryVisible();
#endif
        }

        internal static void SetBoundaryVisible(bool boundaryVisible)
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            Internal.SetBoundaryVisible(boundaryVisible);
#endif
        }

        internal static bool GetAppShouldQuit()
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            return Internal.GetAppShouldQuit();
#else
            return false;
#endif
        }

        internal static bool GetDisplayAvailableFrequencies(IntPtr ptr, ref int numFrequencies)
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            return Internal.GetDisplayAvailableFrequencies(ptr, ref numFrequencies);
#else
            return false;
#endif
        }

        internal static bool SetDisplayFrequency(float refreshRate)
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            return Internal.SetDisplayFrequency(refreshRate);
#else
            return false;
#endif
        }

        internal static bool GetDisplayFrequency(out float refreshRate)
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            return Internal.GetDisplayFrequency(out refreshRate);
#else
            refreshRate = 0.0f;
            return false;
#endif
        }

        internal static SystemHeadset GetSystemHeadsetType()
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            return Internal.GetSystemHeadsetType();
#else
            return SystemHeadset.None;
#endif
        }

        internal static bool GetTiledMultiResSupported()
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            return Internal.GetTiledMultiResSupported();
#else
            return false;
#endif
        }

        internal static void SetTiledMultiResLevel(int level)
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            Internal.SetTiledMultiResLevel(level);
#endif
        }

        internal static int GetTiledMultiResLevel()
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            return Internal.GetTiledMultiResLevel();
#else
            return -1;
#endif
        }

        internal static void SetTiledMultiResDynamic(bool isDynamic)
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            Internal.SetTiledMultiResDynamic(isDynamic);
#endif
        }

        internal static bool GetEyeTrackedFoveatedRenderingSupported()
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            return Internal.GetEyeTrackedFoveatedRenderingSupported();
#else
            return false;
#endif
        }

        internal static bool GetEyeTrackedFoveatedRenderingEnabled()
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            return Internal.GetEyeTrackedFoveatedRenderingEnabled();
#else
            return false;
#endif
        }

        internal static void SetEyeTrackedFoveatedRenderingEnabled(bool isEnabled)
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            Internal.SetEyeTrackedFoveatedRenderingEnabled(isEnabled);
#endif
        }

        internal static bool GetShouldRestartSession()
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            return Internal.GetShouldRestartSession();
#else
            return false;
#endif
        }

        internal static void SetupEnvironmentDepth(Utils.EnvironmentDepthCreateParams createParams)
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            EnvironmentDepthCreateParamsInternal param = new EnvironmentDepthCreateParamsInternal();
            param.removeHands = createParams.removeHands;

            Internal.SetupEnvironmentDepth(ref param);
#endif
        }

        internal static void SetEnvironmentDepthRendering(bool isEnabled)
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            Internal.SetEnvironmentDepthRendering(isEnabled);
#endif
        }

        internal static void ShutdownEnvironmentDepth()
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            Internal.ShutdownEnvironmentDepth();
#endif
        }

        internal static bool GetEnvironmentDepthTextureId(ref uint id)
        {
#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            return Internal.GetEnvironmentDepthTextureId(ref id);
#else
            return false;
#endif
        }

        internal static Utils.EnvironmentDepthFrameDesc GetEnvironmentDepthFrameDesc(int eye)
        {
            Utils.EnvironmentDepthFrameDesc desc = new Utils.EnvironmentDepthFrameDesc();
            desc.isValid = false;

#if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            EnvironmentDepthFrameDescInternal frameDesc = new EnvironmentDepthFrameDescInternal();
            if (Internal.GetEnvironmentDepthFrameDesc(ref frameDesc, eye))
            {
                desc.isValid = frameDesc.isValid;
                desc.createTime = frameDesc.createTime;
                desc.predictedDisplayTime = frameDesc.predictedDisplayTime;
                desc.swapchainIndex = frameDesc.swapchainIndex;
                desc.createPoseLocation = frameDesc.createPoseLocation;
                desc.createPoseRotation = frameDesc.createPoseRotation;
                desc.fovLeftAngle = frameDesc.fovLeftAngle;
                desc.fovRightAngle = frameDesc.fovRightAngle;
                desc.fovTopAngle = frameDesc.fovTopAngle;
                desc.fovDownAngle = frameDesc.fovDownAngle;
                desc.nearZ = frameDesc.nearZ;
                desc.farZ = frameDesc.farZ;
                desc.minDepth = frameDesc.minDepth;
                desc.maxDepth = frameDesc.maxDepth;
            }
#endif
            return desc;
        }

        private static class Internal
        {
            [DllImport("OculusXRPlugin")]
            internal static extern void SetColorScale(float x, float y, float z, float w);

            [DllImport("OculusXRPlugin")]
            internal static extern void SetColorOffset(float x, float y, float z, float w);

            [DllImport("OculusXRPlugin")]
            internal static extern bool GetIsSupportedDevice();

            [DllImport("OculusXRPlugin", CharSet = CharSet.Unicode)]
            internal static extern bool LoadOVRPlugin(string ovrpPath);

            [DllImport("OculusXRPlugin")]
            internal static extern void UnloadOVRPlugin();

            [DllImport("OculusXRPlugin")]
            internal static extern void SetUserDefinedSettings(UserDefinedSettings settings);

            [DllImport("OculusXRPlugin")]
            internal static extern int SetCPULevel(int cpuLevel);

            [DllImport("OculusXRPlugin")]
            internal static extern int SetGPULevel(int gpuLevel);

            [DllImport("OculusXRPlugin", CharSet = CharSet.Auto)]
            internal static extern void GetOVRPVersion(byte[] version);

            [DllImport("OculusXRPlugin")]
            internal static extern void EnablePerfMetrics(bool enable);

            [DllImport("OculusXRPlugin")]
            internal static extern void EnableAppMetrics(bool enable);

            [DllImport("OculusXRPlugin")]
            internal static extern bool SetDeveloperModeStrict(bool active);

            [DllImport("OculusXRPlugin")]
            internal static extern bool GetAppHasInputFocus();

            [DllImport("OculusXRPlugin")]
            internal static extern bool GetBoundaryConfigured();

            [DllImport("OculusXRPlugin")]
            internal static extern bool GetBoundaryDimensions(Boundary.BoundaryType boundaryType, out Vector3 dimensions);

            [DllImport("OculusXRPlugin")]
            internal static extern bool GetBoundaryVisible();

            [DllImport("OculusXRPlugin")]
            internal static extern void SetBoundaryVisible(bool boundaryVisible);

            [DllImport("OculusXRPlugin")]
            internal static extern bool GetAppShouldQuit();

            [DllImport("OculusXRPlugin")]
            internal static extern bool GetDisplayAvailableFrequencies(IntPtr ptr, ref int numFrequencies);

            [DllImport("OculusXRPlugin")]
            internal static extern bool SetDisplayFrequency(float refreshRate);

            [DllImport("OculusXRPlugin")]
            internal static extern bool GetDisplayFrequency(out float refreshRate);

            [DllImport("OculusXRPlugin")]
            internal static extern SystemHeadset GetSystemHeadsetType();

            [DllImport("OculusXRPlugin")]
            internal static extern bool GetTiledMultiResSupported();

            [DllImport("OculusXRPlugin")]
            internal static extern void SetTiledMultiResLevel(int level);

            [DllImport("OculusXRPlugin")]
            internal static extern int GetTiledMultiResLevel();

            [DllImport("OculusXRPlugin")]
            internal static extern void SetTiledMultiResDynamic(bool isDynamic);

            [DllImport("OculusXRPlugin")]
            internal static extern bool GetEyeTrackedFoveatedRenderingSupported();

            [DllImport("OculusXRPlugin")]
            internal static extern bool GetEyeTrackedFoveatedRenderingEnabled();

            [DllImport("OculusXRPlugin")]
            internal static extern void SetEyeTrackedFoveatedRenderingEnabled(bool isEnabled);

            [DllImport("OculusXRPlugin")]
            internal static extern bool GetShouldRestartSession();

            [DllImport("OculusXRPlugin")]
            internal static extern bool SetupEnvironmentDepth(ref EnvironmentDepthCreateParamsInternal createParams);

            [DllImport("OculusXRPlugin")]
            internal static extern bool SetEnvironmentDepthRendering(bool isEnabled);

            [DllImport("OculusXRPlugin")]
            internal static extern bool ShutdownEnvironmentDepth();

            [DllImport("OculusXRPlugin")]
            internal static extern bool GetEnvironmentDepthTextureId(ref uint id);

            [DllImport("OculusXRPlugin")]
            internal static extern bool GetEnvironmentDepthFrameDesc(ref EnvironmentDepthFrameDescInternal frameDesc, int eye);
        }
    }
}
