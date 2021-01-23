using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ArxivExpress.Features.ArticleList;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArxivExpress.Features.SearchArticles
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FoundArticlesList : ContentPage
    {
        private readonly SearchQuery _searchQuery;

        public FoundArticlesList(SearchQuery searchQuery)
        {
            _searchQuery = searchQuery;

            InitializeComponent();
        }

        public async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
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
                _searchQuery.PageNumber++;
                await LoadArticles();
            }
            else if (item == _toolbarItemPrevPage)
            {
                if (_searchQuery.PageNumber > 0)
                {
                    _searchQuery.PageNumber--;
                    await LoadArticles();
                }
            }
        }

        private string GetItemsRange(uint pageIndex)
        {
            var resultsPerPage = _searchQuery.GetResultsPerPage();
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

            if (_searchQuery.PageNumber > 0)
            {
                _toolbarItemPrevPage = CreateToolbarItem(_searchQuery.PageNumber - 1);
                ToolbarItems.Insert(0, _toolbarItemPrevPage);
            }

            _toolbarItemNextPage = CreateToolbarItem(_searchQuery.PageNumber + 1);
            ToolbarItems.Add(_toolbarItemNextPage);
        }

        public async Task LoadArticles()
        {
            var atomFeedProcessor = new AtomFeedProcessor();

            await AtomFeedRequest.MakeRequest(
                _searchQuery.GetQueryString(), atomFeedProcessor);

            Items = atomFeedProcessor.Items;
        }

        public ObservableCollection<ArticleEntry> Items
        {
            set
            {
                ArticleListView.ItemsSource = value;
                SetToolbarPageNavigationItems();
            }
        }
    }
}
