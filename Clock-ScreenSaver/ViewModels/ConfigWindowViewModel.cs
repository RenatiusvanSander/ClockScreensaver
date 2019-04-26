using System;
using Clock_ScreenSaver.Models.LogicModel;

namespace Clock_ScreenSaver.ViewModels
{

    /// <summary>
    /// Interaction logic for ConfigWindow.
    /// </summary>
    public class ConfigWindowViewModel : NotifyPropertyChangedBase
    {

        /// <summary>
        /// Ctor.
        /// </summary>
        public ConfigWindowViewModel()
        {
            InitConfigWindowViewModel();
        }

        private RegistryHandler registryHandler;

        // Some consts which does not need to be initialized every time.
        private const string ZERO = "0";
        private const string ONE = "1";
        private const string ERROR_STRING_IS_NOT_ONE_OR_ZERO =
            "The number has to be 1 for active or 0 for disable the" +
            " screensaver.";

        // These strings are used for the properties to store information.
        private string screensaverActive;
        private string screensaverScreenLock;
        private string screensaverTimeOut;

        // Properties for the config window.

        /// <summary>
        /// Sets or gets screensaver is active.
        /// 1 means an active screensaver.
        /// </summary>
        public string ScreensaverActive
        {
            get
            {
                return screensaverActive;
            }
            set
            {

                // Value and screensaverActive are not the same means there
                // has to be done an update.
                if (!string.Equals(screensaverActive, value))
                {

                    // Validates the string is numeric, one or zero.
                    // If not do not update.
                    if (value.Length == 1 && (value.Equals(ONE) || value.Equals(ZERO)))
                    {

                        // Writes change to user's registry.
                        screensaverActive = value;
                        WriteConfig();
                        OnPropertyChanged();
                    }

                    // Throw an validation error. The textbox gets red coloured
                    // frame on view.
                    else
                    {

                        // ToDo refactor.
                        throw new ArgumentException(
                            ERROR_STRING_IS_NOT_ONE_OR_ZERO);
                    }

                }
            }
        }

        /// <summary>
        /// Sets or gets the screen saver locks Windows. 1 means locked.
        /// </summary>
        public string ScreensaverScreenLock
        {
            get
            {
                return screensaverScreenLock;
            }
            set
            {

                // Validates the string is numeric, one or zero.
                // If not do not update.
                if (!string.Equals(screensaverScreenLock, value))
                {

                    // Writes change to user's registry.
                    if (ValidateOneOrZero(value))
                    {
                        screensaverScreenLock = value;
                        WriteConfig();
                        OnPropertyChanged();
                    }

                    // Throw an validation error. The textbox gets red coloured
                    // frame on view.
                    else
                    {
                        throw new ArgumentException(
                            ERROR_STRING_IS_NOT_ONE_OR_ZERO);
                    }
                }

            }
        }

        /// <summary>
        /// Validate the textbox has got an one or zero.
        /// If not a false is returned and property is not changed.
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>bool</returns>
        private bool ValidateOneOrZero(string value)
        {
            return value.Length == 1 &&
                (value.Equals(ONE) || value.Equals(ZERO));
        }

        /// <summary>
        /// Store the screensaver time out time in minutes.
        /// RegistryHandler multiplies minutes with 60.
        /// </summary>
        public string ScreensaverTimeOut
        {
            get
            {
                return screensaverTimeOut;
            }
            set
            {
                if (!string.Equals(screensaverTimeOut, value))
                {

                    // Validates the string is numeric, one or zero.
                    // If not do not update.
                    if (int.TryParse(value, out int number))
                    {
                        screensaverTimeOut = value;
                        WriteConfig();
                        OnPropertyChanged();
                    }

                    // Throw an validation error. The textbox gets red coloured
                    // frame on view.
                    else
                    {
                        throw new ArgumentException("Is not a number.");
                    }

                }
            }
        }

        /// <summary>
        /// Initializes all members in the view model.
        /// </summary>
        private void InitConfigWindowViewModel()
        {
            registryHandler = new RegistryHandler();
            ReadConfig();
        }

        /// <summary>
        /// Reads config out of the user registry.
        /// </summary>
        private void ReadConfig()
        {
            string[] screensaverSettings = registryHandler.ReadSettings();

            // Ensures none unhandled NullException.
            if (screensaverSettings != null)
            {

                // Sets values for config view.
                screensaverActive = string.IsNullOrEmpty(screensaverSettings[0]) ?
                    ZERO : screensaverSettings[0];
                screensaverScreenLock = string.IsNullOrEmpty(screensaverSettings[1]) ?
                    ZERO : screensaverSettings[1];
                screensaverTimeOut = string.IsNullOrEmpty(screensaverSettings[2]) ?
                    ZERO : (Convert.ToInt32(screensaverSettings[2]) / 60).ToString();
            }

            // Initializes all properties with values.
            else
            {
                screensaverActive = ZERO;
                screensaverScreenLock = ZERO;
                screensaverTimeOut = ZERO;
            }

            // Updates the textboxes on config view.
            OnPropertyChanged(nameof(ScreensaverActive));
            OnPropertyChanged(nameof(ScreensaverScreenLock));
            OnPropertyChanged(nameof(ScreensaverTimeOut));
        }

        /// <summary>
        /// Write the configuration into user's registry.
        /// </summary>
        private void WriteConfig()
        {
            registryHandler.SaveSettings(screensaverActive.Contains("1") ? true : false,
                screensaverScreenLock.Contains("1") ? true : false,
                Convert.ToInt32(screensaverTimeOut), null);
            ActivateScreensaverSettings();
        }

        /// <summary>
        /// Activates changed settings immediateletly.
        /// </summary>
        private void ActivateScreensaverSettings()
        {
            Win32API.SetScreenSaverSecure(screensaverScreenLock.Contains("1") ? 1 : 0);
            Win32API.SetScreenSaverActive(screensaverActive.Contains("1") ? 1 : 0);
            Win32API.SetScreenSaverTimeout(Convert.ToInt32(screensaverTimeOut) * 60);
        }
    }
}
