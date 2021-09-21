using System;
using System.Threading.Tasks;
using ArxivExpress.Features.SearchArticles;

namespace ArxivExpress.Features.SelectedArticles.Forms
{
    public class AddToArticleListButton : StyledButton
    {
        public AddToArticleListButton(IArticleEntry articleEntry)
        {
            BindingContext = articleEntry;

            Clicked += Handle_Pressed;
            Text = "Add to List";
        }

        private void Handle_Pressed(object sender, EventArgs e)
        {
            if (sender == this)
            {
                AddArticleToListAsync();
            }
        }

        private async Task AddArticleToListAsync()
        {
            if (BindingContext is IArticleEntry articleEntry)
            {
                await Navigation.PushAsync(new SelectedArticlesLists(articleEntry));
            }
        }
    }
}
