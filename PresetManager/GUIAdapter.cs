using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PresetManager
{
    public abstract class GUIAdapter
    {
        public static implicit operator GUIAdapter(FrameworkElement data) => new GUIAdapters.WPFAdapter(data);

        public abstract void attachEventHandler(string name, Action<string> action);

        public abstract string getAsString(string name);
        public abstract Int64? getAsInteger(string name);
        public abstract double? getAsDouble(string name);
        public abstract float? getAsFloat(string name);
        public abstract bool? getAsBool(string name);


        public abstract void writeString(string name,string data);
        public abstract void writeInteger(string name,Int64 data);
        public abstract void writeDouble(string name, double data);
        public abstract void writeFloat(string name, float data);
        public abstract void writeBool(string name, bool data);
    }
}
