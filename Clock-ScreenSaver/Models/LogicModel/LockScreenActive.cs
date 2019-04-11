﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clock_ScreenSaver.Models.LogicModel
{

    /// <summary>
    /// Receives if the lock screen of windows is active or not.
    /// </summary>
    public static class LockScreenActive
    {
        private static RegistryHandler registryHandler;

        /// <summary>
        /// Gets the lockscreen is active or not.
        /// </summary>
        /// <returns></returns>
        public static bool GetLockScreenActive()
        {
            registryHandler = new RegistryHandler();
            string[] screensaverSettingsArray = registryHandler.ReadSettings();

            // Checks for null and keeps null exceptions away.
            if (screensaverSettingsArray == null || string.IsNullOrEmpty(screensaverSettingsArray[1]))
            {
                return false;
            }

            // Returns true or false.
            return screensaverSettingsArray[1].Contains("1");
        }
    }
}
