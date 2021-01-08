using System;
using ArxivExpress.Features.LikedArticles;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ArxivExpress
{
    public partial class ArticleInfo : ContentPage
    {
        public ArticleEntry ArticleEntry { get; }

        //  TODO: move buttons into separate sources.
        public class HyperlinkLabel : Label
        {
            public string Url { get; }

            public HyperlinkLabel(string text, string url)
            {
                Url = url;
                Text = text;

                TextDecorations = TextDecorations.Underline;
                TextColor = Color.Blue;
                GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async () => await Launcher.OpenAsync(Url))
                });
            }
        }

        public class HyperlinkButton : Button
        {
            public string Url { get; }  

            public HyperlinkButton(string text, string url)
            {
                Url = url;
                Text = text;

                BorderColor = Color.FromHex("#2196F3");
                BackgroundColor = Color.FromHex("#2196F3");
                BorderWidth = 2;
                Margin = 10;
                HeightRequest = 50;
                VerticalOptions = LayoutOptions.Center;
                HorizontalOptions = LayoutOptions.Center;
                WidthRequest = 200;
                TextColor = Color.White;
                Clicked += new EventHandler(delegate(Object o, EventArgs a)
                {
                    Launcher.OpenAsync(Url);
                });
            }
        }

        public class ToggleLikeButton : Button
        {
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
                LikedArticlesRepository = new LikedArticlesRepository();

                BorderColor = Color.FromHex("#2196F3");
                BackgroundColor = Color.FromHex("#2196F3");
                BorderWidth = 2;
                Margin = 10;
                HeightRequest = 50;
                VerticalOptions = LayoutOptions.Center;
                HorizontalOptions = LayoutOptions.Center;
                WidthRequest = 200;
                TextColor = Color.White;

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

        private void CreatePdfUrl()
        {
            if (ArticleEntry.PdfUrl != null)
            {
                StackLayoutArticleInfo.Children.Add(
                    new HyperlinkButton("Open Pdf", ArticleEntry.PdfUrl)
                    );
            }
        }       

        public void CreateAddLikedArticleButton(string articleId)
        {
            StackLayoutArticleInfo.Children.Add(
                new ToggleLikeButton(articleId)
                );
        }

        public ArticleInfo(ArticleEntry articleEntry)
        {
            ArticleEntry = articleEntry;
            //  TODO: convert LikedArticlesRepository to singleton.
            BindingContext = this;

            InitializeComponent();
            CreatePdfUrl();
            CreateAddLikedArticleButton(articleEntry.Id);
        }
    }
}
