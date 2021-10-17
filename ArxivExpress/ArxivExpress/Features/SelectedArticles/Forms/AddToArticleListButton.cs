using System;
using System.Threading.Tasks;
using ArxivExpress.Features.SearchArticles;

namespace ArxivExpress.Features.SelectedArticles.Forms
{
    public class AddToArticleListButton : StyledButton
    {
        private IArticleEntry _articleEntry;

        public AddToArticleListButton(IArticleEntry articleEntry) : base("icons8_book_shelf_32")
        {
            _articleEntry = articleEntry ??
                throw new Exception("Article entry is not assigned.");

            Clicked += Handle_Pressed;
        }

        private async void Handle_Pressed(object sender, EventArgs e)
        {
            if (sender == this)
            {
                await AddArticleToListAsync();
            }
        }

        private async Task AddArticleToListAsync()
        {
            await Navigation.PushAsync(new SelectedArticlesLists(_articleEntry));
        }
    }
}
