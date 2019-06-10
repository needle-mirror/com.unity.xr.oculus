using System;

using UnityEngine;
using UnityEngine.XR.Management;

namespace Unity.XR.Oculus
{
    [System.Serializable]
    [XRConfigurationData("Oculus", "Unity.XR.Oculus.Settings")]
    public class OculusSettings : ScriptableObject
    {
        public enum StereoRenderingMode
        {
            MultiPass = 0,
            SinglePassInstanced
        }

        [SerializeField, Tooltip("Set the Stereo Rendering Method")]
        public StereoRenderingMode StereoRenderingModeDesktop;

        [SerializeField, Tooltip("Set the Stereo Rendering Method")]
        public StereoRenderingMode StereoRenderingModeAndroid;

        [SerializeField, Tooltip("Enable a shared depth buffer")]
        public bool SharedDepthBuffer;

        [SerializeField, Tooltip("Enable Oculus Dash Support")]
        public bool DashSupport;


        public ushort GetStereoRenderingMode()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return (ushort)StereoRenderingModeAndroid;
# else
            return (ushort)StereoRenderingModeDesktop;
#endif
        }
#if !UNITY_EDITOR
		public static OculusSettings s_Settings;

		public void Awake()
		{
			s_Settings = this;
		}
#endif

    }
}
