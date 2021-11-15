using System;
using System.Collections.Generic;
using System.Net.Mail;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ArxivExpress.Forms
{
    public partial class About : ContentPage
    {
        public string Version => VersionTracking.CurrentVersion;

        public string Build => VersionTracking.CurrentBuild;

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

        }
    }
}
