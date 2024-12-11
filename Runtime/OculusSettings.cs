using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Management;

namespace Unity.XR.Oculus
{
    [System.Serializable]
    [XRConfigurationData("Oculus", "Unity.XR.Oculus.Settings")]
    public class OculusSettings : ScriptableObject
    {
        public const string kUseStickControlThumbsticksDefine = "META_USE_STICK_CONTROL_THUMBSTICKS";

        public enum StereoRenderingModeDesktop
        {
            /// <summary>
            /// Unity makes two passes across the scene graph, each one entirely indepedent of the other.
            /// Each pass has its own eye matrices and render target. Unity draws everything twice, which includes setting the graphics state for each pass.
            /// This is a slow and simple rendering method which doesn't require any special modification to shaders.
            /// </summary>
            MultiPass = 0,
            /// <summary>
            /// Unity uses a single texture array with two elements. Unity converts each call into an instanced draw call.
            /// Shaders need to be aware of this. Unity's shader macros handle the situation.
            /// </summary>
            SinglePassInstanced = 1,
        }

        public enum StereoRenderingModeAndroid
        {
            /// <summary>
            /// Unity makes two passes across the scene graph, each one entirely indepedent of the other.
            /// Each pass has its own eye matrices and render target. Unity draws everything twice, which includes setting the graphics state for each pass.
            /// This is a slow and simple rendering method which doesn't require any special modification to shaders.
            /// </summary>
            MultiPass = 0,
            /// <summary>
            /// Unity uses a single texture array with two elements.
            /// Multiview is very similar to Single Pass Instanced; however, the graphics driver converts each call into an instanced draw call so it requires less work on Unity's side.
            /// As with Single Pass Instanced, shaders need to be aware of the Multiview setting. Unity's shader macros handle the situation.
            /// </summary>
            Multiview = 2
        }

        public enum FoveationMethod
        {
            /// <summary>
            /// Fixed Foveated Rendering
            /// Foveates the image based on a fixed pattern.
            /// </summary>
            FixedFoveatedRendering = 0,
            /// <summary>
            /// Eye Tracked Foveated Rendering
            /// Foveates the image using eye tracking.
            /// </summary>
            EyeTrackedFoveatedRendering = 1,

#if UNITY_2023_2_OR_NEWER && UNITY_URP_16 // Unity's Foveated Rendering API For URP is a new feature starting with Unity 2023.2
            /// <summary>
            /// Fixed Foveated Rendering with Unity's Foveated Rendering API (URP only)
            /// Foveates the image based on a fixed pattern, this version uses the Unity common Foveated Rendering API
            /// </summary>
            FixedFoveatedRenderingUsingUnityAPIForURP = 2,
            /// <summary>
            /// Eye Tracked Foveated Rendering with Unity's Foveated Rendering API (URP only)
            /// Foveates the image using eye tracking, this version uses the Unity common Foveated Rendering API
            /// </summary>
            EyeTrackedFoveatedRenderingUsingUnityAPIForURP = 3
#endif
        }

        /// <summary>
        /// The current stereo rendering mode selected for desktop-based Oculus platforms
        /// </summary>
        [SerializeField, Tooltip("The current stereo rendering mode selected for desktop-based Oculus platforms.")]
        public StereoRenderingModeDesktop m_StereoRenderingModeDesktop = StereoRenderingModeDesktop.SinglePassInstanced;

        /// <summary>
        /// The current stereo rendering mode selected for Android-based Oculus platforms
        /// </summary>
        [SerializeField, Tooltip("The current stereo rendering mode selected for Android-based Oculus platforms.")]
        public StereoRenderingModeAndroid m_StereoRenderingModeAndroid = StereoRenderingModeAndroid.Multiview;

        /// <summary>
        /// Enable or disable support for using a shared depth buffer. This allows Unity and Oculus to use a common depth buffer which enables Oculus to composite the Oculus Dash and other utilities over the Unity application.
        /// </summary>
        [SerializeField, Tooltip("Allows Unity and the Oculus runtime to share a common depth buffer for better scene integration with the Dash.")]
        public bool SharedDepthBuffer = true;

