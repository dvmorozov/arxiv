using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ArxivExpress
{
    public partial class SearchQueryAttributes : ContentPage
    {
        public SearchQueryAttributes()
        {
            InitializeComponent();
        }

        async void Handle_SearchPressed(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ArticleList());
        }
    }
}
