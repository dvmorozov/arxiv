using System;
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
                CreateArticleList();
            }
        }

        private void CreateArticleList()
        {

        }
    }
}
