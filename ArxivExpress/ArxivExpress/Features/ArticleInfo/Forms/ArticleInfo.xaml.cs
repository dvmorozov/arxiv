// ****************************************************************************
//    File "ArticleInfo.xaml.cs"
//    Copyright © Dmitry Morozov 2022
//    If you want to use this file please contact me by dvmorozov@hotmail.com.
// ****************************************************************************

using System;
using System.Threading.Tasks;
using ArxivExpress.Features.ArticleInfo.Forms;
using ArxivExpress.Features.LikedArticles;
using ArxivExpress.Features.LikedArticles.Forms;
using ArxivExpress.Features.RecentlyViewedArticles.Data;
using ArxivExpress.Features.SearchArticles;
using ArxivExpress.Features.SelectedArticles.Data;
using ArxivExpress.Features.SelectedArticles.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ArxivExpress.Features.ArticleInfo
{
    public partial class ArticleInfo : ContentPage
    {
        public IArticleEntry ArticleEntry { get; }

        private class HyperlinkLabel : Label
        {
            public string Url { get; }
            private ContentPage _page;

            private async Task Handle_Tap(string url)
            {
                try
                {
                    await Launcher.OpenAsync(url);
                }
                catch (Exception e)
                {
                    await _page.DisplayAlert("Error", e.Message, "Ok");
                }
            }

            public HyperlinkLabel(string text, string url, ContentPage page)
            {
                if (page == null)
                    throw new Exception("Page is not assigned.");

                Url = url;
                Text = text;
                _page = page;

                TextDecorations = TextDecorations.Underline;
                TextColor = Color.Blue;
                GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async () => await Handle_Tap(Url))
                });
            }
        }

        private void CreatePdfUrl()
        {
            if (ArticleEntry.PdfUrl != "unknown")
            {
                FlexLayoutToolbar.Children.Add(
                    new HyperlinkButton("Open Pdf", ArticleEntry.PdfUrl)
                    );
            }
        }       

        private void CreateAddLikedArticleButton(IArticleEntry articleEntry)
        {
            FlexLayoutToolbar.Children.Add(
                new ToggleLikeButton(articleEntry)
                );
        }

        private void CreateNewArticleListButton(IArticleEntry articleEntry)
        {
            FlexLayoutToolbar.Children.Add(
                new NewArticleListButton(articleEntry)
                );
        }

        private void CreateAddToArticleListButton(IArticleEntry articleEntry)
        {
            var selectedArticlesListsRepository = SelectedArticlesListsRepository.GetInstance();

            if (!selectedArticlesListsRepository.IsEmpty())
            {
                FlexLayoutToolbar.Children.Add(
                    new AddToArticleListButton(articleEntry)
                    );
            }
        }

        private void AddArticleToViewedList(IArticleEntry articleEntry)
        {
            var viewedArticleRepository = ViewedArticlesRepository.GetInstance();
            viewedArticleRepository.AddArticle(new Article(articleEntry));
        }

        private void DisplayButtons()
        {
            FlexLayoutToolbar.Children.Clear();

            CreatePdfUrl();
            CreateAddLikedArticleButton(ArticleEntry);
            CreateNewArticleListButton(ArticleEntry);
            CreateAddToArticleListButton(ArticleEntry);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            DisplayButtons();
        }

        public ArticleInfo(IArticleEntry articleEntry)
        {
            ArticleEntry = articleEntry;
            BindingContext = this;

            InitializeComponent();

            AddArticleToViewedList(articleEntry);
        }
    }
}
