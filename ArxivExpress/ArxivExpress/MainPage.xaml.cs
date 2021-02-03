﻿using ArxivExpress.Features.LikedArticles;
using ArxivExpress.Features.RecentlyViewedArticles;
using ArxivExpress.Features.SearchArticles;
using Xamarin.Forms;

namespace ArxivExpress
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public async void Handle_SearchPressed(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Features.SearchArticles.SearchAttributes());
        }

        public async void Handle_RecentlyViewedPressed(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ArticleList(ViewedArticlesRepository.GetInstance()));
        }

        public async void Handle_LikedArticlesPressed(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ArticleList(LikedArticlesRepository.GetInstance()));
        }

        public async void Handle_RecommendedArticlesPressed(object sender, System.EventArgs e)
        {
            //  TODO: Implement recommended articles.
            //await Navigation.PushAsync(new SearchQuery());
        }

        public async void Handle_ViewedAuthorsPressed(object sender, System.EventArgs e)
        {
            //  TODO: Implement viewed author's articles.
            //await Navigation.PushAsync(new SearchQuery());
        }
    }
}
