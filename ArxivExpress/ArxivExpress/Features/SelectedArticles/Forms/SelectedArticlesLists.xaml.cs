using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ArxivExpress.Features.SearchArticles;
using ArxivExpress.Features.SelectedArticles.Data;
using ArxivExpress.Features.SelectedArticles.Model;
using Xamarin.Forms;

namespace ArxivExpress.Features.SelectedArticles.Forms
{
    public partial class SelectedArticlesLists : ContentPage
    {
        public enum ListMode
        {
            ViewList,
            AddArticleToList
        }

        private SelectedArticlesListsRepository _selectedArticlesListsRepository;
        private ListMode _listMode;
        private IArticleEntry _articleEntry;

        public SelectedArticlesLists(IArticleEntry articleEntry)
        {
            _selectedArticlesListsRepository = SelectedArticlesListsRepository.GetInstance();
            _articleEntry = articleEntry;
            _listMode = ListMode.AddArticleToList;

            InitializeComponent();
            LoadSelectedArticlesLists();
        }


        public SelectedArticlesLists()
        {
            _selectedArticlesListsRepository = SelectedArticlesListsRepository.GetInstance();
            _listMode = ListMode.ViewList;

            InitializeComponent();
            LoadSelectedArticlesLists();
        }

        public async Task LoadSelectedArticlesLists()
        {
            Items = await _selectedArticlesListsRepository.LoadFirstPage();
        }

        private async void OpenArticleList(SelectedArticlesList list)
        {
            var rootNode = _selectedArticlesListsRepository.GetArticleListRoot(list.Name);

            var selectedArticlesListRepository = new SelectedArticlesListRepository(rootNode);
            var articleList = new SelectedArticleList(
                selectedArticlesListRepository, _selectedArticlesListsRepository);

            await Navigation.PushAsync(articleList);
        }

        private async void AddArticleToList(SelectedArticlesList list)
        {
            var rootNode = _selectedArticlesListsRepository.GetArticleListRoot(list.Name);

            var articlesListRepository = new SelectedArticlesListRepository(rootNode);
            articlesListRepository.AddArticle(_articleEntry);

            _selectedArticlesListsRepository.ReplaceArticleListElement(articlesListRepository.Root);
            //  Returns to previous page.
            await Navigation.PopAsync();
        }

        private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            if (e.Item is SelectedArticlesList list)
            {
                switch (_listMode)
                {
                    case (ListMode.ViewList):
                        OpenArticleList(list);
                        break;

                    case (ListMode.AddArticleToList):
                        AddArticleToList(list);
                        break;

                    default:
                        throw new Exception("Unsupported article list mode.");
                }
            }

            // Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        public async void Handle_ToolbarItemClicked(object sender, EventArgs e)
        {
            ToolbarItem item = (ToolbarItem)sender;
            if (item == _toolbarItemNextPage)
            {
                Items = await _selectedArticlesListsRepository.LoadNextPage();
            }
            else if (item == _toolbarItemPrevPage)
            {
                Items = await _selectedArticlesListsRepository.LoadPrevPage();
            }
        }

        private string GetItemsRange(uint pageIndex)
        {
            var resultsPerPage = _selectedArticlesListsRepository.GetResultsPerPage();
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

            if (_selectedArticlesListsRepository.GetPageNumber() > 0)
            {
                _toolbarItemPrevPage = CreateToolbarItem(_selectedArticlesListsRepository.GetPageNumber() - 1);
                ToolbarItems.Insert(0, _toolbarItemPrevPage);
            }

            if (!_selectedArticlesListsRepository.IsLastPage())
            {
                _toolbarItemNextPage = CreateToolbarItem(_selectedArticlesListsRepository.GetPageNumber() + 1);
                ToolbarItems.Add(_toolbarItemNextPage);
            }
        }

        protected ObservableCollection<SelectedArticlesList> Items
        {
            set
            {
                SelectedArticlesListsView.ItemsSource = value;
                SetToolbarPageNavigationItems();
            }
        }
    }
}
