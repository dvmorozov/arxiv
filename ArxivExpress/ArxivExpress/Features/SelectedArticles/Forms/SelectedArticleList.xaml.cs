// ****************************************************************************
//    File "SelectedArticleList.xaml.cs"
//    Copyright © Dmitry Morozov 2021
// ****************************************************************************

using System;
using System.Collections.ObjectModel;
using ArxivExpress.Features.LikedArticles;
using ArxivExpress.Features.RecentlyViewedArticles.Data;
using ArxivExpress.Features.SearchArticles;
using ArxivExpress.Features.SelectedArticles.Data;
using ArxivExpress.Features.ViewedAuthors.Forms;
using ArxivExpress.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArxivExpress.Features.SelectedArticles.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectedArticleList : ContentPage
    {
        private readonly SelectedArticlesListRepository _selectedArticlesListRepository;
        private readonly SelectedArticlesListsRepository _selectedArticlesListsRepository;

        public SelectedArticleList(
            SelectedArticlesListRepository selectedArticlesListRepository,
            SelectedArticlesListsRepository selectedArticlesListsRepository, string Name)
        {
            _selectedArticlesListRepository = selectedArticlesListRepository ??
                throw new Exception("Article repository is not assigned.");
            _selectedArticlesListsRepository = selectedArticlesListsRepository ??
                throw new Exception("Article lists repository is not assigned.");

            InitializeComponent();
            Title = Name;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            LoadArticles();
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

        public async void LoadArticles()
        {
            Items = await _selectedArticlesListRepository.LoadFirstPage();
        }

        private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            if (e.Item is IArticleEntry articleEntry)
            { 
                Navigation.PushAsync(new ArticleInfo.ArticleInfo(articleEntry));
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
                    Items = await _selectedArticlesListRepository.LoadNextPage();
                }
                else if (item == _toolbarItemPrevPage)
                {
                    Items = await _selectedArticlesListRepository.LoadPrevPage();
                }
            }
            catch (Exception ex)
            {
                await ArticleList.DisplayAlert(ex, this);
            }
        }

        private string GetItemsRange(uint pageIndex)
        {
            var resultsPerPage = _selectedArticlesListRepository.GetResultsPerPage();
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

            if (_selectedArticlesListRepository.GetPageNumber() > 0)
            {
                _toolbarItemPrevPage = CreateToolbarItem(_selectedArticlesListRepository.GetPageNumber() - 1);
                ToolbarItems.Insert(0, _toolbarItemPrevPage);
            }

            if (!_selectedArticlesListRepository.IsLastPage())
            {
                _toolbarItemNextPage = CreateToolbarItem(_selectedArticlesListRepository.GetPageNumber() + 1);
                ToolbarItems.Add(_toolbarItemNextPage);
            }
        }

        private ObservableCollection<IArticleEntry> Items
        {
            set
            {
                ArticleListView.ItemsSource = value;
                SetToolbarPageNavigationItems();
            }
        }

        private void OnRemoveArticleFromListPressed(object sender, EventArgs e)
        {
            var menuItem = sender as Button;
            var articleEntry = menuItem.CommandParameter as IArticleEntry;

            RemoveArticleFromList(articleEntry);
            Navigation.PopAsync(true);
        }

        private async void OnDeleteListPressed(object sender, EventArgs e)
        {
            if (_selectedArticlesListRepository.Root == null ||
                _selectedArticlesListRepository.Root.Attribute("Name") == null)
                throw new Exception("Root node is not assigned or its name is not assigned.");

            if (await DisplayAlert("Please confirm",
                "Delete list \"" + _selectedArticlesListRepository.Root.Attribute("Name").Value + "\"?",
                "Yes", "No"))
            {
                _selectedArticlesListsRepository.DeleteArticleListElement(_selectedArticlesListRepository.Root);
                await Navigation.PopAsync(true);
            }
        }

        private void RemoveArticleFromList(IArticleEntry articleEntry)
        {
            if (articleEntry == null)
                throw new Exception("Article entry is empty.");

            _selectedArticlesListRepository.DeleteArticle(articleEntry.Id);
            _selectedArticlesListsRepository.ReplaceArticleListElement(_selectedArticlesListRepository.Root);
        }
    }
}
