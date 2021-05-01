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

        public override void attachEventHandler(string name, Action<string> action)
        {
            FrameworkElement element = getElementByName(name);
            switch (element.GetType().ToString())
            {
                case "System.Windows.Controls.TextBox":
                    TextChangedEventHandler eventHandler = (a, b) => { action(name); };
                    ((TextBox)element).TextChanged += eventHandler;
                    break;
                case "System.Windows.Controls.CheckBox":
                    RoutedEventHandler eventHandler2 = (a, b) => { action(name); };
                    ((CheckBox)element).Checked += eventHandler2;
                    ((CheckBox)element).Unchecked += eventHandler2;
                    break;
                case "System.Windows.Controls.Slider":
                    RoutedPropertyChangedEventHandler<double> eventHandler3 = (a, b) => { action(name); };
                    ((Slider)element).ValueChanged += eventHandler3;
                    break;
                case "System.Windows.Controls.RadioButton":
                    RoutedEventHandler eventHandler4 = (a, b) => { action(name); };
                    ((RadioButton)element).Checked += eventHandler4;
                    ((RadioButton)element).Unchecked += eventHandler4;
                    break;
                case "System.Windows.Controls.ListBoxItem":
                case "System.Windows.Controls.ComboBoxItem":
                    RoutedEventHandler eventHandler5 = (a, b) => { action(name); };
                    ((ListBoxItem)element).Selected += eventHandler5;
                    ((ListBoxItem)element).Unselected += eventHandler5;
                    break;
                default:
                    throw new Exception("attachEventHandler not supported for " + element.GetType().ToString());
                    break;
            }
        }

        public override string getAsString(string name)
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

        public override Int64? getAsInteger(string name)
        {
            FrameworkElement element = getElementByName(name);
            switch (element.GetType().ToString())
            {
                case "System.Windows.Controls.TextBox":
                    Int64 retVal;
                    bool success = Int64.TryParse(((TextBox)element).Text,out retVal);
                    if (success)
                    {
                        return retVal;
                    } else
                    {
                        return null;
                    }
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
                    if (success)
                    {
                        return retVal;
                    }
                    else
                    {
                        return null;
                    }
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
                    if (success)
                    {
                        return retVal;
                    }
                    else
                    {
                        return null;
                    }
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
                case "System.Windows.Controls.ListBoxItem":
                case "System.Windows.Controls.ComboBoxItem":
                    return ((ListBoxItem)element).IsSelected;
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

        public override void writeInteger(string name, Int64 data)
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
                case "System.Windows.Controls.ListBoxItem":
                case "System.Windows.Controls.ComboBoxItem": 
                    ((ListBoxItem)element).IsSelected = data;
                    break;
                default:
                    throw new Exception("writeBool not supported for " + element.GetType().ToString());
                    break;
            }
        }
    }
}
