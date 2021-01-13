using System;
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
        //  TODO: make private.
        public LikedArticlesRepository LikedArticlesRepository { get; }

        class LikeButtonContext
        {
            public readonly string ArticleId;

            public LikeButtonContext(string articleId)
            {
                ArticleId = articleId;
            }
        }

        public ToggleLikeButton(string articleId)
        {
            LikedArticlesRepository = LikedArticlesRepository.GetInstance();
            BindingContext = new LikeButtonContext(articleId);

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
            var context = BindingContext as LikeButtonContext;
            if (context != null)
            {
                if (LikedArticlesRepository.HasArticle(context.ArticleId))
                {
                    LikedArticlesRepository.DeleteArticle(context.ArticleId);
                }
                else
                {
                    LikedArticlesRepository.AddArticle(context.ArticleId);
                }
            }
            SetText();
        }

        private void SetText()
        {
            var context = BindingContext as LikeButtonContext;
            if (context != null)
            {
                Text = LikedArticlesRepository.HasArticle(context.ArticleId)
                    ? "Remove from Liked" : "Add to Liked";
            }
        }
    }
}
