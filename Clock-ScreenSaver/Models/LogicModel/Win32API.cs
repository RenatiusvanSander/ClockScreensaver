using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Clock_ScreenSaver.Models.DataModel;

namespace Clock_ScreenSaver.Models.LogicModel
{

    /// <summary>
    /// Win32API communicates with user´32.dll and pinvokes its methods.
    /// </summary>
    public class Win32API
    {

        // Gets handle for a window for example taskbar handle.
        [DllImport("user32.dll")]
        public static extern int FindWindow(string className, string windowText);

        // Enables or disables a window via win32api.
        [DllImport("user32.dll")]
        public static extern int ShowWindow(int hwnd, int command);

        // Gets the previw window rect size.
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetClientRect(IntPtr hWnd, ref RECT lpRect);

        // Gives the programme ability to lock the workstation.
        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();

        // Signatures for unmanaged calls
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SystemParametersInfo(
           int uAction, int uParam, ref int lpvParam,
           int flags);

        // starts a SystemParametersInfo and receives a wished Parameter.
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SystemParametersInfo(
           int uAction, int uParam, ref bool lpvParam,
           int flags);

        // Gets a post message.
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int PostMessage(IntPtr hWnd,
           int wMsg, int wParam, int lParam);

        // Receives IntPtr for an open desktop.
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr OpenDesktop(
           string hDesktop, int Flags, bool Inherit,
           uint DesiredAccess);

        // Closes desktop.
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool CloseDesktop(
           IntPtr hDesktop);

        // Return number of windows process.
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool EnumDesktopWindows(
           IntPtr hDesktop, EnumDesktopWindowsProc callback,
           IntPtr lParam);

        // Actual window is visible.
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool IsWindowVisible(
           IntPtr hWnd);

        // Gets IntPtr of ForegroundWindow.
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetForegroundWindow();

        // Callbacks
        private delegate bool EnumDesktopWindowsProc(
           IntPtr hDesktop, IntPtr lParam);

        // Constants
        private const int SPI_SETSCREENSAVERACTIVE = 17;
        private const int SPI_SETSCREENSAVERTIMEOUT = 15;
        private const int SPIF_SENDWININICHANGE = 2;
        private const int SPI_SETSCREENSAVESECURE = 119;

        private const uint SPI_GETSCREENSAVESECURE = 0x76;
        private const uint DESKTOP_WRITEOBJECTS = 0x0080;
        private const uint DESKTOP_READOBJECTS = 0x0001;
        private const int WM_CLOSE = 16;

        /// <summary>
        /// Pass in TRUE(1) to activate or FALSE(0) to deactivate
        /// the screen saver.
        /// </summary>
        /// <param name="Active">int</param>
        public static void SetScreenSaverActive(int Active)
        {
            int nullVar = 0;

            SystemParametersInfo(SPI_SETSCREENSAVERACTIVE,
               Active, ref nullVar, SPIF_SENDWININICHANGE);
        }

        /// <summary>
        /// Pass in the number of seconds to set the screen saver
        /// timeout value.
        /// </summary>
        /// <param name="Value">Int32</param>
        public static void SetScreenSaverTimeout(Int32 Value)
        {
            int nullVar = 0;

            SystemParametersInfo(SPI_SETSCREENSAVERTIMEOUT,
               Value, ref nullVar, SPIF_SENDWININICHANGE);
        }

        /// <summary>
        /// Passes 1 for start lockscreen and 0 for do not lock Windows.
        /// </summary>
        /// <param name="Value">Int32</param>
        public static void SetScreenSaverSecure(Int32 Value)
        {
            int nullVar = 0;

            SystemParametersInfo(SPI_SETSCREENSAVESECURE,
               Value, ref nullVar, SPIF_SENDWININICHANGE);
        }

        // From Microsoft's Knowledge Base article #140723: 
        // http://support.microsoft.com/kb/140723
        // "How to force a screen saver to close once started 
        // in Windows NT, Windows 2000, and Windows Server 2003"
        public static void KillScreenSaver()
        {
            IntPtr hDesktop = OpenDesktop("Screen-saver", 0,
               false, DESKTOP_READOBJECTS | DESKTOP_WRITEOBJECTS);
            if (hDesktop != IntPtr.Zero)
            {
                EnumDesktopWindows(hDesktop, new
                   EnumDesktopWindowsProc(KillScreenSaverFunc),
                   IntPtr.Zero);
                CloseDesktop(hDesktop);
            }
            else
            {
                PostMessage(GetForegroundWindow(), WM_CLOSE,
                   0, 0);
            }
        }

        /// <summary>
        /// Ends the screensaver function.
        /// </summary>
        /// <param name="hWnd">IntPtr</param>
        /// <param name="lParam">IntPtr</param>
        /// <returns></returns>
        private static bool KillScreenSaverFunc(IntPtr hWnd, IntPtr lParam)
        {
            if (IsWindowVisible(hWnd))
                PostMessage(hWnd, WM_CLOSE, 0, 0);
            return true;
        }
    }
}
