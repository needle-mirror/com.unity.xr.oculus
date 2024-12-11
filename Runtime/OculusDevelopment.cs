using System;
using UnityEngine;

namespace Unity.XR.Oculus
{
    /// <summary>
    /// Developer mode settings
    /// </summary>
    public static class Development
    {
        private enum UserDeveloperModeSettingCache
        {
            NoUserSettingCached = 0,
            UserSettingFalse = 1,
            UserSettingTrue = 2
        }

        private static UserDeveloperModeSettingCache s_CachedMode = UserDeveloperModeSettingCache.NoUserSettingCached;

        /// <summary>
        /// Enable or disable developer mode, default enable in development build
        /// </summary>
        /// <param name="active">true to attempt to set developer mode active</param>
        public static void TrySetDeveloperMode(bool active)
        {
            //cache this setting to use it in Start() only
            s_CachedMode = active ? UserDeveloperModeSettingCache.UserSettingTrue : UserDeveloperModeSettingCache.UserSettingFalse;
        }

        internal static void OverrideDeveloperModeStart()
        {
            bool enable = true;
            bool shouldOverride = false;
            if (s_CachedMode != UserDeveloperModeSettingCache.NoUserSettingCached)
            {
                shouldOverride = true;
                enable = (s_CachedMode == UserDeveloperModeSettingCache.UserSettingTrue);
            }
            else if (Debug.isDebugBuild)
                shouldOverride = true;

            if (shouldOverride && !NativeMethods.SetDeveloperModeStrict(enable))
                Debug.LogError("Failed to set DeveloperMode on Start.");
        }

        internal static void OverrideDeveloperModeStop()
        {
            if (!NativeMethods.SetDeveloperModeStrict(false))
                Debug.LogError("Failed to set DeveloperMode to false on Stop.");
        }
    }
}
