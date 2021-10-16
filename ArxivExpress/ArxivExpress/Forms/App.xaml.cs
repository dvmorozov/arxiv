using Xamarin.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using ArxivExpress.Features.SearchArticles;

namespace ArxivExpress
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new SearchAttributes());
        }

        protected override void OnStart()
        {
            AppCenter.Start(
                  "android=23120715-db1e-4561-9bb5-51fc267aa42d;" +
                  "uwp=b9684c9b-b51d-4708-8c12-795a21ddc181;" +
                  "ios=40a6ae32-0f82-4331-88e0-a174464f5362;",
                  typeof(Analytics), typeof(Crashes), typeof(Distribute));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
