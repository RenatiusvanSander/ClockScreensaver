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

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SystemParametersInfo(
           int uAction, int uParam, ref bool lpvParam,
           int flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int PostMessage(IntPtr hWnd,
           int wMsg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr OpenDesktop(
           string hDesktop, int Flags, bool Inherit,
           uint DesiredAccess);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool CloseDesktop(
           IntPtr hDesktop);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool EnumDesktopWindows(
           IntPtr hDesktop, EnumDesktopWindowsProc callback,
           IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool IsWindowVisible(
           IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetForegroundWindow();

        // Callbacks
        private delegate bool EnumDesktopWindowsProc(
           IntPtr hDesktop, IntPtr lParam);

        // Constants
        private const int SPI_GETSCREENSAVERACTIVE = 16;
        private const int SPI_SETSCREENSAVERACTIVE = 17;
        private const int SPI_GETSCREENSAVERTIMEOUT = 14;
        private const int SPI_SETSCREENSAVERTIMEOUT = 15;
        private const int SPI_GETSCREENSAVERRUNNING = 114;
        private const int SPIF_SENDWININICHANGE = 2;
        private const int SPI_SETSCREENSAVESECURE = 119;

        private const uint SPI_GETSCREENSAVESECURE = 0x76;
        private const uint DESKTOP_WRITEOBJECTS = 0x0080;
        private const uint DESKTOP_READOBJECTS = 0x0001;
        private const int WM_CLOSE = 16;
        
        /// <summary>
        /// Returns TRUE if the screen saver is active 
        /// (enabled, but not necessarily running).
        /// </summary>
        /// <returns>bool</returns>        
        public static bool GetScreenSaverActive()
        {
            bool isActive = false;

            SystemParametersInfo(SPI_GETSCREENSAVERACTIVE, 0,
               ref isActive, 0);
            return isActive;
        }

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
        /// Returns the screen saver timeout setting, in seconds.
        /// </summary>
        /// <returns>Int32</returns>
        public static Int32 GetScreenSaverTimeout()
        {
            Int32 value = 0;

            SystemParametersInfo(SPI_GETSCREENSAVERTIMEOUT, 0,
               ref value, 0);
            return value;
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

        /// <summary>
        /// Returns TRUE if the screen saver is actually running.
        /// </summary>
        /// <returns></returns>
        public static bool GetScreenSaverRunning()
        {
            bool isRunning = false;

            SystemParametersInfo(SPI_GETSCREENSAVERRUNNING, 0,
               ref isRunning, 0);
            return isRunning;
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
