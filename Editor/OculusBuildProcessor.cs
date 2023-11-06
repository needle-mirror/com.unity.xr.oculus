using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor.Android;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using Unity.XR.Oculus;
using UnityEditor;
using UnityEditor.XR.Management;
using UnityEngine.XR.Management;

namespace UnityEditor.XR.Oculus
{
    public class OculusBuildProcessor : XRBuildHelper<OculusSettings>
    {
        public override string BuildSettingsKey { get {return "Unity.XR.Oculus.Settings";} }

        private static List<BuildTarget> s_ValidStandaloneBuildTargets = new List<BuildTarget>()
        {
            BuildTarget.StandaloneWindows64
        };

        private bool IsCurrentBuildTargetVaild(BuildReport report)
        {
            return report.summary.platformGroup == BuildTargetGroup.Android ||
                (report.summary.platformGroup == BuildTargetGroup.Standalone && s_ValidStandaloneBuildTargets.Contains(report.summary.platform));
        }

        private bool HasLoaderEnabledForTarget(BuildTargetGroup buildTargetGroup)
        {
            if (buildTargetGroup != BuildTargetGroup.Standalone && buildTargetGroup != BuildTargetGroup.Android)
                return false;

            XRGeneralSettings settings = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(buildTargetGroup);
            if (settings == null)
                return false;

            bool loaderFound = false;
            for (int i = 0; i < settings.Manager.activeLoaders.Count; ++i)
            {
                if (settings.Manager.activeLoaders[i] as OculusLoader != null)
                {
                    loaderFound = true;
                    break;
                }
            }

            return loaderFound;
        }

        private readonly string spatializerPluginName = "AudioPluginOculusSpatializer";
        private readonly string spatializerReadableName = "OculusSpatializer";

        private readonly string[] runtimePluginNames = new string[]
        {
            "OculusXRPlugin.dll",
            "OVRPlugin.dll",
            "libOculusXRPlugin.so",
            "OVRPlugin.aar"
        };

        private bool ShouldIncludeRuntimePluginsInBuild(string path, BuildTargetGroup platformGroup)
        {
            return HasLoaderEnabledForTarget(platformGroup);
        }

        private bool ShouldIncludeSpatializerPluginsInBuild(string path)
        {
            string currentSpatializerPluginName = AudioSettings.GetSpatializerPluginName();

            if (string.Compare(spatializerReadableName, currentSpatializerPluginName, true) == 0)
                return true;

            return false;
        }

        /// <summary>OnPreprocessBuild override to provide XR Plugin specific build actions.</summary>
        /// <param name="report">The build report.</param>
        public override void OnPreprocessBuild(BuildReport report)
        {
            if (IsCurrentBuildTargetVaild(report) && HasLoaderEnabledForTarget(report.summary.platformGroup))
                base.OnPreprocessBuild(report);

            var allPlugins = PluginImporter.GetAllImporters();
            foreach (var plugin in allPlugins)
            {
                if (plugin.isNativePlugin)
                {
                    foreach (var pluginName in runtimePluginNames)
                    {
                        if (plugin.assetPath.Contains(pluginName))
                        {
                            plugin.SetIncludeInBuildDelegate((path) => { return ShouldIncludeRuntimePluginsInBuild(path, report.summary.platformGroup); });
                            break;
                        }
                    }

                    // exlude spatializer related plugins if OculusSpatializer not selected under Audio setting
                    if (plugin.assetPath.Contains(spatializerPluginName))
                    {
                        plugin.SetIncludeInBuildDelegate(ShouldIncludeSpatializerPluginsInBuild);
                    }
                }
            }
        }
    }

    public static class OculusBuildTools
    {
        public static bool OculusLoaderPresentInSettingsForBuildTarget(BuildTargetGroup btg)
        {
            var generalSettingsForBuildTarget = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(btg);
            if (!generalSettingsForBuildTarget)
                return false;
            var settings = generalSettingsForBuildTarget.AssignedSettings;
            if (!settings)
                return false;

            bool loaderFound = false;
            for (int i = 0; i < settings.activeLoaders.Count; ++i)
            {
                if (settings.activeLoaders[i] as OculusLoader != null)
                {
                    loaderFound = true;
                    break;
                }
            }

            return loaderFound;
        }

