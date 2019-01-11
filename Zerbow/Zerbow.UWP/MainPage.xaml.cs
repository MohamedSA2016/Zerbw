using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Zerbow.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new Zerbow.App());

            Xamarin.FormsMaps.Init("JT44pdVH11TQvVxFwc3r~ln9naLoS9c0Vg_4lQgcAIg~AlRU6GjbAYboNGhUH9LWdpvUlIZSdJtK0sgre8F5ig8xQNB32SY6EANWKFuTb5Ef ");
        }
    }
}
