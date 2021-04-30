using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using PresetManager.Attributes;
using System.Windows;
using System.ComponentModel;

namespace PresetManager
{
    public class AppSettings
    {
        GUIAdapter _dataContext = null;

        string currentPreset = "";

        Dictionary<string, PropertyMapping> mappings = new Dictionary<string, PropertyMapping>();

        /// <summary>
        /// Bind this settings object to WPF controls. Supported mappings: bool to CheckBox, enum to RadioButtons, string, double, float, int to TextBox
        /// </summary>
        /// <param name="dataContext">The WPF element that contains all the controls required for binding</param>
        public void Bind(GUIAdapter dataContext)
        {
            _dataContext = dataContext;
        }

        public void SetPresetFolder()
        {

        }



        public void updateMappings()
        {

            Dictionary<string, PropertyMapping> mappingsNew = new Dictionary<string, PropertyMapping>();

            GUIAdapter dataContext = _dataContext;

            FieldInfo[] members = this.GetType().GetFields();
            foreach (FieldInfo member in members)
            {

                Type fieldType = member.FieldType;
                //MessageBox.Show(member.Name);
                Control controlInfo = (Control)member.GetCustomAttribute(typeof(Control));

                if(controlInfo == null)
                {
                    throw new Exception("Field "+member.Name+" has no Control attribute set. No binding posssible.");
                }

                // Enums get special treatment
                if(fieldType == typeof(System.Enum)){

                    //PropertyMappingEnum<fieldType> propertyMapping = new PropertyMappingEnum<fieldType>();

                    object propertyMapping = Activator.CreateInstance(typeof(PropertyMappingEnum<>).MakeGenericType(fieldType));
                    MemberInfo[] enumMembers = member.FieldType.GetMembers();
                    foreach(MemberInfo enumMember in enumMembers)
                    {
                        propertyMapping.
                    }

                } else
                {
                    
                    mappingsNew.Add(member.Name,new PropertyMapping(controlInfo.getSourceElement()));
                }

                /*foreach (Control controlInfo in controlInfos)
                {
                    
                    FrameworkElement dataElement = (FrameworkElement)dataContext.FindName(controlInfo.getSourceElement());
                    MessageBox.Show(dataElement.ToString());

                    object converted = "";
                    switch (dataElement.GetType().ToString())
                    {
                        case "System.Windows.Controls.TextBox":
                            TypeDescriptor.GetConverter(dataElement).ConvertTo(((System.Windows.Controls.TextBox)dataElement).Text, member.FieldType);
                            break;
                        case "System.Windows.Controls.CheckBox":
                            converted = ((System.Windows.Controls.CheckBox)dataElement).IsChecked;
                            //TypeDescriptor.GetConverter(dataElement).ConvertTo(((System.Windows.Controls.CheckBox)dataElement).IsChecked, member.FieldType);
                            break;
                        default:
                            break;
                    }

                    MessageBox.Show(converted.ToString());
                }*/
            }
        }


        /*public string[] enumeratePresets()
        {

        }*/

        public void setCurrentPreset(string preset)
        {

        }

        public void saveToPreset(string preset = null)
        {

        }

        public void loadFromPreset(string preset= null)
        {

        }


        public void sendToGUI()
        {

        }

        public void readFromGUI()
        {
            if(_dataContext == null)
            {
                throw new Exception("Cannot read from GUI unless dataContext has been set using Bind()");
            }

            GUIAdapter dataContext = _dataContext;

            FieldInfo[] members = this.GetType().GetFields();
            foreach (FieldInfo member in members)
            {

                MessageBox.Show(member.Name);
                Attribute[] controlInfos = member.GetCustomAttributes(typeof(Control)).ToArray();



                foreach (Control controlInfo in controlInfos)
                {
                    /*
                    FrameworkElement dataElement = (FrameworkElement)dataContext.FindName(controlInfo.getSourceElement());
                    MessageBox.Show(dataElement.ToString());

                    object converted = "";
                    switch (dataElement.GetType().ToString())
                    {
                        case "System.Windows.Controls.TextBox":
                            TypeDescriptor.GetConverter(dataElement).ConvertTo(((System.Windows.Controls.TextBox)dataElement).Text, member.FieldType);
                            break;
                        case "System.Windows.Controls.CheckBox":
                            converted = ((System.Windows.Controls.CheckBox)dataElement).IsChecked;
                            //TypeDescriptor.GetConverter(dataElement).ConvertTo(((System.Windows.Controls.CheckBox)dataElement).IsChecked, member.FieldType);
                            break;
                        default:
                            break;
                    }

                    MessageBox.Show(converted.ToString());*/
                }
            }
        }

        public void saveToConfig()
        {

        }
        public void readFromConfig()
        {

        }




        class PropertyMapping
        {

            public string mappedName;
            public PropertyMapping( string mappedNameA)
            {
                mappedName = mappedNameA;
            }
            public PropertyMapping( )
            {
            }
        }

        class PropertyMappingEnum<T> : PropertyMapping
        {

            public Dictionary<string, T> mappedNames = new Dictionary<string, T>();


        }
    }
}
