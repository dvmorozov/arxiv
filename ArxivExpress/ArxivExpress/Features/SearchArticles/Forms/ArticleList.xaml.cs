// ****************************************************************************
//    File "ArticleList.xaml.cs"
//    Copyright © Dmitry Morozov 2021
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ArxivExpress.Features.Data;
using ArxivExpress.Features.RecentlyViewedArticles.Data;
using ArxivExpress.Features.SelectedArticles.Forms;
using ArxivExpress.Features.ViewedAuthors.Forms;
using ArxivExpress.Forms;
using Microsoft.AppCenter.Crashes;
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

        public static async Task DisplayAlert(Exception e, Page page)
        {
            if (page == null)
                throw new Exception("Page is not assigned.");
            if (e == null)
                throw new Exception("Exception object is not assigned.");

            if (await page.DisplayAlert("Some error occured",
                "Would you like to send information about error \"" + e.Message +
                "\" to developers? No personal information is sent.",
                "Yes", "No"))
            {
                var properties = new Dictionary<string, string>
                {
                    {"Message", e.Message},
                    {"Source", e.Source },
                    {"InnerException", e.InnerException?.Message}
                };
                var errorAttachmentLog = ErrorAttachmentLog.AttachmentWithText(e.StackTrace, e.Source);
                Crashes.TrackError(e, properties, errorAttachmentLog);
            }
        }

        public async void LoadArticles()
        {
            try
            {
                Items = await _articleRepository.LoadFirstPage();
            }
            catch (Exception e)
            {
                await DisplayAlert(e, this);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            LoadArticles();
            DisplayButtons();
        }

        private void DisplayButtons()
        {
            FlexLayoutToolbar.Children.Add(
                new SearchButton()
                );

            FlexLayoutToolbar.Children.Add(
                new RecentlyViewedButton()
            );

            FlexLayoutToolbar.Children.Add(
                new AuthorListButton()
                );

            FlexLayoutToolbar.Children.Add(
                new SelectedArticlesListsButton()
                );
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
                    Items = await _articleRepository.LoadNextPage();
                }
                else if (item == _toolbarItemPrevPage)
                {
                    Items = await _articleRepository.LoadPrevPage();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex, this);
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

        private ToolbarItem CreateToolbarNavigationItem(uint pageNumber)
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

            if (_articleRepository.GetPageNumber() > 0)
            {
                _toolbarItemPrevPage = CreateToolbarNavigationItem(_articleRepository.GetPageNumber() - 1);
                ToolbarItems.Insert(0, _toolbarItemPrevPage);
            }

            if (!_articleRepository.IsLastPage())
            {
                _toolbarItemNextPage = CreateToolbarNavigationItem(_articleRepository.GetPageNumber() + 1);
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
    }
}
