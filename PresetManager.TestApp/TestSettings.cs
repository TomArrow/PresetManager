using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PresetManager;
using PresetManager.Attributes;

namespace PresetManager.TestApp
{
    class TestSettings : AppSettings
    {
        [Control("checkbox1")]
        public bool testValueBinary = true;

        [Control("textbox1")]
        public int testValueNumber = 0;

        [Control("sliderValue")]
        public double testValueDouble = 0;

        public TestRadioValue radioValue = TestRadioValue.value2;

        public enum TestRadioValue
        {
            [Control("radio1")]
            value1,
            [Control("radio2")]
            value2,
            [Control("radio3")]
            value3
        }
    }
}
