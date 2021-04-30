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

        public event EventHandler sendToGUIRequested;
        public event EventHandler readFromGUIRequested;
        
        public event EventHandler saveToConfigRequested;
        public event EventHandler readFromConfigRequested;

        protected virtual void OnSendToGUIRequested(EventArgs e)
        {
            sendToGUIRequested?.Invoke(this, e);
        }
        protected virtual void OnReadFromGUIRequested(EventArgs e)
        {
            readFromGUIRequested?.Invoke(this, e);
        }

        protected virtual void OnSaveToConfigRequested(EventArgs e)
        {
            saveToConfigRequested?.Invoke(this, e);
        }
        protected virtual void OnReadFromConfigRequested(EventArgs e)
        {
            readFromConfigRequested?.Invoke(this, e);
        }

        public void sendToGUI()
        {
            OnSendToGUIRequested(EventArgs.Empty);
        }
        public void readFromGUI()
        {
            OnReadFromGUIRequested(EventArgs.Empty);
        }

        public void saveToConfig()
        {
            OnSaveToConfigRequested(EventArgs.Empty);
        }
        public void readFromConfig()
        {
            OnReadFromConfigRequested(EventArgs.Empty);
        }
        
    }
}
