// ****************************************************************************
//    File "ArticleList.xaml.cs"
//    Copyright © Dmitry Morozov 2021
// ****************************************************************************

using System;
using System.Collections.ObjectModel;
using ArxivExpress.Features.Data;
using ArxivExpress.Features.LikedArticles;
using ArxivExpress.Features.RecentlyViewedArticles.Data;
using ArxivExpress.Features.SelectedArticles.Forms;
using ArxivExpress.Features.ViewedAuthors.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArxivExpress.Features.SearchArticles
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArticleList : ContentPage
    {
        private IListRepository<IArticleEntry> _articleRepository;

        public ArticleList(IListRepository<IArticleEntry> articleRepository, string title)
        {
            _articleRepository = articleRepository;

            InitializeComponent();
            Title = title;
        }

        public async void LoadArticles()
        {
            try
            {
                Items = await _articleRepository.LoadFirstPage();
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", e.Message, "Ok");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadArticles();
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

        public async void Handle_ToolbarItemClicked(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
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

            if (!_articleRepository.IsLastPage())
            {
                _toolbarItemNextPage = CreateToolbarItem(_articleRepository.GetPageNumber() + 1);
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

        public void Handle_SearchPressed(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new SearchAttributes());
        }

        public void Handle_RecentlyViewedPressed(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ArticleList(ViewedArticlesRepository.GetInstance(), "History"));
        }

        public void Handle_LikedArticlesPressed(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ArticleList(LikedArticlesRepository.GetInstance(), "Liked"));
        }

        public void Handle_ViewedAuthorsPressed(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AuthorList());
        }

        public void Handle_ListsPressed(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new SelectedArticlesLists());
        }
    }
}
