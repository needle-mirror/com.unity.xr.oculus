using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.XR.Oculus
{
    public static partial class Utils
    {
        /// <summary>
        /// Returns the type of the current headset
        /// </summary>
        public static SystemHeadset GetSystemHeadsetType()
        {
            return NativeMethods.GetSystemHeadsetType();
        }
    }
}
