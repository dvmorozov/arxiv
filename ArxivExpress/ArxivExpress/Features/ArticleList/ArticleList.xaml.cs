using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.SyndicationFeed;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArxivExpress.Features.ArticleList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArticleList : ContentPage
    {
        private AtomFeedProcessor _atomFeedProcessor;
        private SearchArticles.SearchQuery _searchQuery;

        public ArticleList(SearchArticles.SearchQuery searchQuery)
        {
            _atomFeedProcessor = new AtomFeedProcessor(this);
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
                await MakeRequest();
            }
            else if (item == _toolbarItemPrevPage)
            {
                if (_searchQuery.PageNumber > 0)
                {
                    _searchQuery.PageNumber--;
                    await MakeRequest();
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

        public async Task MakeRequest()
        {
            _atomFeedProcessor.Items.Clear();

            await AtomFeedRequest.MakeRequest(
                _searchQuery.GetQueryString(), _atomFeedProcessor);

            ArticleListView.ItemsSource = _atomFeedProcessor.Items;
            SetToolbarPageNavigationItems();
        }

        private class AtomFeedProcessor : AtomFeedRequest.IAtomFeedProcessor
        {
            //  TODO: remove _articleList.
            private ArticleList _articleList;
            private ObservableCollection<ArticleEntry> _items;
            public ObservableCollection<ArticleEntry> Items
            {
                get { return _items; }
            }

            public AtomFeedProcessor(ArticleList articleList)
            {
                _items = new ObservableCollection<ArticleEntry>();
                _articleList = articleList;
            }

            void AtomFeedRequest.IAtomFeedProcessor.ProcessCategory(ISyndicationCategory category)
            {
            }

            void AtomFeedRequest.IAtomFeedProcessor.ProcessImage(ISyndicationImage image)
            {
            }

            void AtomFeedRequest.IAtomFeedProcessor.ProcessEntry(IAtomEntry entry)
            {
                if (entry != null)
                    _items.Add(new ArticleEntry(entry));
            }

            void AtomFeedRequest.IAtomFeedProcessor.ProcessLink(ISyndicationLink link)
            {
            }

            void AtomFeedRequest.IAtomFeedProcessor.ProcessPerson(ISyndicationPerson person)
            {
            }

            void AtomFeedRequest.IAtomFeedProcessor.ProcessContent(ISyndicationContent content)
            {
            }
        }
    }
}
