using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresetManager
{
    abstract public class ConfigAdapter
    {

        public static implicit operator ConfigAdapter(string data) => new ConfigAdapters.ConfigParserAdapter(data);

        abstract public string[] getAvailableConfigs();
        abstract public void setCurrentConfig(string configName);
        abstract public string getCurrentConfig();
        abstract public void saveCurrentConfig();
        abstract public void loadCurrentConfig();



        public abstract string getAsString(string name,string section=null);
        public abstract Int64? getAsInteger(string name, string section = null);
        public abstract double? getAsDouble(string name, string section = null);
        public abstract float? getAsFloat(string name, string section = null);
        public abstract bool? getAsBool(string name, string section = null);


        public abstract void writeString(string name, string data, string section = null);
        public abstract void writeInteger(string name, Int64 data, string section = null);
        public abstract void writeDouble(string name, double data, string section = null);
        public abstract void writeFloat(string name, float data, string section = null);
        public abstract void writeBool(string name, bool data, string section = null);
    }
}
