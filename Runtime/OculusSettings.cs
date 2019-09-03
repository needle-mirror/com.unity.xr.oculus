using System;

using UnityEngine;
using UnityEngine.XR.Management;

namespace Unity.XR.Oculus
{
    [System.Serializable]
    [XRConfigurationData("Oculus", "Unity.XR.Oculus.Settings")]
    public class OculusSettings : ScriptableObject
    {
        public enum StereoRenderingModeDesktop
        {
            MultiPass = 0,
            SinglePassInstanced = 1,
        }

        public enum StereoRenderingModeAndroid
        {
            MultiPass = 0,
            Multiview = 2
        }

        [SerializeField, Tooltip("Set the Stereo Rendering Method")]
        public StereoRenderingModeDesktop m_StereoRenderingModeDesktop;

        [SerializeField, Tooltip("Set the Stereo Rendering Method")]
        public StereoRenderingModeAndroid m_StereoRenderingModeAndroid;

        [SerializeField, Tooltip("Enable a shared depth buffer")]
        public bool SharedDepthBuffer = true;

        [SerializeField, Tooltip("Enable Oculus Dash Support")]
        public bool DashSupport = true;

        [SerializeField, Tooltip("Configure Manifest for Oculus Quest")]
        public bool V2Signing = true;


        public ushort GetStereoRenderingMode()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return (ushort)m_StereoRenderingModeAndroid;
# else
            return (ushort)m_StereoRenderingModeDesktop;
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
