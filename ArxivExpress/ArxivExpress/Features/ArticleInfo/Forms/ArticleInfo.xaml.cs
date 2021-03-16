using ArxivExpress.Features.LikedArticles;
using ArxivExpress.Features.RecentlyViewedArticles.Data;
using ArxivExpress.Features.SearchArticles;
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

        private void CreatePdfUrl()
        {
            if (ArticleEntry.PdfUrl != "unknown")
            {
                StackLayoutArticleInfo.Children.Add(
                    new HyperlinkButton("Open Pdf", ArticleEntry.PdfUrl)
                    );
            }
        }       

        private void CreateAddLikedArticleButton(IArticleEntry articleEntry)
        {
            StackLayoutArticleInfo.Children.Add(
                new ToggleLikeButton(articleEntry)
                );
        }

        private void AddArticleToViewedList(IArticleEntry articleEntry)
        {
            var viewedArticleRepository = ViewedArticlesRepository.GetInstance();
            viewedArticleRepository.AddArticle(new Article(articleEntry));
        }

        public ArticleInfo(IArticleEntry articleEntry)
        {
            ArticleEntry = articleEntry;
            BindingContext = this;

            InitializeComponent();
            CreatePdfUrl();
            CreateAddLikedArticleButton(articleEntry);

            AddArticleToViewedList(articleEntry);
        }
    }
}
