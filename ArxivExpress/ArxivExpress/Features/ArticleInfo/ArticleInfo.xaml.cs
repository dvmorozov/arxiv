using Xamarin.Essentials;
using Xamarin.Forms;

namespace ArxivExpress
{
    public partial class ArticleInfo : ContentPage
    {
        public ArticleEntry ArticleEntry { get; }

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
            BindingContext = this;

            InitializeComponent();
            CreatePdfUrl();
            CreateAddLikedArticleButton(articleEntry.Id);
        }
    }
}
