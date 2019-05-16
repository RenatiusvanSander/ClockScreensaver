namespace Clock_ScreenSaver.Models.LogicModel
{

    /// <summary>
    /// Receives if the lock screen of windows is active or not.
    /// </summary>
    public static class LockScreenActive
    {
        private static RegistryHandler registryHandler = new RegistryHandler();

        /// <summary>
        /// Gets the lockscreen is active or not.
        /// </summary>
        /// <returns></returns>
        public static bool GetLockScreenActive()
        {
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
