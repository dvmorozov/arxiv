using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ArxivExpress
{
    public partial class ArticleInfo : ContentPage
    {
        public ArticleList.ArticleEntry ArticleEntry { get; }

        public ICommand Handle_PdfUrlTapped => new Command<string>(
            async (url) => await Launcher.OpenAsync(url));

        public ArticleInfo(ArticleList.ArticleEntry articleEntry)
        {
            ArticleEntry = articleEntry;
            BindingContext = this;

            InitializeComponent();
        }
    }
}
