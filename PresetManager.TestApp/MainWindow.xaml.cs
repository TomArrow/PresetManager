﻿using System;
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

namespace PresetManager.TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            TestSettings testSettings = new TestSettings();
            testSettings.Bind(this);
            //PresetMan.Register(testSettings, this);
            testSettings.saveToConfig();
            testSettings.readFromGUI();
        }

        private void test()
        {

        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