        public static OculusSettings GetSettings()
        {
            OculusSettings settings = null;
#if UNITY_EDITOR
            UnityEditor.EditorBuildSettings.TryGetConfigObject<OculusSettings>("Unity.XR.Oculus.Settings", out settings);
#else
            settings = OculusSettings.s_Settings;
#endif
            return settings;
        }
    }

    [InitializeOnLoad]
    public static class OculusEnterPlayModeSettingsCheck
    {
        static OculusEnterPlayModeSettingsCheck()
        {
            EditorApplication.playModeStateChanged += PlaymodeStateChangedEvent;
        }

        private static void PlaymodeStateChangedEvent(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                if (!OculusBuildTools.OculusLoaderPresentInSettingsForBuildTarget(BuildTargetGroup.Standalone))
                {
                    return;
                }

                if (PlayerSettings.GetGraphicsAPIs(BuildTarget.StandaloneWindows)[0] !=
                    GraphicsDeviceType.Direct3D11)
                {
                    Debug.LogError("D3D11 is currently the only graphics API compatible with the Oculus XR Plugin on desktop platforms. Please change the preferred Graphics API setting in Player Settings.");
                }
            }
        }
    }

    internal class OculusBuildHooks : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder { get; }

        private static readonly Dictionary<string, string> AndroidBootConfigVars = new Dictionary<string, string>()
        {
            { "xr-usable-core-mask-enabled", "1" },
            { "xr-vulkan-extension-fragment-density-map-enabled", "1" },
            { "xr-low-latency-audio-enabled", "1"},
            { "xr-require-backbuffer-textures", "0" },
            { "xr-keyboard-overlay-enabled", "1" },
            { "xr-pipeline-cache-enabled", "1" },
            { "xr-skip-B10G11R11-special-casing", "1" },
            { "xr-hide-memoryless-render-texture", "1" },
            { "xr-skip-audio-buffer-size-check", "1" },
            { "xr-meta-enabled", "1" }
        };

        public void OnPreprocessBuild(BuildReport report)
        {
            if(!OculusBuildTools.OculusLoaderPresentInSettingsForBuildTarget(report.summary.platformGroup))
                return;

            var settings = OculusBuildTools.GetSettings();

            if (report.summary.platformGroup == BuildTargetGroup.Android)
            {
                GraphicsDeviceType firstGfxType = PlayerSettings.GetGraphicsAPIs(report.summary.platform)[0];

#if UNITY_2023_1_OR_NEWER
                if (firstGfxType != GraphicsDeviceType.OpenGLES3 && firstGfxType != GraphicsDeviceType.Vulkan)
                {
                    throw new BuildFailedException("OpenGLES3 and Vulkan are currently the only graphics APIs compatible with the Oculus XR Plugin on mobile platforms.");
                }

                if (PlayerSettings.colorSpace != ColorSpace.Linear && (firstGfxType == GraphicsDeviceType.OpenGLES3))
                {
                    throw new BuildFailedException("Only Linear Color Space is supported when using OpenGLES. Please set Color Space to Linear in Player Settings, or switch to Vulkan.");
                }
#else
                if (firstGfxType != GraphicsDeviceType.OpenGLES3 && firstGfxType != GraphicsDeviceType.Vulkan && firstGfxType != GraphicsDeviceType.OpenGLES2)
                {
                    throw new BuildFailedException("OpenGLES2, OpenGLES3, and Vulkan are currently the only graphics APIs compatible with the Oculus XR Plugin on mobile platforms.");
                }

                if (PlayerSettings.colorSpace != ColorSpace.Linear && (firstGfxType == GraphicsDeviceType.OpenGLES3 || firstGfxType == GraphicsDeviceType.OpenGLES2))
                {
                    throw new BuildFailedException("Only Linear Color Space is supported when using OpenGLES. Please set Color Space to Linear in Player Settings, or switch to Vulkan.");
                }
#endif

                if (PlayerSettings.Android.minSdkVersion < AndroidSdkVersions.AndroidApiLevel23)
                {
                    throw new BuildFailedException("Android Minimum API Level must be set to 23 or higher for the Oculus XR Plugin.");
                }

                // some features don't work in non-ARM64 builds
                if ((PlayerSettings.Android.targetArchitectures & AndroidArchitecture.ARM64) != AndroidArchitecture.ARM64)
                {
                    // ETFR requires ARM64
                    if ((settings != null) && (settings.FoveatedRenderingMethod == OculusSettings.FoveationMethod.EyeTrackedFoveatedRendering))
                    {
                        throw new BuildFailedException("Eye Tracked Foveated Rendering can only be enabled when ARM64 is selected as the Target Architecture.");
                    }
                }

                // write Android Meta tags to bootconfig
                var bootConfig = new BootConfig(report);
                bootConfig.ReadBootConfig();

                foreach (var entry in AndroidBootConfigVars)
                {
                    bootConfig.SetValueForKey(entry.Key, entry.Value);
                }

                bootConfig.WriteBootConfig();
            }

            if (report.summary.platform == BuildTarget.StandaloneWindows)
            {
                throw new BuildFailedException("The Oculus XR Plugin doesn't support 32-bit Windows player builds. Please use 64-bit instead.");
            }

            if (report.summary.platform == BuildTarget.StandaloneWindows || report.summary.platform == BuildTarget.StandaloneWindows64)
            {
                if (PlayerSettings.GetGraphicsAPIs(report.summary.platform)[0] !=
                    GraphicsDeviceType.Direct3D11)
                {
                    throw new BuildFailedException("D3D11 is currently the only graphics API compatible with the Oculus XR Plugin on desktop platforms. Please change the Graphics API setting in Player Settings.");
                }
            }
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            if(!OculusBuildTools.OculusLoaderPresentInSettingsForBuildTarget(report.summary.platformGroup))
                return;

            if (report.summary.platformGroup == BuildTargetGroup.Android)
            {
                // clean up Android Meta boot settings after build
                BootConfig bootConfig = new BootConfig(report);
                bootConfig.ReadBootConfig();

                foreach (KeyValuePair<string, string> entry in AndroidBootConfigVars)
                {
                    bootConfig.ClearEntryForKeyAndValue(entry.Key, entry.Value);
                }

                bootConfig.WriteBootConfig();

                // verify settings
                var settings = OculusBuildTools.GetSettings();
                if (settings != null)
                {
                    GraphicsDeviceType firstGfxType = PlayerSettings.GetGraphicsAPIs(report.summary.platform)[0];
                    
                    if (settings.SymmetricProjection && (settings.m_StereoRenderingModeAndroid != OculusSettings.StereoRenderingModeAndroid.Multiview || firstGfxType != GraphicsDeviceType.Vulkan))
                    {
                        Debug.LogWarning("Symmetric Projection is only supported with Vulkan and Multiview.");
                    }
                    
                    if (settings.SubsampledLayout && (firstGfxType != GraphicsDeviceType.Vulkan))
                    {
                        Debug.LogWarning("Subsampled Layout is only supported with Vulkan.");
                    }

                    if (settings.DepthSubmission && (settings.m_StereoRenderingModeAndroid != OculusSettings.StereoRenderingModeAndroid.Multiview || firstGfxType != GraphicsDeviceType.Vulkan))
                    {
                        Debug.LogWarning("Depth Submission is only supported on Vulkan with Multiview.");
                    }

                    if (settings.DepthSubmission)
                    {
                        Debug.LogWarning("Enabling Depth Submission may cause a crash on application startup if MSAA is not enabled. This will be resolved in future versions of Unity.");
                    }
                }
            }

            if (EditorUserBuildSettings.waitForManagedDebugger && report.summary.platformGroup == BuildTargetGroup.Android && ((report.summary.options & BuildOptions.AutoRunPlayer) != 0))
                Debug.Log("[Wait For Managed Debugger To Attach] Use volume Up or Down button on headset to confirm ...\n");
        }
    }

    /// <summary>
    /// Small utility class for reading, updating and writing boot config.
    /// </summary>
    internal class BootConfig
    {
        private const string XrBootSettingsKey = "xr-boot-settings";

        private readonly Dictionary<string, string> bootConfigSettings;
        private readonly string buildTargetName;

        public BootConfig(BuildReport report)
        {
            bootConfigSettings = new Dictionary<string, string>();
            buildTargetName = BuildPipeline.GetBuildTargetName(report.summary.platform);
        }

        public void ReadBootConfig()
        {
            bootConfigSettings.Clear();

            string xrBootSettings = EditorUserBuildSettings.GetPlatformSettings(buildTargetName, XrBootSettingsKey);
            if (!string.IsNullOrEmpty(xrBootSettings))
            {
                // boot settings string format
                // <boot setting>:<value>[;<boot setting>:<value>]*
                var bootSettings = xrBootSettings.Split(';');
                foreach (var bootSetting in bootSettings)
                {
                    var setting = bootSetting.Split(':');
                    if (setting.Length == 2 && !string.IsNullOrEmpty(setting[0]) && !string.IsNullOrEmpty(setting[1]))
                    {
                        bootConfigSettings.Add(setting[0], setting[1]);
                    }
                }
            }
        }

        public void SetValueForKey(string key, string value) => bootConfigSettings[key] = value;

        public bool TryGetValue(string key, out string value) => bootConfigSettings.TryGetValue(key, out value);

        public void ClearEntryForKeyAndValue(string key, string value)
        {
            if (bootConfigSettings.TryGetValue(key, out string dictValue) && dictValue == value)
            {
                bootConfigSettings.Remove(key);
            }
        }

        public void WriteBootConfig()
        {
            // boot settings string format
            // <boot setting>:<value>[;<boot setting>:<value>]*
            bool firstEntry = true;
            var sb = new System.Text.StringBuilder();
            foreach (var kvp in bootConfigSettings)
            {
                if (!firstEntry)
                {
                    sb.Append(";");
                }

                sb.Append($"{kvp.Key}:{kvp.Value}");
                firstEntry = false;
            }

            EditorUserBuildSettings.SetPlatformSettings(buildTargetName, XrBootSettingsKey, sb.ToString());
        }
    }

