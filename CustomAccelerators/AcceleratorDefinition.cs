using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace CustomAccelerators
{   
    /// <summary>
    /// Definition holds the accelerators, that have same identity.
    /// This is the way how to gather (and edit) all possible accelerators
    /// </summary>
    public class AcceleratorDefinition : INotifyPropertyChanged
    {
        
        public AcceleratorDefinition(string identity, string label, VirtualKey keyFromDefinition, VirtualKeyModifiers modifiersFromDefinition ) 
        {
            Identity = identity;
            Label = string.IsNullOrEmpty(label) ? identity : label;
            this.keyDefault = keyFromDefinition;
            this.modifiersDefault = modifiersFromDefinition;
        }

        /// <summary>
        /// Uniq id of definition
        /// </summary>
        public string Identity { get; set; }
        /// <summary>
        /// Human readable string
        /// </summary>
        public string Label { get; set; }

        VirtualKey _key;
        public VirtualKey Key
        {
            get => _key;
            set
            {
                if (_key == value) return;
                Set(ref _key, value);
                LocalSettingsStorage.WriteToLocalSettingKey(this);
                CustomAccelerators.ForEach(x => x.SetKey(value));
            }
        }

        public string DontDisplayNone(VirtualKeyModifiers mod)
        {
            return mod == VirtualKeyModifiers.None ? "" : mod.ToString() + " + ";
        }

        VirtualKeyModifiers _modifiers;
        private readonly VirtualKey keyDefault;// used when user wants to reset shortcut
        private readonly VirtualKeyModifiers modifiersDefault;

        public void SetKeyAndModifiersToDefault()
        {
            Modifiers = modifiersDefault;
            Key = keyDefault;
        }


        public VirtualKeyModifiers Modifiers
        {
            get => _modifiers;
            set
            {
                if (_modifiers == value) return;
                Set(ref _modifiers, value);
                LocalSettingsStorage.WriteToLocalSettingModifiers(this);
                    CustomAccelerators.ForEach( x=> x.SetModifiers(value));
            }
        }
        //internal void RefreshAllTooltips()
        //{
        //    CustomAccelerators.ForEach(x => x.RefreshTooltip());
        //}

        /// <summary>
        /// List of xaml accelerators with same identity...You can have multiple accelerators with same identity.
        /// It is all bcs of: https://stackoverflow.com/questions/53735503/keyboard-accelerator-stops-working-in-uwp-app/62025749#62025749
        /// </summary>
        //public List<CustomAccelerator> CustomAccelerators { get; set; } = new List<CustomAccelerator>();
        public List<CustomAccelerator> CustomAccelerators { get; set; } = new List<CustomAccelerator>();
        //public List<CustomAcceleratorSecondary> SecondaryAccelerators { get; internal set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }
            storage = value;
            OnPropertyChanged(propertyName);
        }


    }
}
