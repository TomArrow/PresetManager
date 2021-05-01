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
        private GUIAdapter _dataContext = null;

        private string currentPreset = "";

        private Dictionary<string, PropertyMapping> mappings = new Dictionary<string, PropertyMapping>();

        /// <summary>
        /// Bind this settings object to WPF controls. Supported mappings: bool to CheckBox, enum to RadioButtons, string, double, float, int to TextBox
        /// </summary>
        /// <param name="dataContext">The WPF element that contains all the controls required for binding</param>
        public void Bind(GUIAdapter dataContext)
        {
            _dataContext = dataContext;
            updateMappings();
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


                // Enums get special treatment
                if(fieldType.IsSubclassOf(typeof(System.Enum))){

                   

                    PropertyMappingEnum propertyMapping = new PropertyMappingEnum();
                    propertyMapping.fieldInfo = member;
                    propertyMapping.fieldType = fieldType;
                    FieldInfo[] enumMembers = member.FieldType.GetFields();
                    int[] enumValues = (int[])member.FieldType.GetEnumValues();
                    int enumValueIndex = 0;
                    for (int i = 0; i < enumMembers.Length; i++)
                    {
                        FieldInfo enumMember = enumMembers[i];
                        if (fieldType == enumMember.FieldType) { 
                            int enumValue = enumValues[enumValueIndex++];

                            Control controlInfoHere = (Control)enumMember.GetCustomAttribute(typeof(Control));
                            if (controlInfoHere == null)
                            {
                                throw new Exception("Enum member " + enumMember.Name + " has no Control attribute set. No binding posssible.");
                            }
                            propertyMapping.mappedNames.Add(enumValue,controlInfoHere.getSourceElement());
                        }
                    }
                    mappingsNew.Add(member.Name,propertyMapping);
                } else
                {

                    if (controlInfo == null)
                    {
                        throw new Exception("Field " + member.Name + " has no Control attribute set. No binding posssible.");
                    }

                    PropertyMapping propertyMapping = new PropertyMapping(controlInfo.getSourceElement());
                    propertyMapping.fieldType = fieldType;
                    propertyMapping.fieldInfo = member;
                    mappingsNew.Add(member.Name, propertyMapping);
                }

            }
            mappings = mappingsNew;
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
            if (_dataContext == null)
            {
                throw new Exception("Cannot read from GUI unless dataContext has been set using Bind()");
            }

            foreach (KeyValuePair<string, PropertyMapping> mappingPair in mappings)
            {
                string fieldName = mappingPair.Key;

                sendSingleValueToGUI(fieldName);
            }
        }

        public void readFromGUI()
        {
            if(_dataContext == null)
            {
                throw new Exception("Cannot read from GUI unless dataContext has been set using Bind()");
            }

            foreach(KeyValuePair<string,PropertyMapping> mappingPair in mappings)
            {
                string fieldName = mappingPair.Key;

                readSingleValueFromGUI(fieldName);
            }
        }

        public void sendSingleValueToGUI(string fieldName)
        {
            PropertyMapping mapping = mappings[fieldName];

            switch (mapping.fieldType.ToString())
            {
                case "System.Int32":
                    _dataContext.writeInteger(mapping.mappedName,(int)mapping.fieldInfo.GetValue(this));
                    break;
                case "System.Single":
                    _dataContext.writeFloat(mapping.mappedName, (float)mapping.fieldInfo.GetValue(this));
                    break;
                case "System.Double":
                    _dataContext.writeDouble(mapping.mappedName, (double)mapping.fieldInfo.GetValue(this));
                    break;
                case "System.Boolean":
                    _dataContext.writeBool(mapping.mappedName, (bool)mapping.fieldInfo.GetValue(this));
                    break;
                default:
                    if (mapping.GetType() == typeof(PropertyMappingEnum) && mapping.fieldType.IsSubclassOf(typeof(System.Enum)))
                    {
                        // Radio buttons
                        int activeRadios = 0;
                        int valueToSet  = (int)mapping.fieldInfo.GetValue(this); 
                        foreach (KeyValuePair<int, string> subMapping in (mapping as PropertyMappingEnum).mappedNames)
                        {
                            if(subMapping.Key == valueToSet)
                            {
                                _dataContext.writeBool(subMapping.Value,true);
                                activeRadios++;
                            } else
                            {
                                _dataContext.writeBool(subMapping.Value, false);
                            }
                            if (activeRadios > 1)
                            {
                                throw new Exception(fieldName + ": Multiple simmultaneously active radio buttons for enum mapping are not allowed.");
                            }
                        }
                    }
                    else
                    {

                        throw new Exception(mapping.fieldType.ToString() + " not implemented for reading from GUI.");
                    }
                    break;
            }
        }

        public void readSingleValueFromGUI(string fieldName)
        {
            PropertyMapping mapping = mappings[fieldName];

            switch (mapping.fieldType.ToString())
            {
                case "System.Int32":
                    mapping.fieldInfo.SetValue(this, _dataContext.getAsInteger(mapping.mappedName));
                    break;
                case "System.Single":
                    mapping.fieldInfo.SetValue(this, _dataContext.getAsFloat(mapping.mappedName));
                    break;
                case "System.Double":
                    mapping.fieldInfo.SetValue(this, _dataContext.getAsDouble(mapping.mappedName));
                    break;
                case "System.Boolean":
                    mapping.fieldInfo.SetValue(this,_dataContext.getAsBool(mapping.mappedName));
                    break;
                default:
                    if (mapping.GetType() == typeof(PropertyMappingEnum) &&  mapping.fieldType.IsSubclassOf(typeof(System.Enum)))
                    {
                        // Radio buttons
                        int enumNumber = -1;
                        int activeRadios = 0;
                        foreach(KeyValuePair<int,string> subMapping in (mapping as PropertyMappingEnum).mappedNames)
                        {
                            if (_dataContext.getAsBool(subMapping.Value) == true)
                            {
                                enumNumber = subMapping.Key;
                                activeRadios++;
                            }
                            if(activeRadios > 1)
                            {
                                throw new Exception(fieldName+": Multiple simmultaneously active radio buttons for enum mapping are not allowed.");
                            }
                        }
                        mapping.fieldInfo.SetValue(this, enumNumber);
                    }
                    else
                    {

                        throw new Exception(mapping.fieldType.ToString() + " not implemented for reading from GUI.");
                    }
                    break;
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
            public FieldInfo fieldInfo;
            public Type fieldType;
            public string mappedName;
            public PropertyMapping( string mappedNameA)
            {
                mappedName = mappedNameA;
            }
            public PropertyMapping( )
            {
            }
        }

        class PropertyMappingEnum : PropertyMapping
        {

            public Dictionary<int, string> mappedNames = new Dictionary<int,string>();


        }
    }
}
