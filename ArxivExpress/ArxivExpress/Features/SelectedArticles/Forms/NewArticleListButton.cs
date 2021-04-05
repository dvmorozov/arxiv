using System;
using System.Threading.Tasks;

namespace ArxivExpress.Features.SelectedArticles.Forms
{
    public class NewArticleListButton : StyledButton
    {
        public NewArticleListButton()
        {
            Clicked += Handle_Pressed;

            Text = "New List";
        }

        private void Handle_Pressed(object sender, EventArgs e)
        {
            if (sender == this)
            {
                CreateArticleListAsync();
            }
        }

        private async Task CreateArticleListAsync()
        {
            await Navigation.PushAsync(new NewList());
        }
    }
}