#if UNITY_ANDROID
    internal class OculusManifest : IPostGenerateGradleAndroidProject
    {
        static readonly string k_AndroidURI = "http://schemas.android.com/apk/res/android";
        static readonly string k_AndroidManifestPath = "/src/main/AndroidManifest.xml";
        static readonly string k_AndroidProGuardPath = "/proguard-unity.txt";
        static readonly string k_OculusProGuardRule = Environment.NewLine + "-keep class com.unity.oculus.OculusUnity { *; }" + Environment.NewLine;

        void UpdateOrCreateAttributeInTag(XmlDocument doc, string parentPath, string tag, string name, string value)
        {
            var xmlNode = doc.SelectSingleNode(parentPath + "/" + tag);

            if (xmlNode != null)
            {
                ((XmlElement)xmlNode).SetAttribute(name, k_AndroidURI, value);
            }
        }

        void UpdateOrCreateNameValueElementsInTag(XmlDocument doc, string parentPath, string tag,
            string firstName, string firstValue, string secondName, string secondValue)
        {
            var xmlNodeList = doc.SelectNodes(parentPath + "/" + tag);

            foreach (XmlNode node in xmlNodeList)
            {
                var attributeList = ((XmlElement)node).Attributes;

                foreach (XmlAttribute attrib in attributeList)
                {
                    if (attrib.Value == firstValue)
                    {
                        XmlAttribute valueAttrib = attributeList[secondName, k_AndroidURI];
                        if (valueAttrib != null)
                        {
                            valueAttrib.Value = secondValue;
                        }
                        else
                        {
                            ((XmlElement)node).SetAttribute(secondName, k_AndroidURI, secondValue);
                        }
                        return;
                    }
                }
            }

            // Didn't find any attributes that matched, create both (or all three)
            XmlElement childElement = doc.CreateElement(tag);
            childElement.SetAttribute(firstName, k_AndroidURI, firstValue);
            childElement.SetAttribute(secondName, k_AndroidURI, secondValue);

            var xmlParentNode = doc.SelectSingleNode(parentPath);

            if (xmlParentNode != null)
            {
                xmlParentNode.AppendChild(childElement);
            }
        }

        // same as above, but don't create if the node already exists
        void CreateNameValueElementsInTag(XmlDocument doc, string parentPath, string tag,
            string firstName, string firstValue, string secondName=null, string secondValue=null, string thirdName=null, string thirdValue=null)
        {
            var xmlNodeList = doc.SelectNodes(parentPath + "/" + tag);

            // don't create if the firstValue matches
            foreach (XmlNode node in xmlNodeList)
            {
                foreach (XmlAttribute attrib in node.Attributes)
                {
                    if (attrib.Value == firstValue)
                    {
                        return;
                    }
                }
            }

            XmlElement childElement = doc.CreateElement(tag);
            childElement.SetAttribute(firstName, k_AndroidURI, firstValue);

            if (secondValue != null)
            {
                childElement.SetAttribute(secondName, k_AndroidURI, secondValue);
            }

            if (thirdValue != null)
            {
                childElement.SetAttribute(thirdName, k_AndroidURI, thirdValue);
            }

            var xmlParentNode = doc.SelectSingleNode(parentPath);

            if (xmlParentNode != null)
            {
                xmlParentNode.AppendChild(childElement);
            }
        }

        void RemoveNameValueElementInTag(XmlDocument doc, string parentPath, string tag, string name, string value)
        {
            var xmlNodeList = doc.SelectNodes(parentPath + "/" + tag);

            foreach (XmlNode node in xmlNodeList)
            {
                var attributeList = ((XmlElement)node).Attributes;

                foreach (XmlAttribute attrib in attributeList)
                {
                    if (attrib.Name == name && attrib.Value == value)
                    {
                        node.ParentNode?.RemoveChild(node);
                    }
                }
            }
        }

        // disable ProGuard on Oculus Java files
        void AddProGuardRule(string path)
        {
            try
            {
                var proguardPath = path + k_AndroidProGuardPath;

                if (File.Exists(proguardPath))
                {
                    File.AppendAllText(proguardPath, k_OculusProGuardRule);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Failed to append Oculus rule to ProGuard file: " + e.ToString());
            }
        }

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            if(!OculusBuildTools.OculusLoaderPresentInSettingsForBuildTarget(BuildTargetGroup.Android))
                return;

            AddProGuardRule(path);

            var manifestPath = path + k_AndroidManifestPath;
            var manifestDoc = new XmlDocument();
            manifestDoc.Load(manifestPath);

            var sdkVersion = (int)PlayerSettings.Android.minSdkVersion;

            UpdateOrCreateAttributeInTag(manifestDoc, "/", "manifest", "installLocation", "auto");

            var nodePath = "/manifest/application";
            UpdateOrCreateNameValueElementsInTag(manifestDoc, nodePath, "meta-data", "name", "com.samsung.android.vr.application.mode", "value", "vr_only");

            var settings = OculusBuildTools.GetSettings();

            var lowOverheadModeVal = ((settings != null) && settings.LowOverheadMode) ? "true" : "false";
            UpdateOrCreateNameValueElementsInTag(manifestDoc, nodePath, "meta-data", "name", "com.unity.xr.oculus.LowOverheadMode", "value", lowOverheadModeVal);

            var lateLatchingVal = ((settings != null) && settings.LateLatching) ? "true" : "false";
            UpdateOrCreateNameValueElementsInTag(manifestDoc, nodePath, "meta-data", "name", "com.unity.xr.oculus.LateLatching", "value", lateLatchingVal);

            var lateLatchingDebugVal = ((settings != null) && settings.LateLatchingDebug) ? "true" : "false";
            UpdateOrCreateNameValueElementsInTag(manifestDoc, nodePath, "meta-data", "name", "com.unity.xr.oculus.LateLatchingDebug", "value", lateLatchingDebugVal);

            nodePath = "/manifest/application";
            UpdateOrCreateAttributeInTag(manifestDoc, nodePath, "activity", "screenOrientation", "landscape");

            var styleTheme = string.Empty;
#if UNITY_2023_1_OR_NEWER
            switch (PlayerSettings.Android.applicationEntry)
            {
                case AndroidApplicationEntry.GameActivity: 
                    styleTheme = "@style/BaseUnityGameActivityTheme"; 
                    break;

                case AndroidApplicationEntry.Activity:
                    styleTheme = "@style/UnityThemeSelector";
                    break;
            }
#else
            styleTheme = "@android:style/Theme.Black.NoTitleBar.Fullscreen";
#endif
            UpdateOrCreateAttributeInTag(manifestDoc, nodePath, "activity", "theme", styleTheme);

            var configChangesValue = "keyboard|keyboardHidden|navigation|orientation|screenLayout|screenSize|uiMode";
            configChangesValue = ((sdkVersion >= 24) ? configChangesValue + "|density" : configChangesValue);
            UpdateOrCreateAttributeInTag(manifestDoc, nodePath, "activity", "configChanges", configChangesValue);

            if (sdkVersion >= 24)
            {
                UpdateOrCreateAttributeInTag(manifestDoc, nodePath, "activity", "resizeableActivity", "false");
            }

            UpdateOrCreateAttributeInTag(manifestDoc, nodePath, "activity", "launchMode", "singleTask");

            nodePath = "/manifest";
            CreateNameValueElementsInTag(manifestDoc, nodePath, "uses-feature", "name", "android.hardware.vr.headtracking", "required", "true", "version", "1");

            var eyeTrackedFoveatedRendering = ((settings != null) && (settings.FoveatedRenderingMethod == OculusSettings.FoveationMethod.EyeTrackedFoveatedRendering));
            if (eyeTrackedFoveatedRendering)
            {
                CreateNameValueElementsInTag(manifestDoc, nodePath, "uses-feature", "name", "oculus.software.eye_tracking", "required", "false");
                CreateNameValueElementsInTag(manifestDoc, nodePath, "uses-permission", "name", "com.oculus.permission.EYE_TRACKING");
            }

            string supportedDevices = null;

            if (settings != null)
            {
                var deviceList = new List<string>();

                if (settings.TargetQuest2)
                    deviceList.Add("quest2");

                if (settings.TargetQuestPro)
                    deviceList.Add("cambria");

                if (settings.TargetQuest3)
                    deviceList.Add("eureka");

                if (deviceList.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string delim = "";

                    foreach (string device in deviceList)
                    {
                        sb.Append(delim);
                        sb.Append(device);
                        delim = "|";
                    }

                    supportedDevices = sb.ToString();
                }
                else
                {
                    Debug.LogWarning("No target devices selected in Oculus Android project settings. No devices will be listed as supported in the application Android manifest.");
                }
            }
            else
            {
                supportedDevices = "quest2";
            }

            if (supportedDevices != null)
            {
                nodePath = "/manifest/application";
                UpdateOrCreateNameValueElementsInTag(manifestDoc, nodePath, "meta-data", "name", "com.oculus.supportedDevices", "value", supportedDevices);
            }

            if (settings != null && settings.SystemSplashScreen != null)
            {
                string splashScreenAssetPath = AssetDatabase.GetAssetPath(settings.SystemSplashScreen);
                if (Path.GetExtension(splashScreenAssetPath).ToLower() != ".png")
                {
                    throw new BuildFailedException("Invalid file format of System Splash Screen. It has to be a PNG file to be used by the Quest OS. The asset path: " + splashScreenAssetPath);
                }
                else
                {
                    string sourcePath = splashScreenAssetPath;
                    string targetFolder = Path.Combine(path, "src/main/assets");
                    string targetPath = targetFolder + "/vr_splash.png";

                    // copy the splash over into the gradle folder and make sure it's not read only
                    FileUtil.ReplaceFile(sourcePath, targetPath);
                    FileInfo targetInfo = new FileInfo(targetPath);
                    targetInfo.IsReadOnly = false;
                }

                nodePath = "/manifest/application";
                UpdateOrCreateNameValueElementsInTag(manifestDoc, nodePath, "meta-data", "name", "com.oculus.ossplash", "value", "true");
            }

            nodePath = "/manifest/application/activity";
            UpdateOrCreateNameValueElementsInTag(manifestDoc, nodePath, "meta-data", "name", "com.oculus.vr.focusaware", "value", "true");

            nodePath = "/manifest/application/activity/intent-filter";
            CreateNameValueElementsInTag(manifestDoc, nodePath, "category", "name", "com.oculus.intent.category.VR");

            // if the Microphone class is used in a project, the BLUETOOTH permission is automatically added to the manifest
            // we remove it here since it will cause projects to fail Oculus cert
            // this shouldn't affect Bluetooth HID devices, which don't need the permission
            nodePath = "/manifest";
            RemoveNameValueElementInTag(manifestDoc, nodePath, "uses-permission", "android:name", "android.permission.BLUETOOTH");

            manifestDoc.Save(manifestPath);
        }

        public int callbackOrder { get { return 10000; } }

        void DebugPrint(XmlDocument doc)
        {
            var sw = new System.IO.StringWriter();
            var xw = XmlWriter.Create(sw);
            doc.Save(xw);
            Debug.Log(sw);
        }
    }
#endif
}
