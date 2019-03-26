using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Zerbow.Views;
using Zerbow.Models;
using System.Collections.Generic;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Zerbow
{
    public partial class App : Application
    {



        public App()
        {


            InitializeComponent();




            MainPage = new NavigationPage(new Login());



        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {


        }

        protected override void OnResume()
        {

        }
    }
}
