using System;
using ArxivExpress.Features.LikedArticles;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ArxivExpress
{
    public partial class ArticleInfo : ContentPage
    {
        public ArticleEntry ArticleEntry { get; }

        public LikedArticlesRepository LikedArticlesRepository { get; }

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

        public class OrdinaryButton : Button
        {
            public OrdinaryButton(string text, EventHandler clicked)
            {
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
                Clicked += clicked;
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

        void Handle_LikePressed(object sender, EventArgs e)
        {
            LikedArticlesRepository.AddArticle("test");
        }

        public void CreateAddLikedArticleButton(string articleId, EventHandler clicked)
        {
            if (!LikedArticlesRepository.HasArticle(articleId))
            {
                StackLayoutArticleInfo.Children.Add(
                    new OrdinaryButton("Add to Liked", clicked)
                    );
            }
            else
            {
                StackLayoutArticleInfo.Children.Add(
                   new OrdinaryButton("Remove from Liked", clicked)
                   );
            }
        }

        public ArticleInfo(ArticleEntry articleEntry)
        {
            ArticleEntry = articleEntry;
            LikedArticlesRepository = new LikedArticlesRepository();
            BindingContext = this;

            InitializeComponent();
            CreatePdfUrl();
            CreateAddLikedArticleButton(articleEntry.Id, Handle_LikePressed);
        }
    }
}
