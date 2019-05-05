using Clock_ScreenSaver.Models.LogicModel;
using Clock_ScreenSaver.ViewModels;
using System;
using System.Windows;
using System.Windows.Interop;

namespace Clock_ScreenSaver.Views
{
    /// <summary>
    /// Interaktionslogik für PreviewWindow.xaml
    /// </summary>
    public partial class PreviewWindow : Window
    {

        /// <summary>
        /// Ctor for a preview of this screensaver.
        /// </summary>
        /// <param name="displayWidth"></param>
        /// <param name="displayHeight"></param>
        /// <param name="parentWindowHandle"></param>
        public PreviewWindow(int displayWidth,
            int displayHeight,
            IntPtr parentWindowHandle)
        {
            
            // Initializes the windows as view.
            InitializeComponent();
            previewWindowsViewModel =
                new PreviewWindowViewModel();
            DataContext = previewWindowsViewModel;

            Width = displayWidth;
            Height = displayHeight;
            Left = 10;
            Top = 10;

            // Gets windows handle of this object and sets parent window.
            IntPtr thisWindowHandle = new WindowInteropHelper(this).Handle;
            Win32API.SetParent(thisWindowHandle, parentWindowHandle);

            // Make this a child window, so when the select screensaver.
            // Dialog closes, this will also close.
            Win32API.SetWindowLong(thisWindowHandle,
                -16,
                new IntPtr(Win32API.
                GetWindowLong(thisWindowHandle, -16) | 0x40000000));
        }

        private PreviewWindowViewModel previewWindowsViewModel;
    }
}
