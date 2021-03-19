using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ArxivExpress.Features.SearchArticles;
using ArxivExpress.Features.ViewedAuthors.Data;
using ArxivExpress.Features.ViewedAuthors.Model;
using Xamarin.Forms;

namespace ArxivExpress.Features.ViewedAuthors.Forms
{
    public partial class AuthorList : ContentPage
    {
        private ViewedAuthorsRepository _authorsRepository;

        public AuthorList()
        {
            _authorsRepository = ViewedAuthorsRepository.GetInstance();

            InitializeComponent();
            LoadAuthors();
        }

        public async Task LoadAuthors()
        {
            Items = await _authorsRepository.LoadFirstPage();
        }

        private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            if (e.Item is Author author)
            {
                //await Navigation.PushAsync(new SearchAttributes());

                // Search articles by author.
                var _searchArticleRepository = SearchArticlesRepository.GetInstance();
                _searchArticleRepository.SearchQuery =
                    new SearchQuery(searchTerm : author.Name, prefix : "au");

                var articleList = new ArticleList(_searchArticleRepository);
                await Navigation.PushAsync(articleList);
            }

            // Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        public async void Handle_ToolbarItemClicked(object sender, EventArgs e)
        {
            ToolbarItem item = (ToolbarItem)sender;
            if (item == _toolbarItemNextPage)
            {
                Items = await _authorsRepository.LoadNextPage();
            }
            else if (item == _toolbarItemPrevPage)
            {
                Items = await _authorsRepository.LoadPrevPage();
            }
        }

        private string GetItemsRange(uint pageIndex)
        {
            var resultsPerPage = _authorsRepository.GetResultsPerPage();
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

            if (_authorsRepository.GetPageNumber() > 0)
            {
                _toolbarItemPrevPage = CreateToolbarItem(_authorsRepository.GetPageNumber() - 1);
                ToolbarItems.Insert(0, _toolbarItemPrevPage);
            }

            if (!_authorsRepository.IsLastPage())
            {
                _toolbarItemNextPage = CreateToolbarItem(_authorsRepository.GetPageNumber() + 1);
                ToolbarItems.Add(_toolbarItemNextPage);
            }
        }

        protected ObservableCollection<Author> Items
        {
            set
            {
                AuthorListView.ItemsSource = value;
                SetToolbarPageNavigationItems();
            }
        }
    }
}
