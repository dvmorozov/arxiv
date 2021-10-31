// ****************************************************************************
//    File "Main.cs"
//    Copyright © Dmitry Morozov 2021
// ****************************************************************************

using UIKit;

namespace ArxivExpress.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
