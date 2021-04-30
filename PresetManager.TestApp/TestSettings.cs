using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresetManager;
using PresetManager.Attributes;

namespace PresetManager.TestApp
{
    class TestSettings : AppSettings
    {
        [Control("checkbox1")]
        public bool testValueBinary = false;
    }
}
