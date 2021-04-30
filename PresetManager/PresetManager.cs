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
    public static class PresetMan
    {

        private static Dictionary<AppSettings, FrameworkElement> dataContexts = new Dictionary<AppSettings, FrameworkElement>();

        private static void handleGUIRead(object sender, EventArgs e)
        {
            FrameworkElement dataContext = dataContexts[(AppSettings)sender];

            FieldInfo[] members = ((AppSettings)sender).GetType().GetFields();
            foreach (FieldInfo member in members)
            {

                MessageBox.Show(member.Name);
                Attribute[] controlInfos = member.GetCustomAttributes(typeof(Control)).ToArray();
                foreach (Control controlInfo in controlInfos)
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
                }
            }
        }

        public static void Register(AppSettings appSettingsObject,FrameworkElement dataContext)
        {

            dataContexts.Add(appSettingsObject, dataContext);
            appSettingsObject.readFromGUIRequested += handleGUIRead;

        }
    }
}
