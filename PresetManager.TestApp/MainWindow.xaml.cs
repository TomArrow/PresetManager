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

namespace PresetManager.TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        TestSettings testSettings = new TestSettings();
        public MainWindow()
        {
            InitializeComponent();

            testSettings.Bind(this);
            testSettings.BindConfig("config");
            testSettings.attachPresetManager(presetManPanel);
            //PresetMan.Register(testSettings, this);
            testSettings.readFromGUI();
            testSettings.radioValue = TestSettings.TestRadioValue.value3;
            testSettings.testValueNumber = 500000;
            testSettings.sendToGUI();
        }

        private void test()
        {

        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(testSettings.testValueDouble.ToString() + "," + testSettings.testValueBinary + "," + testSettings.testValueNumber + "," + testSettings.radioValue);
        }


    }
}
