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
    }
}