        /// <summary>
        /// Enables support for submitting the depth buffer on mobile. This allows for depth testing between layers on mobile Oculus platforms.
        /// </summary>
        [SerializeField, Tooltip("Enables support for submitting the depth buffer on mobile. This allows for depth testing between layers on mobile Oculus platforms.")]
        public bool DepthSubmission = false;

        /// <summary>
        /// Enable or disable Dash support. This inintializes the Oculus Plugin with Dash support which enables the Oculus Dash to composite over the Unity application.
        /// </summary>
        [SerializeField, Tooltip("Initialize the Oculus Plugin with Dash support which allows the Oculus Dash to composite over the Unity application.")]
        public bool DashSupport = true;

        /// <summary>
        /// If enabled, the GLES graphics driver will bypass validation code, potentially running faster.
        /// </summary>
        [SerializeField, Tooltip("If enabled, the GLES graphics driver will bypass validation code, potentially running faster at the expense of detecting and reporting errors. GLES only.")]
        public bool LowOverheadMode = false;

        /// <summary>
        /// If enabled, the depth buffer and MSAA contents will be discarded rather than resolved. This is an optimization that can possibly break rendering in certain cases. Vulkan only.
        /// </summary>
        [SerializeField, Tooltip("If enabled, the depth buffer and MSAA contents will be discarded rather than resolved. This is an optimization that can possibly break rendering in certain cases. Vulkan only.")]
        public bool OptimizeBufferDiscards = true;

        /// <summary>
        /// Enables a latency optimization technique which can reduce simulation latency by several ms, depending on application workload.
        /// </summary>
        [SerializeField, Tooltip("Enables a latency optimization technique which can reduce simulation latency by several ms, depending on application workload. Note: Phase Sync is now always active and this setting has been deprecated.")]
        [Obsolete("Phase Sync is always active and this setting has been deprecated", false)]
        public bool PhaseSync = true;

        /// <summary>
        /// Allows the application to render with symmetric projection matrices which can improve performance when using multiview.
        /// </summary>
        [SerializeField, Tooltip("Allows the application to render with symmetric projection matrices.")]
        public bool SymmetricProjection = true;

        /// <summary>
        /// Enables a subsampled eye texture layout, which can improve performance when using FFR and reduce FFR related artifacts. Vulkan only.
        /// </summary>
        [SerializeField, Tooltip("Enables a subsampled eye texture layout, which can improve performance when using FFR and reduce FFR related artifacts. Vulkan only.")]
        public bool SubsampledLayout = false;

        /// <summary>
        /// Choose which foveated rendering method is used when foveation is enabled.
        /// </summary>
        [SerializeField, Tooltip("Choose which foveated rendering method is used when foveation is enabled.")]
        public FoveationMethod FoveatedRenderingMethod = FoveationMethod.FixedFoveatedRendering;

        /// <summary>
        /// Reduces tracked rendering latency by updating head and controller poses as late as possible before rendering. Vulkan only.
        /// </summary>
        [SerializeField, Tooltip("Reduces tracked rendering latency by updating head and controller poses as late as possible before rendering. Vulkan only.")]
        public bool LateLatching = false;

        /// <summary>
        /// Debug mode for Late Latching which will print information about the Late Latching system as well as any errors.
        /// </summary>
        [SerializeField, Tooltip("Debug mode for Late Latching which will print information about the Late Latching system as well as any errors.")]
        public bool LateLatchingDebug = false;

        /// <summary>
        /// When the Tracking Origin Mode is set to Floor, the tracking origin won't change with a system recenter.
        /// </summary>
        [SerializeField, Tooltip("When the Tracking Origin Mode is set to Floor, the tracking origin won't change with a system recenter.")]
        public bool EnableTrackingOriginStageMode = false;

        /// <summary>
        /// A frame synthesis technology to allow your application to render at half frame rate, while still delivering a smooth experience. Note that this currently requires a custom version of the URP provided by Oculus in order to work, and should not be enabled if you aren't using that customized Oculus URP package.
        /// </summary>
        [SerializeField, Tooltip("A frame synthesis technology to allow your application to render at half frame rate, while still delivering a smooth experience. Note that this currently requires a custom version of the URP provided by Oculus in order to work, and should not be enabled if you aren't using that customized Oculus URP package.")]
        public bool SpaceWarp = false;

