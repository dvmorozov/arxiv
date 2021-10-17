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

        public ArticleInfo(IArticleEntry articleEntry)
        {
            ArticleEntry = articleEntry;
            BindingContext = this;

            InitializeComponent();

            CreatePdfUrl();
            CreateAddLikedArticleButton(articleEntry);
            CreateNewArticleListButton(articleEntry);
            CreateAddToArticleListButton(articleEntry);

            AddArticleToViewedList(articleEntry);
        }
    }
}
