﻿using Clock_ScreenSaver.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Clock_ScreenSaver.Views
{

    /// <summary>
    /// Interaktionslogik für ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        private ConfigWindowViewModel configWindowViewModel;

        /// <summary>
        /// Ctor.
        /// </summary>
        public ConfigWindow()
        {
            InitializeComponent();
            configWindowViewModel = new ConfigWindowViewModel();
            DataContext = configWindowViewModel;
        }
    }
}