        /// <summary>
        /// Adds a Quest 2 entry to the supported devices list in the Android manifest.
        /// </summary>
        [SerializeField, Tooltip("Adds a Quest 2 entry to the supported devices list in the Android manifest.")]
        public bool TargetQuest2 = true;

        /// <summary>
        /// Adds a Quest Pro entry to the supported devices list in the Android manifest.
        /// </summary>
        [SerializeField, Tooltip("Adds a Quest Pro entry to the supported devices list in the Android manifest.")]
        public bool TargetQuestPro = false;

        /// <summary>
        /// Adds a Quest 3 entry to the supported devices list in the Android manifest.
        /// </summary>
        [SerializeField, Tooltip("Adds a Quest 3 entry to the supported devices list in the Android manifest.")]
        public bool TargetQuest3 = false;

        /// <summary>
        /// Adds a Quest 3S entry to the supported devices list in the Android manifest.
        /// </summary>
        [SerializeField, Tooltip("Adds a Quest 3S entry to the supported devices list in the Android manifest.")]
        public bool TargetQuest3S = false;

        /// <summary>
        /// Adds a PNG under the Assets folder as the system splash screen image. If set, the OS will display the system splash screen as a high quality compositor layer as soon as the app is starting to launch until the app submits the first frame.
        /// </summary>
        [SerializeField, Tooltip("Adds a PNG under the Assets folder as the system splash screen image. If set, the OS will display the system splash screen as a high quality compositor layer as soon as the app is starting to launch until the app submits the first frame.")]
        public Texture2D SystemSplashScreen;

        /// <summary>
        /// Switch to use StickControl thumbsticks instead of Vector2Control, but may break existing projects that have code dependencies to the Vector2Control type. StickControl allows more input options for thumbstick-based control, such as acting as both a combined 2D vector, two independent axes or a four-way Dpad with 4 independent buttons. This setting affects both Android and Standalone settings. Only works with Input System package version 1.6.2 onwards.
        /// </summary>
        [SerializeField, Tooltip("Switch to use StickControl thumbsticks instead of Vector2Control, but may break existing projects that have code dependencies to the Vector2Control type. StickControl allows more input options for thumbstick-based control, such as acting as both a combined 2D vector, two independent axes or a four-way Dpad with 4 independent buttons. This setting affects both Android and Standalone settings. Only works with Input System package version 1.6.2 onwards.")]
        public bool UseStickControlThumbsticks = false;

        /// <summary>
        /// If enabled, optimize the viewports, scissors and render areas per eye to reduce rendering work. Vulkan with Multiview and Symmetric Projection only.
        /// </summary>
        [SerializeField, Tooltip("If enabled, optimize the viewports, scissors and render areas per eye to reduce rendering work. Vulkan with Multiview and Symmetric Projection only.")]
        public bool OptimizeMultiviewRenderRegions = false;


        public ushort GetStereoRenderingMode()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return (ushort)m_StereoRenderingModeAndroid;
# else
            return (ushort)m_StereoRenderingModeDesktop;
#endif
        }

        /// <summary>
        /// Return true if the current Foveated Rendering settings are configured to request eye tracking on devices that supports it.
        /// </summary>
        public bool IsEyeTrackingRequested()
        {
#if UNITY_2023_2_OR_NEWER && UNITY_URP_16
            return FoveatedRenderingMethod == FoveationMethod.EyeTrackedFoveatedRendering
                   || FoveatedRenderingMethod == FoveationMethod.EyeTrackedFoveatedRenderingUsingUnityAPIForURP;
#else
            return FoveatedRenderingMethod == FoveationMethod.EyeTrackedFoveatedRendering;
#endif
        }

#if !UNITY_EDITOR
        public static OculusSettings s_Settings;

        public void Awake()
        {
            s_Settings = this;
        }
#else
        private void OnValidate()
        {
            if(SystemSplashScreen == null)
                return;

            string splashScreenAssetPath = AssetDatabase.GetAssetPath(SystemSplashScreen);
            if (Path.GetExtension(splashScreenAssetPath).ToLower() != ".png")
            {
                SystemSplashScreen = null;
                throw new ArgumentException("Invalid file format of System Splash Screen. It has to be a PNG file to be used by the Quest OS. The asset path: " + splashScreenAssetPath);
            }
        }
#endif
    }
}
