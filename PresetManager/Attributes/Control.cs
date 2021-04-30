using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection ;

namespace PresetManager.Attributes
{
    [AttributeUsage(AttributeTargets.Field,AllowMultiple =true,Inherited =true)]
    public class Control : Attribute
    {

        string _sourceelement = "";

        public Control(string sourceElement)
        {

            _sourceelement = sourceElement;
        }

        public string getSourceElement()
        {
            return _sourceelement;
        }
    }
}
