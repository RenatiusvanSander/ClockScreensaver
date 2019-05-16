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

        // These varies are used for the properties to store information.
        private bool isScreensaverActiveChecked;
        private bool isScreensaverScreenLockChecked;
        private string screensaverTimeOut;

        /// <summary>
        /// Sets or gets screensaver is active.
        /// </summary>
        public bool IsScreensaverActiveChecked
        {
            get
            {
                return isScreensaverActiveChecked;
            }
            set
            {

                // Value and screensaverActive are not the same means there
                // has to be done an update.
                if (isScreensaverActiveChecked != value)
                {

                    // Writes change to user's registry.
                    isScreensaverActiveChecked = value;
                    WriteConfig();
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Sets or gets the screen saver locks Windows.
        /// </summary>
        public bool IsScreensaverScreenLockChecked
        {
            get
            {
                return isScreensaverScreenLockChecked;
            }
            set
            {

                // Validates the string is numeric, one or zero.
                // If not do not update.
                if (isScreensaverScreenLockChecked != value)
                {

                    // Writes change to user's registry.
                    isScreensaverScreenLockChecked = value;
                    WriteConfig();
                    OnPropertyChanged();
                }

            }
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
                isScreensaverActiveChecked = string.IsNullOrEmpty(screensaverSettings[0]) ?
                    false : true;
                isScreensaverScreenLockChecked = string.IsNullOrEmpty(screensaverSettings[1]) ?
                    false : true;
                screensaverTimeOut = string.IsNullOrEmpty(screensaverSettings[2]) ?
                    ZERO : (Convert.ToInt32(screensaverSettings[2]) / 60).ToString();
            }

            // Initializes all properties with values.
            else
            {
                isScreensaverActiveChecked = false;
                isScreensaverScreenLockChecked = false;
                screensaverTimeOut = ZERO;
            }

            // Updates the textboxes on config view.
            OnPropertyChanged(nameof(IsScreensaverActiveChecked));
            OnPropertyChanged(nameof(IsScreensaverScreenLockChecked));
            OnPropertyChanged(nameof(ScreensaverTimeOut));
        }

        /// <summary>
        /// Writes configuration into user's registry.
        /// </summary>
        private void WriteConfig()
        {
            registryHandler.SaveSettings(isScreensaverActiveChecked ? true : false,
                isScreensaverScreenLockChecked ? true : false,
                Convert.ToInt32(screensaverTimeOut), null);
            ActivateScreensaverSettings();
        }

        /// <summary>
        /// Activate changed settings immediateletly. Windows.ini is refreshed
        /// with changed values and flushed to screensaver preview.
        /// </summary>
        private void ActivateScreensaverSettings()
        {
            Win32API.SetScreenSaverSecure(IsScreensaverScreenLockChecked ? 1 : 0);
            Win32API.SetScreenSaverActive(IsScreensaverActiveChecked ? 1 : 0);
            Win32API.SetScreenSaverTimeout(Convert.ToInt32(screensaverTimeOut) * 60);
        }
    }
}
