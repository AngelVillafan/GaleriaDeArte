using GaleriaDeArte.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GaleriaDeArte
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Sharpnado.Shades.Initializer.Initialize(loggerEnable: false);

            //MainPage = new MainPage();
            MainPage = new NavigationPage(new GaleriaView());

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
