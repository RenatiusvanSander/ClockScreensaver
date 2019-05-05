using Clock_ScreenSaver.Views;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

// Here you will find Win32API.
// Here you will find RECT.
// Here you will find WindowsStyles.
using Clock_ScreenSaver.Models.LogicModel;
using Clock_ScreenSaver.Models.DataModel;
using System.IO;
using System.Reflection;
using MessageBox = System.Windows.MessageBox;

namespace Clock_ScreenSaver
{

    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : System.Windows.Application
    {

        // These are the command line arguments of windows.
        // s -> start.
        // c -> configure.
        // p -> preview.
        // i -> install screensaver.
        private const string S = "/s";
        private const string C = "/c";
        private const string P = "/p";
        private const string I = "/i";

        private const string SCR_FILE_NAME = "Clock-ScreenSaver.scr";

        // This are important varies for the preview.
        private HwndSource winWPFContent;
        private PreviewWindow previewClockWindow;

        private RegistryHandler registryHandler;

        /// <summary>
        /// Ctor.
        /// </summary>
        public App()
        {
            InitApplication();
            Startup += Application_Startup;
        }

        /// <summary>
        /// Starts the app and computes the command line arguments from windows
        /// os or exits.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">StartupEventArgs</param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string[] stringCommandLineArguments = null;

            // Closes the application, because an error happened.
            if (e.Args.Length < 1)
            {
                Environment.Exit(1);
            }

            // Compute the command line arguments, if there are some.
            else
            {
                stringCommandLineArguments = e.Args;

                // Identify and execute preview, start or config.
                switch (stringCommandLineArguments[0].ToLower().Trim().Substring(0, 2))
                {
                    // Start case of screensaver.
                    case S:
                        ShowScreenSaver();
                        break;
                    case I:
                        InstallScreensaver();
                        break;

                    // Preview case of screensaver.
                    case P:
                        PreviewScreensaver(e);
                        break;

                    // Configure case of screensaver.
                    case C:
                        ConfigWindow configWindow = new ConfigWindow();
                        configWindow.Show();
                        break;

                    // Default case of screensaver starts the screensaver.
                    default:
                        ShowScreenSaver();
                        break;
                }
            }
        }

        /// <summary>
        /// Shows the screensaver on every monitor. This is a multi monitor
        /// application.
        /// </summary>
        private void ShowScreenSaver()
        {
            ClockWindow ownerWindow = null;

            // Creates window on other screens.
            foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                ClockWindow window = new ClockWindow(screen.Bounds.Width,
                    screen.Bounds.Height);

                // Primary screen does not have WindowsStartupLocation.
                if (screen.Primary)
                {

                    // Maximizes screen.
                    window.WindowState = WindowState.Maximized;

                    ownerWindow = window;
                }
                else
                {

                    // Other screens need a WindowStartupLocation on manual.
                    window.WindowStartupLocation = WindowStartupLocation.Manual;

                    System.Drawing.Rectangle location = screen.Bounds;
                    window.Top = location.Top;
                    window.Left = location.Left - 480;
                    window.Width = location.Width;
                    window.Height = location.Height;
                }

                window.Show();
            }

            // Sets every other screen owned to prmary window.
            // It closes all windows at once.
            foreach (Window window in Current.Windows)
            {
                if (window != ownerWindow)
                {
                    window.Owner = ownerWindow;
                }
            }
        }

        /// <summary>
        /// Previews the screensaver in screen saver small window.
        /// For that the window handle is needed and set to this.
        /// </summary>
        /// <param name="e">StartupEventArgs</param>
        private void PreviewScreensaver(StartupEventArgs e)
        {

            // Gets windows' handle for screensaver preview window.
            Int32 previewInt32 = Convert.ToInt32(e.Args[1]);
            IntPtr previewHandle = new IntPtr(previewInt32);

            // Receives window size via RECT and Win32API.
            RECT lpRect = new RECT();
            Win32API.GetClientRect(previewHandle, ref lpRect);

            previewClockWindow =
                new PreviewWindow(lpRect.Right, lpRect.Bottom, previewHandle);

            HwndSourceParameters sourceParams =
                new HwndSourceParameters("sourceParams");

            // Defines source parameters.
            sourceParams.PositionX = 0;
            sourceParams.PositionY = 0;
            sourceParams.Height = lpRect.Bottom - lpRect.Top;
            sourceParams.Width = lpRect.Right - lpRect.Left;
            sourceParams.ParentWindow = previewHandle;
            sourceParams.WindowStyle = (int)(WindowStyles.WS_VISIBLE
                | WindowStyles.WS_CHILD |
                WindowStyles.WS_CLIPCHILDREN);

            // Transmits pcitures to screensaver window of windows.
            winWPFContent = new HwndSource(sourceParams);
            winWPFContent.Disposed += new EventHandler(WinWPFContent_Disposed);
            winWPFContent.RootVisual = previewClockWindow.clockBorder;

            previewClockWindow.Topmost = false;
            previewClockWindow.Show();
        }

        /// <summary>
        /// Initializes the screensaver, if there is something to initialize.
        /// </summary>
        private void InitApplication()
        {
            registryHandler = new RegistryHandler();
        }

        /// <summary>
        /// Installs the screensaver to user's appData.
        /// </summary>
        private void InstallScreensaver()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string screensaverFilePath = Path.Combine(appDataPath, SCR_FILE_NAME);
            string[] fileNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            // Try to copy screensaver file to disc.
            try
            {
                foreach (String fileName in fileNames)
                {
                    using (FileStream fileStream = File.Create(screensaverFilePath))
                    {
                        Assembly.GetExecutingAssembly().
                            GetManifestResourceStream(fileName).
                            CopyTo(fileStream);
                    }
                }
            }

            // Catch application exception.
            catch (Exception)
            {
                throw new ApplicationException();
            }

            // Installs screensaver into registry.
            registryHandler?.SaveSettings(true,
                LockScreenActive.GetLockScreenActive(),
                15,
                screensaverFilePath);

            // Now update Windows changes to screensaver.exe.
            Win32API.SetScreenSaverSecure(1);
            Win32API.SetScreenSaverActive(1);
            Win32API.SetScreenSaverTimeout(15 * 60);
        }

        // <summary>
        /// Event that triggers when parent window is disposed--used when doing
        /// screen saver preview, so that we know when to exit.  If we didn't
        /// do this, Task Manager would get a new .scr instance every time
        /// we opened Screen Saver dialog or switched dropdown to this saver.
        /// </summary>
        /// <param name = "sender" >object</ param >
        /// < param name="e">EventArgs</param>
        private void WinWPFContent_Disposed(object sender, EventArgs e)
        {
            previewClockWindow.Close();
        }
    }
}
