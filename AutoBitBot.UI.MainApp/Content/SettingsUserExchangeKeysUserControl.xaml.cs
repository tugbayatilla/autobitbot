﻿using AutoBitBot.UI.Presentation;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoBitBot.UI.MainApp.Content
{
    /// <summary>
    /// Interaction logic for SettingsAppearance.xaml
    /// </summary>
    public partial class SettingsUserExchangeKeysUserControl : UserControl
    {
        public SettingsUserExchangeKeysUserControl()
        {
            InitializeComponent();

            // a simple view model for appearance configuration
            this.DataContext = new SettingsUserExchangeKeysViewModel();
        }
    }
}