// ****************************************************************************
//    File "SelectedArticlesLists.xaml.cs"
//    Copyright © Dmitry Morozov 2021
// ****************************************************************************

using System;
using System.Collections.ObjectModel;
using ArxivExpress.Features.LikedArticles;
using ArxivExpress.Features.RecentlyViewedArticles.Data;
using ArxivExpress.Features.SearchArticles;
using ArxivExpress.Features.SelectedArticles.Model;
using ArxivExpress.Features.ViewedAuthors.Forms;
using ArxivExpress.Forms;
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

        private IArticleEntry _articleEntry;
        private ListMode _listMode;
        private SelectedArticlesListsHelper _selectedArticlesListsHelper;

        /// <summary>
        /// Shows lists and add given article to selected list on tapping its name.
        /// </summary>
        /// <param name="articleEntry"></param>
        public SelectedArticlesLists(IArticleEntry articleEntry)
        {
            _articleEntry = articleEntry ??
                throw new Exception("Article entry is not assigned.");

            _selectedArticlesListsHelper = new SelectedArticlesListsHelper(Navigation);
            _listMode = ListMode.AddArticleToList;

            InitializeComponent();
        }

        /// <summary>
        /// This handler reloads list of lists on subsequent
        /// displaying after deleting some list of articles.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            LoadSelectedArticlesLists();
            DisplayButtons();
        }

        private void DisplayButtons()
        {
            var menuButtons = new StyledButton[]
            {
                new SearchButton(),
                new RecentlyViewedButton(),
                new LikedListButton(),
                new AuthorListButton()
            };

            foreach (var button in menuButtons)
                FlexLayoutToolbar.Children.Add(button);
        }

        /// <summary>
        /// Adds article to given list and opens the list immediately skipping
        /// displaying all article lists.
        /// </summary>
        /// <param name="articleEntry"></param>
        /// <param name="listName"></param>
        public SelectedArticlesLists(IArticleEntry articleEntry, string listName)
        {
            _articleEntry = articleEntry ??
                throw new Exception("Article entry is not assigned.");

            _selectedArticlesListsHelper = new SelectedArticlesListsHelper(Navigation);
            _listMode = ListMode.ViewList;

            InitializeComponent();
            _selectedArticlesListsHelper.AddArticleToList(listName, _articleEntry);
        }

        /// <summary>
        /// Shows lists and open selected list on tapping its name.
        /// </summary>
        public SelectedArticlesLists()
        {
            _selectedArticlesListsHelper = new SelectedArticlesListsHelper(Navigation);
            _listMode = ListMode.ViewList;

            InitializeComponent();
        }

        public async void LoadSelectedArticlesLists()
        {
            try
            {
                Items = await _selectedArticlesListsHelper.Repository.LoadFirstPage();
            }
            catch (Exception e)
            {
                await ArticleList.DisplayAlert(e, this);
            }
        }

        private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            if (e.Item is SelectedArticlesList list)
            {
                switch (_listMode)
                {
                    case (ListMode.ViewList):
                        _selectedArticlesListsHelper.OpenArticleList(list.Name);
                        break;

                    case (ListMode.AddArticleToList):
                        _selectedArticlesListsHelper.AddArticleToList(list.Name, _articleEntry);
                        _listMode = ListMode.ViewList;
                        break;

                    default:
                        throw new Exception("Unsupported article list mode.");
                }
            }

            // Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        public async void Handle_ToolbarItemAboutClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new About());
        }

        public async void Handle_ToolbarItemNavigationClicked(object sender, EventArgs e)
        {
            try
            {
                ToolbarItem item = (ToolbarItem)sender;

                if (item == _toolbarItemNextPage)
                {
                    Items = await _selectedArticlesListsHelper.Repository.LoadNextPage();
                }
                else if (item == _toolbarItemPrevPage)
                {
                    Items = await _selectedArticlesListsHelper.Repository.LoadPrevPage();
                }
            }
            catch (Exception ex)
            {
                await ArticleList.DisplayAlert(ex, this);
            }
        }

        private string GetItemsRange(uint pageIndex)
        {
            var resultsPerPage = _selectedArticlesListsHelper.Repository.GetResultsPerPage();
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
            item.Clicked += Handle_ToolbarItemNavigationClicked;

            return item;
        }

        private void SetToolbarPageNavigationItems()
        {
            DeleteToolbarItemPages();

            if (_selectedArticlesListsHelper.Repository.GetPageNumber() > 0)
            {
                _toolbarItemPrevPage = CreateToolbarItem(_selectedArticlesListsHelper.Repository.GetPageNumber() - 1);
                ToolbarItems.Insert(0, _toolbarItemPrevPage);
            }

            if (!_selectedArticlesListsHelper.Repository.IsLastPage())
            {
                _toolbarItemNextPage = CreateToolbarItem(_selectedArticlesListsHelper.Repository.GetPageNumber() + 1);
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
