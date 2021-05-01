using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using PresetManager.Attributes;
using System.Windows;
using System.ComponentModel;
using wincon = System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PresetManager
{
    public class ValueUpdatedEventArgs : EventArgs
    {
        string _fieldName;
        
        public string FieldName
        {
            get
            {
                return _fieldName;
            }
        }
        public ValueUpdatedEventArgs(string fieldName)
        {
            _fieldName = fieldName;
        }
    }
    public class AppSettings
    {
        private GUIAdapter _dataContext = null;

        private ConfigAdapter _configAdapter = null;

        private Dictionary<string, PropertyMapping> mappings = new Dictionary<string, PropertyMapping>();

        public delegate void ValueUpdatedEventHandler(object sender, ValueUpdatedEventArgs e);
        public event ValueUpdatedEventHandler ValueUpdatedInGUI;

        protected virtual void OnValueUpdatedInGUI(ValueUpdatedEventArgs e)
        {
            ValueUpdatedEventHandler handler = ValueUpdatedInGUI;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Bind this settings object to WPF controls. Supported mappings: bool to CheckBox, enum to RadioButtons or ListBoxItems, string, double, float, int to TextBox
        /// </summary>
        /// <param name="dataContext">The WPF element that contains all the controls required for binding</param>
        /// <param name="autoUpdateSettingsObject">Whether to attach event handlers to automatically apply changes in the GUI to the settings object</param>
        public void Bind(GUIAdapter dataContext,bool autoUpdateSettingsObject = true)
        {
            _dataContext = dataContext;
            updateMappings(autoUpdateSettingsObject);
        }
        
        public void BindConfig(ConfigAdapter config,bool loadDefaultAutomatically = true)
        {
            _configAdapter = config;
            if (loadDefaultAutomatically)
            {
                _configAdapter.setCurrentConfig("default");
                _configAdapter.loadCurrentConfig();
                readFromConfig();
                sendToGUI();
            }
        }

        public void attachPresetManager(wincon.Panel container,int comboBoxMinWidth= 200)
        {
            if(_configAdapter == null)
            {
                throw new Exception("Cannot create a preset manager before attaching a config via BindConfig()");
            }

            wincon.GroupBox groupbox = new wincon.GroupBox();
            groupbox.Header = "Presets";
            wincon.WrapPanel groupboxInner = new wincon.WrapPanel();
            groupbox.Content = groupboxInner;

            wincon.ComboBox comboBox = new wincon.ComboBox();
            comboBox.ItemsSource = _configAdapter.getAvailableConfigs();
            comboBox.IsEditable = true;
            comboBox.MinWidth = comboBoxMinWidth;
            comboBox.SelectedValue = "default";

            wincon.Button btnLoadPreset = new wincon.Button();
            wincon.Image imgLoad = new wincon.Image();
            imgLoad.Stretch = Stretch.None;
            imgLoad.Source = new BitmapImage(new Uri("pack://application:,,,/PresetManager;component/images/Open_16x.png"));
            btnLoadPreset.Content = imgLoad;
            btnLoadPreset.Click += (a,b) => { loadFromPreset(comboBox.Text); };

            wincon.Button btnSavePreset = new wincon.Button();
            wincon.Image imgSave = new wincon.Image();
            imgSave.Stretch = Stretch.None;
            imgSave.Source = new BitmapImage(new Uri("pack://application:,,,/PresetManager;component/images/Save_16x.png"));
            btnSavePreset.Content = imgSave;
            btnSavePreset.Click += (a, b) => { saveToPreset(comboBox.Text); comboBox.ItemsSource = _configAdapter.getAvailableConfigs(); };

            wincon.Button btnRefreshPresets = new wincon.Button();
            wincon.Image imgRefresh = new wincon.Image();
            imgRefresh.Stretch = Stretch.None;
            imgRefresh.Source = new BitmapImage( new Uri("pack://application:,,,/PresetManager;component/images/Refresh_16x.png"));
            btnRefreshPresets.Content = imgRefresh;
            btnRefreshPresets.Click += (a, b) => { comboBox.ItemsSource = _configAdapter.getAvailableConfigs(); };

            container.Children.Add(groupbox);
            groupboxInner.Children.Add(btnRefreshPresets);
            groupboxInner.Children.Add(comboBox);
            groupboxInner.Children.Add(btnLoadPreset);
            groupboxInner.Children.Add(btnSavePreset);
        }




        // TODO Automatically read default values and write to default config if none exists at startup.
        public void updateMappings(bool autoUpdateSettingsObject = true)
        {

            Dictionary<string, PropertyMapping> mappingsNew = new Dictionary<string, PropertyMapping>();

            GUIAdapter dataContext = _dataContext;

            FieldInfo[] members = this.GetType().GetFields();
            foreach (FieldInfo member in members)
            {

                Type fieldType = member.FieldType;
                //MessageBox.Show(member.Name);
                Control controlInfo = (Control)member.GetCustomAttribute(typeof(Control));
                Category categoryInfo = (Category)member.GetCustomAttribute(typeof(Category));


                // Enums get special treatment
                if(fieldType.IsSubclassOf(typeof(System.Enum))){

                   

                    PropertyMappingEnum propertyMapping = new PropertyMappingEnum();
                    if(categoryInfo != null)
                    {
                        propertyMapping.categoryName = categoryInfo.getCategoryName();
                    }
                    propertyMapping.fieldInfo = member;
                    propertyMapping.fieldType = fieldType;
                    propertyMapping.defaultValue = member.GetValue(this);
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
                            

                            string localCopyOfMemberNameForLambda = member.Name;
                            _dataContext.attachEventHandler(controlInfoHere.getSourceElement(), (a) => {
                                if (autoUpdateSettingsObject)
                                {
                                    readSingleValueFromGUI(localCopyOfMemberNameForLambda);
                                }
                                OnValueUpdatedInGUI(new ValueUpdatedEventArgs(localCopyOfMemberNameForLambda));
                            });
                            
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
                    propertyMapping.defaultValue = member.GetValue(this);
                    if (categoryInfo != null)
                    {
                        propertyMapping.categoryName = categoryInfo.getCategoryName();
                    }

                    string localCopyOfMemberNameForLambda = member.Name;
                    _dataContext.attachEventHandler(controlInfo.getSourceElement(), (a) => {
                        if (autoUpdateSettingsObject)
                        {
                            readSingleValueFromGUI(localCopyOfMemberNameForLambda);
                        }
                        OnValueUpdatedInGUI(new ValueUpdatedEventArgs(localCopyOfMemberNameForLambda)); 
                    });
                    
                    mappingsNew.Add(member.Name, propertyMapping);
                }

            }
            mappings = mappingsNew;
        }


        /*public string[] enumeratePresets()
        {

        }*/


        public void saveToPreset(string presetName)
        {
            _configAdapter.setCurrentConfig(presetName);
            sendToConfig();
            _configAdapter.saveCurrentConfig();

        }

        public void loadFromPreset(string presetName,bool writeToGUIAfterLoading = true)
        {
            _configAdapter.setCurrentConfig(presetName);
            _configAdapter.loadCurrentConfig();
            readFromConfig();
            if(writeToGUIAfterLoading && _dataContext!= null)
            {
                sendToGUI();
            }
        }



        private void sendToConfig()
        {
            if (_configAdapter == null)
            {
                throw new Exception("Cannot send to config unless configAdapter has been set using BindConfig()");
            }

            foreach (KeyValuePair<string, PropertyMapping> mappingPair in mappings)
            {
                string fieldName = mappingPair.Key;

                sendSingleValueToConfig(fieldName);
            }
        }
        private void readFromConfig()
        {
            if (_configAdapter == null)
            {
                throw new Exception("Cannot read from config unless configAdapter has been set using BindConfig()");
            }

            foreach (KeyValuePair<string, PropertyMapping> mappingPair in mappings)
            {
                string fieldName = mappingPair.Key;

                readSingleValueFromConfig(fieldName);
            }
        }


        public void sendToGUI()
        {
            if (_dataContext == null)
            {
                throw new Exception("Cannot send to GUI unless dataContext has been set using Bind()");
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

        public void sendSingleValueToConfig(string fieldName)
        {
            PropertyMapping mapping = mappings[fieldName];

            switch (mapping.fieldType.ToString())
            {
                case "System.String":
                    _configAdapter.writeString(fieldName, (string)mapping.fieldInfo.GetValue(this),mapping.categoryName);
                    break;
                case "System.Int32":
                    _configAdapter.writeInteger(fieldName, (int)mapping.fieldInfo.GetValue(this), mapping.categoryName);
                    break;
                case "System.Int64":
                    _configAdapter.writeInteger(fieldName, (Int64)mapping.fieldInfo.GetValue(this), mapping.categoryName);
                    break;
                case "System.Single":
                    _configAdapter.writeFloat(fieldName, (float)mapping.fieldInfo.GetValue(this), mapping.categoryName);
                    break;
                case "System.Double":
                    _configAdapter.writeDouble(fieldName, (double)mapping.fieldInfo.GetValue(this), mapping.categoryName);
                    break;
                case "System.Boolean":
                    _configAdapter.writeBool(fieldName, (bool)mapping.fieldInfo.GetValue(this), mapping.categoryName);
                    break;
                default:
                    if (mapping.GetType() == typeof(PropertyMappingEnum) && mapping.fieldType.IsSubclassOf(typeof(System.Enum)))
                    {

                        _configAdapter.writeString(fieldName, mapping.fieldInfo.GetValue(this).ToString(), mapping.categoryName);
                    }
                    else
                    {

                        throw new Exception(mapping.fieldType.ToString() + " not implemented for writing to config.");
                    }
                    break;
            }
        }

        public void readSingleValueFromConfig(string fieldName)
        {
            PropertyMapping mapping = mappings[fieldName];

            switch (mapping.fieldType.ToString())
            {
                case "System.String":
                    string guiString = _configAdapter.getAsString(fieldName, mapping.categoryName);
                    if(guiString != null)
                    {

                        mapping.fieldInfo.SetValue(this, guiString);
                    } else if (mapping.defaultValue != null)
                    {
                        mapping.fieldInfo.SetValue(this, mapping.defaultValue);
                    }
                    break;
                case "System.Int32":
                    Int64? guiInt = _configAdapter.getAsInteger(fieldName, mapping.categoryName);
                    if (guiInt.HasValue)
                    {

                        mapping.fieldInfo.SetValue(this, (int)Math.Min(Int32.MaxValue,Math.Max(Int32.MinValue,guiInt.Value)));
                    }
                    else if (mapping.defaultValue != null)
                    {
                        mapping.fieldInfo.SetValue(this, mapping.defaultValue);
                    }
                    break;
                case "System.Int64":
                    Int64? guiInt64 = _configAdapter.getAsInteger(fieldName, mapping.categoryName);
                    if (guiInt64.HasValue)
                    {

                        mapping.fieldInfo.SetValue(this, guiInt64.Value);
                    }
                    else if (mapping.defaultValue != null)
                    {
                        mapping.fieldInfo.SetValue(this, mapping.defaultValue);
                    }
                    break;
                case "System.Single":
                    float? guiFloat = _configAdapter.getAsFloat(fieldName, mapping.categoryName);
                    if (guiFloat.HasValue)
                    {

                        mapping.fieldInfo.SetValue(this, guiFloat.Value);
                    }
                    else if (mapping.defaultValue != null)
                    {
                        mapping.fieldInfo.SetValue(this, mapping.defaultValue);
                    }
                    break;
                case "System.Double":
                    double? guiDouble = _configAdapter.getAsDouble(fieldName, mapping.categoryName);
                    if (guiDouble.HasValue)
                    {

                        mapping.fieldInfo.SetValue(this, guiDouble.Value);
                    }
                    else if (mapping.defaultValue != null)
                    {
                        mapping.fieldInfo.SetValue(this, mapping.defaultValue);
                    }
                    break;
                case "System.Boolean":
                    bool? guiBool = _configAdapter.getAsBool(fieldName, mapping.categoryName);
                    if (guiBool.HasValue)
                    {

                        mapping.fieldInfo.SetValue(this, guiBool.Value);
                    }
                    else if (mapping.defaultValue != null)
                    {
                        mapping.fieldInfo.SetValue(this, mapping.defaultValue);
                    }
                    break;
                default:
                    if (mapping.GetType() == typeof(PropertyMappingEnum) &&  mapping.fieldType.IsSubclassOf(typeof(System.Enum)))
                    {
                        string enumName = _configAdapter.getAsString(fieldName, mapping.categoryName);
                        if (enumName != null)
                        {
                            //try
                            //{
                            mapping.fieldInfo.SetValue(this, Enum.Parse(mapping.fieldType, enumName, true)); // This will throw an exception if there is an string value in the config that does not correlate to any of the enum values. But that's okay maybe.
                            //}
                            //mapping.fieldInfo.SetValue(this, (int)guiInt642.Value);
                        }
                        else if (mapping.defaultValue != null)
                        {
                            mapping.fieldInfo.SetValue(this, mapping.defaultValue);
                        }
                    }
                    else
                    {

                        throw new Exception(mapping.fieldType.ToString() + " not implemented for reading from config.");
                    }
                    break;
            }
        }

        

        public void sendSingleValueToGUI(string fieldName)
        {
            PropertyMapping mapping = mappings[fieldName];

            switch (mapping.fieldType.ToString())
            {
                case "System.String":
                    _dataContext.writeString(mapping.mappedName,(string)mapping.fieldInfo.GetValue(this));
                    break;
                case "System.Int32":
                    _dataContext.writeInteger(mapping.mappedName,(int)mapping.fieldInfo.GetValue(this));
                    break;
                case "System.Int64":
                    _dataContext.writeInteger(mapping.mappedName,(Int64)mapping.fieldInfo.GetValue(this));
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

                        throw new Exception(mapping.fieldType.ToString() + " not implemented for writing to GUI.");
                    }
                    break;
            }
        }

        public void readSingleValueFromGUI(string fieldName)
        {
            PropertyMapping mapping = mappings[fieldName];

            switch (mapping.fieldType.ToString())
            {
                case "System.String":
                    string guiString =  _dataContext.getAsString(mapping.mappedName);
                    if(guiString != null)
                    {

                        mapping.fieldInfo.SetValue(this, guiString);
                    }
                    else if (mapping.defaultValue != null)
                    {
                        mapping.fieldInfo.SetValue(this, mapping.defaultValue);
                    }
                    break;
                case "System.Int32":
                    Int64? guiInt = _dataContext.getAsInteger(mapping.mappedName);
                    if (guiInt.HasValue)
                    {

                        mapping.fieldInfo.SetValue(this, (int)Math.Min(Int32.MaxValue,Math.Max(Int32.MinValue,guiInt.Value)));
                    }
                    else if (mapping.defaultValue != null)
                    {
                        mapping.fieldInfo.SetValue(this, mapping.defaultValue);
                    }
                    break;
                case "System.Int64":
                    Int64? guiInt64 = _dataContext.getAsInteger(mapping.mappedName);
                    if (guiInt64.HasValue)
                    {

                        mapping.fieldInfo.SetValue(this, guiInt64.Value);
                    }
                    else if (mapping.defaultValue != null)
                    {
                        mapping.fieldInfo.SetValue(this, mapping.defaultValue);
                    }
                    break;
                case "System.Single":
                    float? guiFloat = _dataContext.getAsFloat(mapping.mappedName);
                    if (guiFloat.HasValue)
                    {

                        mapping.fieldInfo.SetValue(this, guiFloat.Value);
                    }
                    else if (mapping.defaultValue != null)
                    {
                        mapping.fieldInfo.SetValue(this, mapping.defaultValue);
                    }
                    break;
                case "System.Double":
                    double? guiDouble = _dataContext.getAsDouble(mapping.mappedName);
                    if (guiDouble.HasValue)
                    {

                        mapping.fieldInfo.SetValue(this, guiDouble.Value);
                    }
                    else if (mapping.defaultValue != null)
                    {
                        mapping.fieldInfo.SetValue(this, mapping.defaultValue);
                    }
                    break;
                case "System.Boolean":
                    bool? guiBool = _dataContext.getAsBool(mapping.mappedName);
                    if (guiBool.HasValue)
                    {

                        mapping.fieldInfo.SetValue(this, guiBool.Value);
                    }
                    else if (mapping.defaultValue != null)
                    {
                        mapping.fieldInfo.SetValue(this, mapping.defaultValue);
                    }
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
                        if(enumNumber == -1)
                        {
                            if (mapping.defaultValue != null)
                            {
                                enumNumber = (int)mapping.defaultValue;
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





        class PropertyMapping
        {
            public string categoryName = null;
            public object defaultValue = null;
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
