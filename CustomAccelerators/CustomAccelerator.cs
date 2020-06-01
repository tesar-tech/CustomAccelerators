using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.UserDataTasks;
using Windows.System;
using Windows.UI.Xaml.Input;

namespace CustomAccelerators
{
    public class CustomAccelerator : KeyboardAccelerator, INotifyPropertyChanged
    {
        private string _identity;
        public string Identity
        {
            get => _identity;

            set
            {
                if (!string.IsNullOrEmpty(_identity))
                    throw new Exception("it is set twice, its wrong");
                _identity = value;
                 AcceleratorsManager.AddToList(this);//adds itself to manager,
                //so it can change the key when user do it within the settings
            }
        }

        private string _TooltipString;
        /// <summary>
        /// Lable with shortcut (e.g. "Copy (Control+C)")
        /// </summary>
        public string TooltipString
        {
            get => _TooltipString;
           private set => Set(ref _TooltipString, value);
        }

        /// <summary>
        /// method for refreshing tooltip string when key or modifiers are changed
        /// </summary>
        private void ConstructTooltipString() => TooltipString = $"{Label} ({GetAcceleratorString()})";

        string GetAcceleratorString()
        {
            var modStr = base.Modifiers == VirtualKeyModifiers.None ? "" : base.Modifiers.ToString() + " + ";
            return modStr + base.Key.ToString();
        }


        private string _Label;
        /// <summary>
        /// This will appear in Edit control. If not set, Identity is used
        /// </summary>
        public string Label
        {
            get => _Label;
            set {
                Set(ref _Label, value);
                ConstructTooltipString();
            }
        }


        [Obsolete("This doesn't set the key. Set default value in definition")]
        public new VirtualKey Key { get; set; }
  
        [Obsolete("This doesn't set the modifiers. Set default value in definition")]
        public new VirtualKeyModifiers Modifiers { get; set; }

        internal void SetKey(VirtualKey key)
        {
            base.Key = key;
            ConstructTooltipString();
        }

        internal void SetModifiers(VirtualKeyModifiers modifiers)
        {
            base.Modifiers = modifiers;
            ConstructTooltipString();
        }

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
