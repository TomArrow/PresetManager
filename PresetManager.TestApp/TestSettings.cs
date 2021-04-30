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
        public bool testValueBinary = false;

        [Control("textbox1")]
        public int testValueNumber = 2;

        public TestRadioValue radioValue = TestRadioValue.value1;

        public enum TestRadioValue
        {
            [Control("radio1")]
            value1,
            [Control("radio2")]
            value2
        }
    }
}
