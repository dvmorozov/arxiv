using System;
using ArxivExpress.Features.ArticleList;
using Xamarin.Forms;

namespace ArxivExpress.Features.LikedArticles
{
    public partial class LikedArticleList : ContentPage
    {
        private LikedArticlesRepository _likedArticlesRepository;

        public LikedArticleList()
        {
            _likedArticlesRepository = LikedArticlesRepository.GetInstance();

            InitializeComponent();
            LikedArticleListView.ItemsSource = _likedArticlesRepository.LikedArticles;
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

        private uint _pageNumber = 0;

        public void Handle_ToolbarItemClicked(object sender, EventArgs e)
        {
            ToolbarItem item = (ToolbarItem)sender;
            if (item == _toolbarItemNextPage)
            {
                _pageNumber++;
            }
            else if (item == _toolbarItemPrevPage)
            {
                if (_pageNumber > 0)
                {
                    _pageNumber--;
                }
            }
        }

        private string GetItemsRange(uint pageIndex)
        {
            var resultsPerPage = 50;
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

            if (_pageNumber > 0)
            {
                _toolbarItemPrevPage = CreateToolbarItem(_pageNumber - 1);
                ToolbarItems.Insert(0, _toolbarItemPrevPage);
            }

            _toolbarItemNextPage = CreateToolbarItem(_pageNumber + 1);
            ToolbarItems.Add(_toolbarItemNextPage);
        }
    }
}
