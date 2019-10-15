using System.Runtime.InteropServices;
using UnityEngine;

namespace Unity.XR.Oculus
{
    public static class Utils
    {
        public static void SetColorScaleAndOffset(Vector4 colorScale, Vector4 colorOffset)
        {
            SetColorScale(colorScale.x, colorScale.y, colorScale.z, colorScale.w);
            SetColorOffset(colorOffset.x, colorOffset.y, colorOffset.z, colorOffset.w);
        }

       [DllImport("XRSDKOculus", CharSet=CharSet.Auto)]
        static extern void SetColorScale(float x, float y, float z, float w);

        
        [DllImport("XRSDKOculus", CharSet=CharSet.Auto)]
        static extern void SetColorOffset(float x, float y, float z, float w);
    }

}