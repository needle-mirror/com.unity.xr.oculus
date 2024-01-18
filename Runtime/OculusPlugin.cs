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
    
    // some platform support can only be determined at runtime such as Windows Arm64
    internal static class RuntimePlatformChecks
    {
        private static readonly bool isRuntimeUnsupportedPlatform;
        
        static RuntimePlatformChecks()
        {
            // catch situations not handled by OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            isRuntimeUnsupportedPlatform = false;

#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
            if (RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
            {
                isRuntimeUnsupportedPlatform = true;
            }
#endif
        }

        internal static bool IsSupportedPlatform()
        {
#if OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            return false;
#else
            return !isRuntimeUnsupportedPlatform;
#endif
        }
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
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                Internal.SetColorScale(x, y, z, w);
            }
        }

        internal static void SetColorOffset(float x, float y, float z, float w)
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                Internal.SetColorOffset(x, y, z, w);
            }
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
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                return Internal.LoadOVRPlugin(ovrpPath);
            }
            else
            {
                return false;
            }
        }

        internal static void UnloadOVRPlugin()
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                Internal.UnloadOVRPlugin();
            }
        }

        internal static void SetUserDefinedSettings(UserDefinedSettings settings)
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                Internal.SetUserDefinedSettings(settings);
            }
        }

        internal static int SetCPULevel(int cpuLevel)
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                 return Internal.SetCPULevel(cpuLevel);
            }
            else
            {
                return -1;
            }
        }

        internal static int SetGPULevel(int gpuLevel)
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                return Internal.SetGPULevel(gpuLevel);
            }
            else
            {
                return -1;
            }
        }

        internal static void GetOVRPVersion(byte[] version)
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                Internal.GetOVRPVersion(version);
            }
        }

        internal static void EnablePerfMetrics(bool enable)
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                Internal.EnablePerfMetrics(enable);
            }
        }

        internal static void EnableAppMetrics(bool enable)
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                Internal.EnableAppMetrics(enable);
            }
        }

        internal static bool SetDeveloperModeStrict(bool active)
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                return Internal.SetDeveloperModeStrict(active);
            }
            else
            {
                return false;
            }
        }

        internal static bool GetHasInputFocus()
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                return Internal.GetAppHasInputFocus();
            }
            else
            {
                return false;
            }
        }

        internal static bool GetBoundaryConfigured()
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                return Internal.GetBoundaryConfigured();
            }
            else
            {
                return false;
            }
        }

        internal static bool GetBoundaryDimensions(Boundary.BoundaryType boundaryType, out Vector3 dimensions)
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                return Internal.GetBoundaryDimensions(boundaryType, out dimensions);
            }
            else
            {
                dimensions = Vector3.zero;
                return false;
            }
        }

        internal static bool GetBoundaryVisible()
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                return Internal.GetBoundaryVisible();
            }
            else
            {
                return false;
            }
        }

        internal static void SetBoundaryVisible(bool boundaryVisible)
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                Internal.SetBoundaryVisible(boundaryVisible);
            }
        }

        internal static bool GetAppShouldQuit()
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                return Internal.GetAppShouldQuit();
            }
            else
            {
                return false;
            }
        }

        internal static bool GetDisplayAvailableFrequencies(IntPtr ptr, ref int numFrequencies)
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                return Internal.GetDisplayAvailableFrequencies(ptr, ref numFrequencies);
            }
            else
            {
                return false;
            }
        }

        internal static bool SetDisplayFrequency(float refreshRate)
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                return Internal.SetDisplayFrequency(refreshRate);
            }
            else
            {
                return false;
            }
        }

        internal static bool GetDisplayFrequency(out float refreshRate)
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                return Internal.GetDisplayFrequency(out refreshRate);
            }
            else
            {
                refreshRate = 0.0f;
                return false;
            }
        }

        internal static SystemHeadset GetSystemHeadsetType()
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                return Internal.GetSystemHeadsetType();
            }
            else
            {
                return SystemHeadset.None;
            }
        }

        internal static bool GetTiledMultiResSupported()
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                return Internal.GetTiledMultiResSupported();
            }
            else
            {
                return false;
            }
        }

        internal static void SetTiledMultiResLevel(int level)
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                Internal.SetTiledMultiResLevel(level);
            }
        }

        internal static int GetTiledMultiResLevel()
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                return Internal.GetTiledMultiResLevel();
            }
            else
            {
                return -1;
            }
        }

        internal static void SetTiledMultiResDynamic(bool isDynamic)
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                Internal.SetTiledMultiResDynamic(isDynamic);
            }
        }

        internal static bool GetEyeTrackedFoveatedRenderingSupported()
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                return Internal.GetEyeTrackedFoveatedRenderingSupported();
            }
            else
            {
                return false;
            }
        }

        internal static bool GetEyeTrackedFoveatedRenderingEnabled()
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                return Internal.GetEyeTrackedFoveatedRenderingEnabled();
            }
            else
            {
                return false;
            }
        }

        internal static void SetEyeTrackedFoveatedRenderingEnabled(bool isEnabled)
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                Internal.SetEyeTrackedFoveatedRenderingEnabled(isEnabled);
            }
        }

        internal static bool GetShouldRestartSession()
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                return Internal.GetShouldRestartSession();
            } 
            else
            {
                return false;
            }
        }

        internal static void SetupEnvironmentDepth(Utils.EnvironmentDepthCreateParams createParams)
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                EnvironmentDepthCreateParamsInternal param = new EnvironmentDepthCreateParamsInternal();
                param.removeHands = createParams.removeHands;

                Internal.SetupEnvironmentDepth(ref param);
            }
        }

        internal static void SetEnvironmentDepthRendering(bool isEnabled)
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                Internal.SetEnvironmentDepthRendering(isEnabled);
            }
        }

        internal static void ShutdownEnvironmentDepth()
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                Internal.ShutdownEnvironmentDepth();
            }
        }

        internal static bool GetEnvironmentDepthTextureId(ref uint id)
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                return Internal.GetEnvironmentDepthTextureId(ref id);
            }
            else
            {
                return false;
            }
        }

        internal static Utils.EnvironmentDepthFrameDesc GetEnvironmentDepthFrameDesc(int eye)
        {
            Utils.EnvironmentDepthFrameDesc desc = new Utils.EnvironmentDepthFrameDesc();
            desc.isValid = false;

            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
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
            }

            return desc;
        }

        internal static bool SetEnvironmentDepthHandRemoval(bool isEnabled)
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                return Internal.SetEnvironmentDepthHandRemoval(isEnabled);
            }
            else
            {
                return false;
            }
        }

        internal static bool GetEnvironmentDepthSupported()
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                return Internal.GetEnvironmentDepthSupported();
            }
            else
            {
                return false;
            }
        }

        internal static bool GetEnvironmentDepthHandRemovalSupported()
        {
            if (RuntimePlatformChecks.IsSupportedPlatform())
            {
                return Internal.GetEnvironmentDepthHandRemovalSupported();
            }
            else
            {
                return false;
            }
        }

        private static class Internal
        {
            [DllImport("OculusXRPlugin")]
            internal static extern void SetColorScale(float x, float y, float z, float w);

            [DllImport("OculusXRPlugin")]
            internal static extern void SetColorOffset(float x, float y, float z, float w);

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool GetIsSupportedDevice();

            [DllImport("OculusXRPlugin", CharSet = CharSet.Unicode)]
            [return: MarshalAs(UnmanagedType.U1)]
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
            internal static extern void EnablePerfMetrics([MarshalAs(UnmanagedType.I1)] bool enable);

            [DllImport("OculusXRPlugin")]
            internal static extern void EnableAppMetrics([MarshalAs(UnmanagedType.I1)] bool enable);

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool SetDeveloperModeStrict([MarshalAs(UnmanagedType.I1)] bool active);

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool GetAppHasInputFocus();

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool GetBoundaryConfigured();

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool GetBoundaryDimensions(Boundary.BoundaryType boundaryType, out Vector3 dimensions);

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool GetBoundaryVisible();

            [DllImport("OculusXRPlugin")]
            internal static extern void SetBoundaryVisible([MarshalAs(UnmanagedType.I1)] bool boundaryVisible);

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool GetAppShouldQuit();

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool GetDisplayAvailableFrequencies(IntPtr ptr, ref int numFrequencies);

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool SetDisplayFrequency(float refreshRate);

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool GetDisplayFrequency(out float refreshRate);

            [DllImport("OculusXRPlugin")]
            internal static extern SystemHeadset GetSystemHeadsetType();

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool GetTiledMultiResSupported();

            [DllImport("OculusXRPlugin")]
            internal static extern void SetTiledMultiResLevel(int level);

            [DllImport("OculusXRPlugin")]
            internal static extern int GetTiledMultiResLevel();

            [DllImport("OculusXRPlugin")]
            internal static extern void SetTiledMultiResDynamic([MarshalAs(UnmanagedType.I1)] bool isDynamic);

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool GetEyeTrackedFoveatedRenderingSupported();

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool GetEyeTrackedFoveatedRenderingEnabled();

            [DllImport("OculusXRPlugin")]
            internal static extern void SetEyeTrackedFoveatedRenderingEnabled([MarshalAs(UnmanagedType.I1)] bool isEnabled);

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool GetShouldRestartSession();

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool SetupEnvironmentDepth(ref EnvironmentDepthCreateParamsInternal createParams);

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool SetEnvironmentDepthRendering([MarshalAs(UnmanagedType.I1)] bool isEnabled);

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool ShutdownEnvironmentDepth();

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool GetEnvironmentDepthTextureId(ref uint id);

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool GetEnvironmentDepthFrameDesc(ref EnvironmentDepthFrameDescInternal frameDesc, int eye);

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool SetEnvironmentDepthHandRemoval([MarshalAs(UnmanagedType.I1)] bool isEnabled);

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool GetEnvironmentDepthSupported();

            [DllImport("OculusXRPlugin")]
            [return: MarshalAs(UnmanagedType.U1)]
            internal static extern bool GetEnvironmentDepthHandRemovalSupported();
        }
    }
}
