using System;
using Microsoft.Win32;

namespace Clock_ScreenSaver.Models.LogicModel
{

    /// <summary>
    /// Reads and writes into the user's registry every changes.
    /// This handler for general use and is also used for
    /// config changes.
    /// </summary>
    public class RegistryHandler
    {

        // Defines some static string varies for registry keys. SCR shorts
        // Screensaver.
        private const string STRING_REGISTRY_USER_SCREENSAVER_PATH = "Control Panel\\Desktop";
        private const string SCR_ACTIVE = "ScreenSaveActive";
        private const string SCR_LOCKSCREEN_IS_ACTIVE = "ScreenSaverIsSecure";
        private const string SCR_TIME_OUT = "ScreenSaveTimeOut";
        private const string SCR_FULL_PATH = "SCRNSAVE.EXE";

        private RegistryKey screensaverRegistryKey;

        public RegistryHandler()
        {

            // ToDo refactor.
            screensaverRegistryKey = Registry.CurrentUser.OpenSubKey(STRING_REGISTRY_USER_SCREENSAVER_PATH, true);
        }

        /// <summary>
        /// Saves changes to screensaver keys in user's registry.
        /// </summary>
        /// <returns>bool</returns>
        public bool SaveSettings(bool active, bool isSecure, int intTimeOut, string scrFullPath)
        {

            // Tries to save values.
            try
            {

                intTimeOut *= 60;

                // Sets each value for subkey.
                screensaverRegistryKey.SetValue(SCR_ACTIVE, active ? 1 : 0, RegistryValueKind.DWord);
                screensaverRegistryKey.SetValue(SCR_LOCKSCREEN_IS_ACTIVE, isSecure ? 1 : 0, RegistryValueKind.DWord);
                screensaverRegistryKey.SetValue(SCR_TIME_OUT, intTimeOut, RegistryValueKind.DWord);

                // Writes last value, if this is not null or empty.
                if(!string.IsNullOrEmpty(scrFullPath))
                {
                    screensaverRegistryKey.SetValue(SCR_FULL_PATH, scrFullPath, RegistryValueKind.String);
                }

                // Flushes and closes registry.
                screensaverRegistryKey.Flush();
            }

            // Throw exception if an exception is caught.
            catch(Exception e)
            {
                throw new Exception("RegistryKey not saved.", e);
            }

            return true;
        }

        /// <summary>
        /// Reads screensaver settings from user's registry.
        /// </summary>
        /// <returns>string[]</returns>
        public string[] ReadSettings()
        {
            string[] stringSettingsArray = new string[3];
            stringSettingsArray[0] = null;
            stringSettingsArray[1] = null;
            stringSettingsArray[2] = null;

            // Tries to read settings from user's registry.
            try
            {
                stringSettingsArray[0] = Read(SCR_ACTIVE);
                stringSettingsArray[1] = Read(SCR_LOCKSCREEN_IS_ACTIVE);
                stringSettingsArray[2] = Read(SCR_TIME_OUT);

                foreach(string setting in stringSettingsArray)
                {
                    if(setting == null)
                    {
                        throw new NullReferenceException();
                    }
                }
            }

            // If this fails an exception is thrown.
            catch(Exception e)
            {
                throw new Exception("NullReferenceException happened", e);
            }
            
            return stringSettingsArray;
        }

        /// <summary>
        /// Reads only one value as string and return as string this value.
        /// </summary>
        /// <param name="keyName">string</param>
        /// <returns>string</returns>
        private string Read(string keyName)
        {
            var readValue = screensaverRegistryKey?.GetValue(keyName)?.ToString() ?? null;

            return readValue;
        }
    }
}
