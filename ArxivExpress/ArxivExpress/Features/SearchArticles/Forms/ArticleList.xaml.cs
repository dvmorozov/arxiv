using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArxivExpress.Features.SearchArticles
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArticleList : ContentPage
    {
        private IArticlesRepository _articleRepository;

        public ArticleList(IArticlesRepository articleRepository)
        {
            _articleRepository = articleRepository;

            InitializeComponent();
            LoadArticles();
        }

        public async Task LoadArticles()
        {
            Items = await _articleRepository.LoadArticles();
        }

        private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            if (e.Item is IArticleEntry articleEntry)
            { 
                await Navigation.PushAsync(new ArticleInfo.ArticleInfo(articleEntry));
            }

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        public async void Handle_ToolbarItemClicked(object sender, EventArgs e)
        {
            ToolbarItem item = (ToolbarItem)sender;
            if (item == _toolbarItemNextPage)
            {
                Items = await _articleRepository.LoadNextPage();
            }
            else if (item == _toolbarItemPrevPage)
            {
                Items = await _articleRepository.LoadPrevPage();
            }
        }

        private string GetItemsRange(uint pageIndex)
        {
            var resultsPerPage = _articleRepository.GetResultsPerPage();
            var startIndex = pageIndex * resultsPerPage + 1;
            var endIndex = (pageIndex + 1) * resultsPerPage;

            return startIndex.ToString() + "-" + endIndex.ToString();
        }

        private ToolbarItem _toolbarItemPrevPage;
        private ToolbarItem _toolbarItemNextPage;

        private void DeleteToolbarItemPages()
        {
            if (_toolbarItemPrevPage != null)
            {
                this.ToolbarItems.Remove(_toolbarItemPrevPage);
            }
            if (_toolbarItemNextPage != null)
            {
                this.ToolbarItems.Remove(_toolbarItemNextPage);
            }
        }

        private ToolbarItem CreateToolbarItem(uint pageNumber)
        {
            var item = new ToolbarItem
            {
                Text = GetItemsRange(pageNumber),
                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };
            item.Clicked += Handle_ToolbarItemClicked;

            return item;
        }

        private void SetToolbarPageNavigationItems()
        {
            DeleteToolbarItemPages();

            if (_articleRepository.GetPageNumber() > 0)
            {
                _toolbarItemPrevPage = CreateToolbarItem(_articleRepository.GetPageNumber() - 1);
                ToolbarItems.Insert(0, _toolbarItemPrevPage);
            }

            _toolbarItemNextPage = CreateToolbarItem(_articleRepository.GetPageNumber() + 1);
            ToolbarItems.Add(_toolbarItemNextPage);
        }

        private ObservableCollection<IArticleEntry> Items
        {
            set
            {
                ArticleListView.ItemsSource = value;
                SetToolbarPageNavigationItems();
            }
        }
    }
}
