using Clock_ScreenSaver.Models.LogicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Clock_ScreenSaver.ViewModels
{

    /// <summary>
    /// Interaction logic for ClockWindow View.
    /// </summary>
    public class ClockWindowViewModel : INotifyPropertyChanged
    {
        private ClockTimer clockTimer;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Ctor.
        /// </summary>
        public ClockWindowViewModel()
        {
            InitMembers();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPrevieMode { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public string ClockTime
        {
            private set { }
            get
            {
                return clockTimer.Time;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ClockDate
        {
            private set { }
            get
            {
                return clockTimer.Date;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitMembers()
        {
            clockTimer = new ClockTimer();
            clockTimer.PropertyChanged += UpdateClockWindow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateClockWindow(object sender = null, PropertyChangedEventArgs e = null)
        {
            OnPropertyChanged("ClockTime");
            OnPropertyChanged("ClockDate");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clockView"></param>
        internal void CloseWindow(Views.ClockWindow clockView)
        {
            clockView.Close();
            Win32API.LockWorkStation();
            Environment.Exit(0);
        }

        /// <summary>
        /// Satisfies the INotifyPropertyChanged has to be refactored.
        /// </summary>
        /// <param name="propertyName"></param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.
                Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
