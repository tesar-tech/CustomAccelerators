using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.UserDataTasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CustomAccelerators
{
    public class CustomAccelerator : KeyboardAccelerator, INotifyPropertyChanged
    {

        
        public CustomAccelerator()
        {
                        
            
        }
        public CustomAccelerator(Button btn, string identity, bool isEnabled)
        :this()
        {
            buttonParent = btn;
            if (buttonParent.GetType() == typeof(AppBarButton))
                appBarButtonParent = (AppBarButton)btn;
            IsEnabled = isEnabled;
            Identity = identity;
        }
        /// <summary>
        /// this is for setting tooltip when key changes. This is only set when using Extension
        /// </summary>
        private Button buttonParent;

        private AppBarButton appBarButtonParent;

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
        internal Type TypeOfPageOfAccelerator { get; set; }

        private string _TooltipString;
        /// <summary>
        /// Lable with shortcut (e.g. "Copy (Control+C)")
        /// </summary>
        public string TooltipString
        {
            get => _TooltipString;
            private set
            {
                Set(ref _TooltipString, value);
                if (buttonParent != null)
                {
                    ToolTipService.SetToolTip(buttonParent, value);
                    if (appBarButtonParent != null)
                        appBarButtonParent.KeyboardAcceleratorTextOverride = AcceleratorString;
                    //this is necessary for appbar buttons and their shortcut in menuflyout
                }

            }
        }

        /// <summary>
        /// method for refreshing tooltip string when key or modifiers are changed
        /// When accelerator is disabled => dont add shortcut to tooltip
        /// </summary>
        private void ConstructTooltipString() => TooltipString = $"{Label} ({AcceleratorString})";

        string AcceleratorString
        {
            get
            {
                var modStr = base.Modifiers == VirtualKeyModifiers.None ? "" : base.Modifiers.ToString() + " + ";
                return modStr + base.Key.ToString();
            }
        }

        internal void RefreshTooltip()
        {
            OnPropertyChanged(nameof(TooltipString));
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

        /// <summary>
        /// simplification when creating modifiers - this will construct tooltip only once
        /// </summary>
        /// <param name="key"></param>
        /// <param name="modifiers"></param>
        /// <param name="label"></param>
        internal void SetKeyModifierLabel(VirtualKey key, VirtualKeyModifiers modifiers, string label)
        {
            base.Key = key;
            base.Modifiers = modifiers;
            Label = label;
        }
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

        internal void SetIsEnabled(bool isEnabled)
        {
            IsEnabled = isEnabled;
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
