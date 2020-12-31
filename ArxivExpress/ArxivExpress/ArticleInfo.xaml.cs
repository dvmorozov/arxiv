using Xamarin.Forms;

namespace ArxivExpress
{
    public partial class ArticleInfo : ContentPage
    {
        public ArticleList.ArticleEntry ArticleEntry { get; }

        public ArticleInfo(ArticleList.ArticleEntry articleEntry)
        {
            ArticleEntry = articleEntry;
            BindingContext = ArticleEntry;

            InitializeComponent();
        }
    }
}
