// ****************************************************************************
//    File "MainActivity.cs"
//    Copyright © Dmitry Morozov 2022
//    If you want to use this file please contact me by dvmorozov@hotmail.com.
// ****************************************************************************

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using PCLAppConfig;

namespace ArxivExpress.Droid
{
    [Activity(Label = "ArxivExpress", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            global::Xamarin.Forms.Forms.SetFlags("RadioButton_Experimental");

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}