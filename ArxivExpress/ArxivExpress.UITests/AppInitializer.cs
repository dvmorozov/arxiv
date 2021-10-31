// ****************************************************************************
//    File "AppInitializer.cs"
//    Copyright © Dmitry Morozov 2021
// ****************************************************************************

using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace ArxivExpress.UITests
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp.Android.StartApp();
            }

            return ConfigureApp.iOS.StartApp();
        }
    }
}