using System;
using System.IO;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GptApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
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
