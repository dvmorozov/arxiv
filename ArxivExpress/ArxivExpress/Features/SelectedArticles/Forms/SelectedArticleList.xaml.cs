﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ArxivExpress.Features.SearchArticles;
using ArxivExpress.Features.SelectedArticles.Data;
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
            SelectedArticlesListsRepository selectedArticlesListsRepository)
        {
            if (selectedArticlesListRepository == null)
                throw new Exception("Article repository is not assigned.");
            if (selectedArticlesListsRepository == null)
                throw new Exception("Article lists repository is not assigned.");

            _selectedArticlesListRepository = selectedArticlesListRepository;
            _selectedArticlesListsRepository = selectedArticlesListsRepository;

            InitializeComponent();
            LoadArticles();
        }

        public async Task LoadArticles()
        {
            Items = await _selectedArticlesListRepository.LoadFirstPage();
        }

        private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            if (e.Item is IArticleEntry articleEntry)
            { 
                await Navigation.PushAsync(new ArticleInfo.ArticleInfo(articleEntry));
            }

            // Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        public async void Handle_ToolbarItemClicked(object sender, EventArgs e)
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
            item.Clicked += Handle_ToolbarItemClicked;

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

        private void RemoveArticleFromList(IArticleEntry articleEntry)
        {
            if (articleEntry == null)
                throw new Exception("Article entry is empty.");

            _selectedArticlesListRepository.DeleteArticle(articleEntry.Id);
            _selectedArticlesListsRepository.ReplaceArticleListElement(_selectedArticlesListRepository.Root);
        }
    }
}
