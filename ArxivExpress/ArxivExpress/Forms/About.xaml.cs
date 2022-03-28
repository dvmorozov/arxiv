// ****************************************************************************
//    File "About.xaml.cs"
//    Copyright © Dmitry Morozov 2022
//    If you want to use this file please contact me by dvmorozov@hotmail.com.
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.Net.Mail;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ArxivExpress.Forms
{
    public partial class About : ContentPage
    {
        public string Version => AppInfo.VersionString;

        public string Build => AppInfo.BuildString;

        public string AppName => AppInfo.Name;

        public string AppTitle
        {
            get
            {
                string[] parts = AppName.Split('.');
                return "About " + (parts.Length != 0 ? parts[0] : "");
            }
        }

        public string Feedback;

        public About()
        {
            BindingContext = this;
            InitializeComponent();
        }

        private void Handle_FeedbackCompleted(object sender, EventArgs e)
        {
            if (sender == EditorFeedback)
            {
                Feedback = ((Editor)sender).Text;
            }
        }

        private void Handle_FeedbackChanged(object sender, EventArgs e)
        {
            if (sender == EditorFeedback)
            {
                Feedback = ((Editor)sender).Text;
            }
        }

        private void Handle_SendFeedbackPressed(object sender, EventArgs e)
        {
            var properties = new Dictionary<string, string>
            {
                {"Feedback", Feedback}
            };

            Analytics.TrackEvent("Feedback", properties);
        }
    }
}
