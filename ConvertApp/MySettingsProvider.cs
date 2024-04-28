using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertApp
{
    internal class MySettingsProvider
    {
        public class CustomSettingsProvider : SettingsProvider
        {
            // Specify the fixed path for settings storage
            private readonly string settingsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ConvertApp",
                "Settings");

            public override string ApplicationName
            {
                get { return "ConvertApp"; }
                set { }
            }

            public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
            {
                base.Initialize(ApplicationName, config);
            }

            public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
            {
                SettingsPropertyValueCollection values = new SettingsPropertyValueCollection();

                foreach (SettingsProperty property in collection)
                {
                    SettingsPropertyValue value = new SettingsPropertyValue(property)
                    {
                        PropertyValue = LoadSetting(property)
                    };

                    values.Add(value);
                }

                return values;
            }

            public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
            {
                foreach (SettingsPropertyValue value in collection)
                {
                    SaveSetting(value.Property, value.PropertyValue);
                }
            }

            private object LoadSetting(SettingsProperty property)
            {
                // Load setting from the fixed path
                string filePath = Path.Combine(settingsPath, property.Name + ".settings");
                if (File.Exists(filePath))
                {
                    return File.ReadAllText(filePath);
                }
                else
                {
                    return property.DefaultValue;
                }
            }

            private void SaveSetting(SettingsProperty property, object value)
            {
                // Save setting to the fixed path
                string filePath = Path.Combine(settingsPath, property.Name + ".settings");
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                File.WriteAllText(filePath, value.ToString());
            }
        }
    }
}
