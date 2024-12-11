using UnityEngine;
using System;
using UnityEngine.Android;

namespace Unity.XR.Oculus
{
    public static partial class Utils
    {
        public struct EnvironmentDepthFrameDesc
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

        public struct EnvironmentDepthCreateParams
        {
            public bool removeHands;
        }

        private static EnvironmentDepthCreateParams s_EnvironmentDepthCreateParams;

        static void ScenePermissionGrantedCallback(string permissionName)
        {
            if (permissionName == "com.oculus.permission.USE_SCENE")
            {
                NativeMethods.SetupEnvironmentDepth(s_EnvironmentDepthCreateParams);
            }
        }

        public static bool IsScenePermissionGranted()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return Permission.HasUserAuthorizedPermission("com.oculus.permission.USE_SCENE");
#else
            return true;
#endif
        }

        public static void SetupEnvironmentDepth(EnvironmentDepthCreateParams createParams)
        {
            if (IsScenePermissionGranted())
            {
                NativeMethods.SetupEnvironmentDepth(createParams);
            }
            else
            {
                s_EnvironmentDepthCreateParams = createParams;
                var permissionCallbacks = new PermissionCallbacks();
                permissionCallbacks.PermissionGranted += ScenePermissionGrantedCallback;
                Permission.RequestUserPermission("com.oculus.permission.USE_SCENE", permissionCallbacks);
            }
        }

        public static void SetEnvironmentDepthRendering(bool isEnabled)
        {
            if (IsScenePermissionGranted())
            {
                NativeMethods.SetEnvironmentDepthRendering(isEnabled);
            }
            else
            {
                Debug.LogError("Failed to set environment depth rendering because permission was not given.");
            }
        }

        public static void ShutdownEnvironmentDepth()
        {
            NativeMethods.ShutdownEnvironmentDepth();
        }

        public static bool GetEnvironmentDepthTextureId(ref uint id)
        {
            return NativeMethods.GetEnvironmentDepthTextureId(ref id);
        }

        public static EnvironmentDepthFrameDesc GetEnvironmentDepthFrameDesc(int eye)
        {
            return NativeMethods.GetEnvironmentDepthFrameDesc(eye);
        }

        public static bool SetEnvironmentDepthHandRemoval(bool isEnabled)
        {
            return NativeMethods.SetEnvironmentDepthHandRemoval(isEnabled);
        }

        public static bool GetEnvironmentDepthSupported()
        {
            return NativeMethods.GetEnvironmentDepthSupported();
        }

        public static bool GetEnvironmentDepthHandRemovalSupported()
        {
            return NativeMethods.GetEnvironmentDepthHandRemovalSupported();
        }
    }
}
