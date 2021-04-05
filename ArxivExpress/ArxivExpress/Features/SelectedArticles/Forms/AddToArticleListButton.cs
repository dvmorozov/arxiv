using System;
using ArxivExpress.Features.SearchArticles;

namespace ArxivExpress.Features.SelectedArticles.Forms
{
    public class AddToArticleListButton : StyledButton
    {
        public AddToArticleListButton(IArticleEntry articleEntry)
        {
            Clicked += Handle_Pressed;

            Text = "Add to List";
        }

        private void Handle_Pressed(object sender, EventArgs e)
        {
            if (sender == this)
            {
                AddArticleToList();
            }
        }

        private void AddArticleToList()
        {

        }
    }
}
