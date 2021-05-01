using Salaros.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresetManager.ConfigAdapters
{
    class ConfigParserAdapter : ConfigAdapter
    {

        string _rootFolder;
        ConfigParser _configParser;
        string _currentConfig = "default";
        public ConfigParserAdapter(string configFolder,bool loadDefault = true)
        {
            Directory.CreateDirectory(configFolder); // Create config folder and all folders in between unless already existing.
            _rootFolder = Path.GetFullPath(configFolder).Trim(Path.DirectorySeparatorChar);
            if (loadDefault)
            {

                _currentConfig = "default";
                _configParser = new ConfigParser(_rootFolder+Path.DirectorySeparatorChar+_currentConfig + ".ini");
            }
        }


        public override string[] getAvailableConfigs()
        {
            List<string> retVal = new List<string>();
            string[] iniFiles = Directory.GetFiles(_rootFolder, "*.ini");
            bool defaultFound = false;
            foreach(string iniFile in iniFiles)
            {
                string configName = Path.GetFileNameWithoutExtension(iniFile);
                if (configName == "default")
                {
                    defaultFound = true;
                }
                retVal.Add(configName);
            }
            if (!defaultFound)
            {
                retVal.Add("default");
            }
            retVal.Sort();
            return retVal.ToArray();
        }

        public override string getCurrentConfig()
        {
            return _currentConfig;
        }

        public override void loadCurrentConfig()
        {
            _configParser = new ConfigParser(_rootFolder+Path.DirectorySeparatorChar+ _currentConfig+".ini");
        }

        public override void saveCurrentConfig()
        {
            _configParser.Save(_rootFolder + Path.DirectorySeparatorChar + _currentConfig + ".ini");
        }

        /// <summary>
        /// Sets the name of the current config. This is used for loading and saving the config file. However this function in and of itself does not load or save a config file.
        /// </summary>
        /// <param name="configName"></param>
        public override void setCurrentConfig(string configName)
        {
            _currentConfig = configName;
        }

        public override bool? getAsBool(string name, string section = null)
        {
            string value = _configParser.GetValue(section == null ? "master" : section, name);
            if(value== null)
            {
                return null;
            } else
            {
                bool retVal;
                bool success = bool.TryParse(value, out retVal);
                if (success)
                {
                    return retVal;
                } else
                {
                    return null;
                }
            }
        }

        public override double? getAsDouble(string name, string section = null)
        {
            string value = _configParser.GetValue(section == null ? "master" : section, name);
            if (value == null)
            {
                return null;
            }
            else
            {
                double retVal;
                bool success = double.TryParse(value, out retVal);
                if (success)
                {
                    return retVal;
                }
                else
                {
                    return null;
                }
            }
        }

        public override float? getAsFloat(string name, string section = null)
        {
            string value = _configParser.GetValue(section == null ? "master" : section, name);
            if (value == null)
            {
                return null;
            }
            else
            {
                float retVal;
                bool success = float.TryParse(value, out retVal);
                if (success)
                {
                    return retVal;
                }
                else
                {
                    return null;
                }
            }
        }

        public override long? getAsInteger(string name, string section = null)
        {
            string value = _configParser.GetValue(section == null ? "master" : section, name);
            if (value == null)
            {
                return null;
            }
            else
            {
                long retVal;
                bool success = long.TryParse(value, out retVal);
                if (success)
                {
                    return retVal;
                }
                else
                {
                    return null;
                }
            }
        }

        public override string getAsString(string name, string section = null)
        {
            return _configParser.GetValue(section == null ? "master" : section, name);
        }


        public override void writeBool(string name, bool data, string section = null)
        {
            _configParser.SetValue(section == null ? "master" : section, name,data);
        }

        public override void writeDouble(string name, double data, string section = null)
        {
            _configParser.SetValue(section == null ? "master" : section, name, data);
        }

        public override void writeFloat(string name, float data, string section = null)
        {
            _configParser.SetValue(section == null ? "master" : section, name, data);
        }

        public override void writeInteger(string name, long data, string section = null)
        {
            _configParser.SetValue(section == null ? "master" : section, name, data);
        }

        public override void writeString(string name, string data, string section = null)
        {
            _configParser.SetValue(section == null ? "master" : section, name, data);
        }
    }
}
