using System;
using ArxivExpress.Features.ArticleList;
using ArxivExpress.Features.LikedArticles;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ArxivExpress
{
    public class StyledButton : Button
    {
        public StyledButton()
        {
            BorderColor = Color.FromHex("#2196F3");
            BackgroundColor = Color.FromHex("#2196F3");
            BorderWidth = 2;
            Margin = 10;
            HeightRequest = 50;
            VerticalOptions = LayoutOptions.Center;
            HorizontalOptions = LayoutOptions.Center;
            WidthRequest = 200;
            TextColor = Color.White;
        }
    }

    public class HyperlinkButton : StyledButton
    {
        public string Url { get; }

        public HyperlinkButton(string text, string url)
        {
            Url = url;
            Text = text;

            Clicked += new EventHandler(delegate (Object o, EventArgs a)
            {
                Launcher.OpenAsync(Url);
            });
        }
    }

    public class ToggleLikeButton : StyledButton
    { 
        public ToggleLikeButton(IArticleEntry articleEntry)
        {
            BindingContext = articleEntry;

            Clicked += Handle_Pressed;
            SetText();
        }

        private void Handle_Pressed(object sender, EventArgs e)
        {
            if (sender == this)
            {
                ToggleLikeStatus();
            }
        }

        private void ToggleLikeStatus()
        {
            var likedArticlesRepository = LikedArticlesRepository.GetInstance();

            if (BindingContext is IArticleEntry articleEntry)
            {
                if (likedArticlesRepository.HasArticle(articleEntry.Id))
                {
                    likedArticlesRepository.DeleteArticle(articleEntry.Id);
                }
                else
                {
                    likedArticlesRepository.AddArticle(new LikedArticle(articleEntry));
                }
            }
            SetText();
        }

        private void SetText()
        {
            if (BindingContext is IArticleEntry articleEntry)
            {
                var likedArticlesRepository = LikedArticlesRepository.GetInstance();
                Text = likedArticlesRepository.HasArticle(articleEntry.Id)
                    ? "Remove from Liked" : "Add to Liked";
            }
        }
    }
}
