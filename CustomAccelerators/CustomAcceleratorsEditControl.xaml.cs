using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.UserDataTasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CustomAccelerators
{
    public sealed partial class CustomAcceleratorsEditControl : UserControl
    {
        public CustomAcceleratorsEditControl()
        {
            this.InitializeComponent();
            //MainListBox.ItemsSource = new List<AcceleratorDefinition> { new AcceleratorDefinition("vddd")  };

            MainListBox.ItemsSource =AcceleratorsManager.AcceleratorsDefinitions;

            //KeyDown += CustomAcceleratorsEditControl_KeyDown;
            //KeyUp += CustomAcceleratorsEditControl_KeyUp;
                
                //AcceleratorsManager.AcceleratorsDefinitions;
        }

        private void CustomAcceleratorsEditControl_KeyUp(object sender, KeyRoutedEventArgs e)
        {
 

        }

        private void CustomAcceleratorsEditControl_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void EditButton_Clicked(object sender, RoutedEventArgs e)
        {
            VirtualKeyModifiers mod = VirtualKeyModifiers.Control | VirtualKeyModifiers.Menu;
            Debug.WriteLine(mod.ToString());
            Debug.WriteLine((int)mod);
            var btn = (Button)sender;
            AcceleratorDefinition ad = (AcceleratorDefinition)btn.Tag;
            ad.Key = VirtualKey.A;
        }

        AcceleratorDefinition currentad;
        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = MainListBox.SelectedItem;
            currentad = (AcceleratorDefinition)selectedItem;
        }

        private void MainListBox_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void MainListBox_PreviewKeyUp(object sender, KeyRoutedEventArgs e)
        {
            e.Handled = true;
            if (currentad == null)
                return;
            if (e.Key == VirtualKey.Control || e.Key == VirtualKey.Shift || e.Key == VirtualKey.Menu || e.Key == VirtualKey.LeftWindows
                 || e.Key == VirtualKey.RightWindows)
                return;
            var sum = (Window.Current.CoreWindow.GetKeyState(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down) ? (int)VirtualKeyModifiers.Control : 0)
           + (Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down) ? (int)VirtualKeyModifiers.Shift : 0)
           + (Window.Current.CoreWindow.GetKeyState(VirtualKey.Menu).HasFlag(CoreVirtualKeyStates.Down) ? (int)VirtualKeyModifiers.Menu : 0)
           + ((Window.Current.CoreWindow.GetKeyState(VirtualKey.RightWindows).HasFlag(CoreVirtualKeyStates.Down)
            || Window.Current.CoreWindow.GetKeyState(VirtualKey.LeftWindows).HasFlag(CoreVirtualKeyStates.Down)) ? (int)VirtualKeyModifiers.Windows : 0);

            currentad.Modifiers = (VirtualKeyModifiers)sum;
            currentad.Key = e.Key;
            e.Handled = true;

        }
    }
}
