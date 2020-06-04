using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CustomAccelerators
{
    [Bindable(BindableSupport.Yes,BindingDirection.OneWay)]
    public  class Extension :DependencyObject
    {
        public static string GetIdentity(Button obj)
        {
            return (string)obj.GetValue(IdentityProperty);
        }

        public static void SetIdentity(Button obj, string value)
        {
            obj.SetValue(IdentityProperty, value);
            CustomAccelerator ca = new CustomAccelerator(obj, value, GetIsEnabled(obj));
            obj.KeyboardAccelerators.Insert(0, ca);
        }

        // Using a DependencyProperty as the backing store for Identity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdentityProperty =
            DependencyProperty.RegisterAttached("Identity", typeof(string), typeof(Extension), new PropertyMetadata(string.Empty));

        public static bool GetIsEnabled(Button obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }
        public static void SetIsEnabled(Button obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
            if (obj.KeyboardAccelerators.Any())
            {
                obj.KeyboardAccelerators[0].IsEnabled = value;
            }
        }

        // Using a DependencyProperty as the backing store for IsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(Extension), new PropertyMetadata(true));



    }
}
