using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;

namespace CustomAccelerators
{
    /// <summary>
    /// Saves and loads setting of keyboard accelerator
    /// </summary>
    internal static class LocalSettingsStorage
    {
        internal static void WriteToLocalSettingKey(AcceleratorDefinition acceleratorDefinition)
        {
            ApplicationData.Current.LocalSettings.Values[$"{_stringSettingKey}.{acceleratorDefinition.Identity}"] = (int)acceleratorDefinition.Key;
        }
        internal static void WriteToLocalSettingModifiers(AcceleratorDefinition acceleratorDefinition)
        {
            ApplicationData.Current.LocalSettings.Values[$"{_stringSettingModifiers}.{acceleratorDefinition.Identity}"] = (int)acceleratorDefinition.Modifiers;
        }
        private const string _stringSettingKey = "CustomAccelerators.VirtualKey";
        private const string _stringSettingModifiers = "CustomAccelerators.VirtualModifiers";
        internal static VirtualKey GetStoredKeyForIdentity(string identity)
        {
            string keyToLocalSettings = $"{_stringSettingKey}.{identity}";
            var settingObject = ApplicationData.Current.LocalSettings.Values[keyToLocalSettings];
            if (settingObject != null)
                return (VirtualKey)(int)settingObject;
            else
                return VirtualKey.None;
        }

        internal static (bool wasSetting, VirtualKeyModifiers modif) GetStoredModifiersForIdentity(string identity)
        {
            string keyToLocalSettings = $"{_stringSettingModifiers}.{identity}";
            var settingObject = ApplicationData.Current.LocalSettings.Values[keyToLocalSettings];
            if (settingObject != null)
                return (true, (VirtualKeyModifiers)(int)settingObject);
            else
                return (false, VirtualKeyModifiers.None);
        }
    }
}
