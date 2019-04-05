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

namespace Clock_ScreenSaver
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private const string A = "/a";
        private const string S = "/s";
        private const string C = "/c";
        private const string P = "/p";
        private const string I = "/i";
        private HwndSource winWPFContent;
        private ClockWindow previewClockWindow;

        public App()
        {
            Startup += Application_Startup;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string[] stringCommandLineArguments = null;

            if(e.Args.Length > 0)
            {
                stringCommandLineArguments = e.Args;
            }

            if(stringCommandLineArguments?.Length > 0)
            {
                switch(stringCommandLineArguments[0].ToLower().Trim().Substring(0, 2))
                {
                    case A:
                        break;
                    case S:
                        ShowScreenSaver();
                        break;
                    case I:
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
                    default:
                        ShowScreenSaver();
                        break;
                }
            }
        }
        
        private void ShowScreenSaver()
        {
            Window ownerWindow = null;

            // Starts same clock on all monitors.
            foreach (Screen screen in Screen.AllScreens)
            {
                ClockWindow clockWindow = new ClockWindow();

                // Sets Windows Properties.
                // clockWindow.WindowState = WindowState.Maximized;
                clockWindow.WindowStyle = WindowStyle.None;
                // clockWindow.ShowInTaskbar = false;
                                
                // Creates the primary window.
                if (!screen.Primary)
                {

                    clockWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                    clockWindow.Top = screen.WorkingArea.Top;
                    clockWindow.Left = screen.WorkingArea.Left;
                }
                else
                {
                    ownerWindow = clockWindow;
                }

                clockWindow.Show();
            }
            
            foreach(Window window in Windows)
            {
                if(window.Owner != ownerWindow.Owner)
                {
                    window.Owner = ownerWindow;
                }
            }
        }

        private void PreviewScreensaver(StartupEventArgs e)
        {
            previewClockWindow = new ClockWindow();
            Int32 previewHandle = Convert.ToInt32(e.Args[1]);
            IntPtr pPreviewHnd = new IntPtr(previewHandle);

            // Receives window size of Win32API.
            RECT lpRect = new RECT();
            bool bGetRect = Win32API.GetClientRect(pPreviewHnd, ref lpRect);

            HwndSourceParameters sourceParams = new HwndSourceParameters("sourceParams");

            // Defines source parameters.
            sourceParams.PositionX = 0;
            sourceParams.PositionY = 0;
            sourceParams.Height = lpRect.Bottom - lpRect.Top;
            sourceParams.Width = lpRect.Right - lpRect.Left;
            sourceParams.ParentWindow = pPreviewHnd;
            sourceParams.WindowStyle = (int)(WindowStyles.WS_VISIBLE | WindowStyles.WS_CHILD | WindowStyles.WS_CLIPCHILDREN);

            // Transmits pcitures to screensaver window of windows.
            winWPFContent = new HwndSource(sourceParams);
            winWPFContent.Disposed += new EventHandler(winWPFContent_Disposed);
            winWPFContent.RootVisual = previewClockWindow.border1;
        }

        private void InitApplication()
        {
        }

        // <summary>
        /// Event that triggers when parent window is disposed--used when doing
        /// screen saver preview, so that we know when to exit.  If we didn't
        /// do this, Task Manager would get a new .scr instance every time
        /// we opened Screen Saver dialog or switched dropdown to this saver.
        /// </summary>
        /// <param name = "sender" ></ param >
        /// < param name="e"></param>
        void winWPFContent_Disposed(object sender, EventArgs e)
        {
            previewClockWindow.Close();
        }
    }
}
