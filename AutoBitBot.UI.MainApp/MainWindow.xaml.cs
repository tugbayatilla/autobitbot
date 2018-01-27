﻿using AutoBitBot.UI.Windows.Controls;
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

namespace AutoBitBot.UI.MainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();


            
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            RichTextBox output = (RichTextBox)Template.FindName("Output", this);

            ViewModels.MainViewModel model = new ViewModels.MainViewModel(this.Dispatcher, output);
            model.Init();
            this.DataContext = model;
        }
    }
}
