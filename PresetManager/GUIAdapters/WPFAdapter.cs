using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PresetManager.GUIAdapters
{
    class WPFAdapter:GUIAdapter
    {

        private FrameworkElement _rootElement;
        public WPFAdapter(FrameworkElement rootElement)
        {
            _rootElement = rootElement;
        }



        private Dictionary<string, FrameworkElement> _cachedSearches = new Dictionary<string,FrameworkElement>();

        private FrameworkElement getElementByName(string name)
        {
            if (_cachedSearches.ContainsKey(name))
            {
                return _cachedSearches[name];
            } else
            {
                FrameworkElement foundElement = (FrameworkElement)_rootElement.FindName(name);
                if(foundElement == null)
                {
                    throw new Exception("WPF element reference '"+name+"' not found.");
                } else
                {
                    _cachedSearches.Add(name, foundElement);
                    return foundElement;
                }
            }
        }

        public override string? getAsString(string name)
        {
            FrameworkElement element = getElementByName(name);
            switch (element.GetType().ToString())
            {
                case "System.Windows.Controls.TextBox":
                    return ((TextBox)element).Text;
                    break;
                default:
                    throw new Exception("getAsString not supported for "+ element.GetType().ToString());
                    break;
            }
        }

        public override int? getAsInteger(string name)
        {
            FrameworkElement element = getElementByName(name);
            switch (element.GetType().ToString())
            {
                case "System.Windows.Controls.TextBox":
                    int retVal;
                    bool success = int.TryParse(((TextBox)element).Text,out retVal);
                    return success ? retVal : null;
                    break;
                case "System.Windows.Controls.Slider":
                    return (int)((Slider)element).Value;
                    break;
                default:
                    throw new Exception("getAsInteger not supported for " + element.GetType().ToString());
                    break;
            }
        }

        public override double? getAsDouble(string name)
        {
            FrameworkElement element = getElementByName(name);
            switch (element.GetType().ToString())
            {
                case "System.Windows.Controls.TextBox":
                    double retVal;
                    bool success = double.TryParse(((TextBox)element).Text, out retVal);
                    return success ? retVal : null;
                    break;
                case "System.Windows.Controls.Slider":
                    return ((Slider)element).Value;
                    break;
                default:
                    throw new Exception("getAsDouble not supported for " + element.GetType().ToString());
                    break;
            }
        }

        public override float? getAsFloat(string name)
        {
            FrameworkElement element = getElementByName(name);
            switch (element.GetType().ToString())
            {
                case "System.Windows.Controls.TextBox":
                    float retVal;
                    bool success = float.TryParse(((TextBox)element).Text, out retVal);
                    return success ? retVal : null;
                    break;
                case "System.Windows.Controls.Slider":
                    return (float)((Slider)element).Value;
                    break;
                default:
                    throw new Exception("getAsFloat not supported for " + element.GetType().ToString());
                    break;
            }
        }

        public override bool? getAsBool(string name)
        {
            FrameworkElement element = getElementByName(name);
            switch (element.GetType().ToString())
            {
                case "System.Windows.Controls.CheckBox":
                    return ((CheckBox)element).IsChecked;
                    break;
                case "System.Windows.Controls.RadioButton":
                    return ((RadioButton)element).IsChecked;
                    break;
                default:
                    throw new Exception("getAsBool not supported for " + element.GetType().ToString());
                    break;
            }
        }

        public override void writeString(string name, string data)
        {
            FrameworkElement element = getElementByName(name);
            switch (element.GetType().ToString())
            {
                case "System.Windows.Controls.TextBox":
                    ((TextBox)element).Text = data;
                    break;
                default:
                    throw new Exception("writeString not supported for " + element.GetType().ToString());
                    break;
            }
        }

        public override void writeInteger(string name, int data)
        {
            FrameworkElement element = getElementByName(name);
            switch (element.GetType().ToString())
            {
                case "System.Windows.Controls.TextBox":
                    ((TextBox)element).Text = data.ToString();
                    break;
                case "System.Windows.Controls.Slider":
                    ((Slider)element).Value = data;
                    break;
                default:
                    throw new Exception("writeInteger not supported for " + element.GetType().ToString());
                    break;
            }
        }

        public override void writeDouble(string name, double data)
        {
            FrameworkElement element = getElementByName(name);
            switch (element.GetType().ToString())
            {
                case "System.Windows.Controls.TextBox":
                    ((TextBox)element).Text = data.ToString();
                    break;
                case "System.Windows.Controls.Slider":
                    ((Slider)element).Value = data;
                    break;
                default:
                    throw new Exception("writeDouble not supported for " + element.GetType().ToString());
                    break;
            }
        }

        public override void writeFloat(string name, float data)
        {
            FrameworkElement element = getElementByName(name);
            switch (element.GetType().ToString())
            {
                case "System.Windows.Controls.TextBox":
                    ((TextBox)element).Text = data.ToString();
                    break;
                case "System.Windows.Controls.Slider":
                    ((Slider)element).Value = data;
                    break;
                default:
                    throw new Exception("writeFloat not supported for " + element.GetType().ToString());
                    break;
            }
        }

        public override void writeBool(string name, bool data)
        {
            FrameworkElement element = getElementByName(name);
            switch (element.GetType().ToString())
            {
                case "System.Windows.Controls.CheckBox":
                    ((CheckBox)element).IsChecked = data;
                    break;
                case "System.Windows.Controls.RadioButton":
                    ((RadioButton)element).IsChecked = data;
                    break;
                default:
                    throw new Exception("writeBool not supported for " + element.GetType().ToString());
                    break;
            }
        }
    }
}
