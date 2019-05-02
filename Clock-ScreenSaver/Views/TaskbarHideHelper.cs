
using Clock_ScreenSaver.Models.LogicModel;

namespace Clock_ScreenSaver.Views
{

    /// <summary>
    /// This helper gets the handle of taskbar to command via wn32API either
    /// show or hide the taskbar.
    /// </summary>
    public class TaskbarHideHelper
    {

        /// <summary>
        /// Gets hadnle of the taskbar window.
        /// </summary>
        protected static int Handle
        {
            get
            {
                return Win32API.FindWindow("Shell_TrayWnd", "");
            }
        }

        /// <summary>
        /// Hides ctor.
        /// </summary>
        private TaskbarHideHelper() {}

        /// <summary>
        /// Shows Taskbar.
        /// </summary>
        public static void Show()
        {
           Win32API.ShowWindow(Handle, 1);
        }

        /// <summary>
        /// Hides Taskbar.
        /// </summary>
        public static void Hide()
        {
            Win32API.ShowWindow(Handle, 0);
        }
    }
}