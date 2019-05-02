using Clock_ScreenSaver.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace Clock_ScreenSaver.Views
{

    /// <summary>
    /// Interaktionslogik für ClockWindow.xaml
    /// </summary>
    public partial class ClockWindow : Window
    {
        private ClockWindowViewModel clockWindowViewModel;

        //start off OriginalLoction with an X and Y of int.MaxValue, because
        //it is impossible for the cursor to be at that position. That way, we
        //know if this variable has been set yet.
        Point OriginalLocation = new Point(int.MaxValue, int.MaxValue);

        /// <summary>
        /// Ctor.
        /// </summary>
        public ClockWindow()
        {
            InitializeComponent();
            clockWindowViewModel = new ClockWindowViewModel();
            DataContext = clockWindowViewModel;
        }

        /// <summary>
        /// Ctor for DisplayResolution.
        /// </summary>
        public ClockWindow(int displayWidth, int displayHeight)
        {
            InitializeComponent();
            clockWindowViewModel = 
                new ClockWindowViewModel(displayWidth, displayHeight);
            DataContext = clockWindowViewModel;
        }

        /// <summary>
        /// Exits on Button Click.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void QuitBtn_Click(object sender, RoutedEventArgs e)
        {
            clockWindowViewModel.CloseWindow(this);
        }

        /// <summary>
        /// Exits on KeyDown.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">KeyEventArgs</param>
        private void ClockWindow_KeyDown(object sender,
            System.Windows.Input.KeyEventArgs e)
        {
            clockWindowViewModel.CloseWindow(this);
        }

        /// <summary>
        /// Exits on MouseMove.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MouseEventArgs</param>
        private void ClockWindow_MouseMove(object sender,
            System.Windows.Input.MouseEventArgs e)
        {

            // See if originallocation has been set.
            if (OriginalLocation.X == int.MaxValue &
                OriginalLocation.Y == int.MaxValue)
            {
                OriginalLocation = e.GetPosition((Window)sender);
            }

            // see if the mouse has moved more than 0.01 pixels.
            // in any direction. If it has, close the application.
            if (Math.Abs(e.GetPosition((Window)sender).X - OriginalLocation.X) > 0.01 |
                Math.Abs(e.GetPosition((Window)sender).Y - OriginalLocation.Y) > 0.01)
            {
                clockWindowViewModel.CloseWindow(this);
            }
        }

        /// <summary>
        /// Exits on MouseDown.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">MouseButtonEventArgs</param>
        private void ClockWindow_MouseDown(object sender,
            MouseButtonEventArgs e)
        {
            clockWindowViewModel.CloseWindow(this);
        }

        /// <summary>
        /// Unhides taskbar on window closing.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">System.ComponentModel.CancelEventArgs</param>
        private void ClockWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TaskbarHideHelper.Show();
        }

        /// <summary>
        /// Hides the taskbar after the window is loaded.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void ClockWindowLoaded(object sender, RoutedEventArgs e)
        {
            TaskbarHideHelper.Hide();
        }
    }
}
