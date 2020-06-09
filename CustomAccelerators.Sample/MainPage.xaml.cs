using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CustomAccelerators.Sample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var acceleratorsList = new List<(string identity, string label, VirtualKey key, VirtualKeyModifiers modifiers)>()
                {
                    ("Hello","I am Hello Button",VirtualKey.H,VirtualKeyModifiers.Control|VirtualKeyModifiers.Shift),
                    ("Another Accelerator","",VirtualKey.PageDown,VirtualKeyModifiers.None),
                    ("notInXamlButton","Not In Xaml now, but could be in a future this is long text to test UI",VirtualKey.F10,VirtualKeyModifiers.Menu),
                    ("SimpleAccelerator","Simple Accelerator",VirtualKey.F,VirtualKeyModifiers.Control|VirtualKeyModifiers.Shift),

                };

            AcceleratorsManager.AddDefaultsAndLoadFromStorage(acceleratorsList);
        }

        int pressedCountHelloButton = 0;
        int pressedCountAnotherButton = 0;
        private void HelloButton_Click(object sender, RoutedEventArgs e)
        {
            PressedCountHelloButton.Text = (++pressedCountHelloButton).ToString();
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = true;
        }

        readonly Random rnd = new Random();
        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            SimpleButton.Content = $"RND num: {rnd.Next(0,1000)}";
        }
    }
}
