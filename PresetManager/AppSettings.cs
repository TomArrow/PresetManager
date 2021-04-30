using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using PresetManager.Attributes;
using System.Windows;

namespace PresetManager
{
    public class AppSettings
    {
        public AppSettings()
        {
            MemberInfo[] members = this.GetType().GetMembers();
            MessageBox.Show(members[0].Name);
            MessageBox.Show(members[0].GetCustomAttributes(typeof(Control)).ToArray().ToString());
        }
    }
}
