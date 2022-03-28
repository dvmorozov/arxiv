// ****************************************************************************
//    File "NewArticleListButton.cs"
//    Copyright © Dmitry Morozov 2022
//    If you want to use this file please contact me by dvmorozov@hotmail.com.
// ****************************************************************************

using System;
using System.Threading.Tasks;
using ArxivExpress.Features.SearchArticles;

namespace ArxivExpress.Features.SelectedArticles.Forms
{
    public class NewArticleListButton : StyledButton
    {
        private IArticleEntry _articleEntry;

        public NewArticleListButton(IArticleEntry articleEntry) : base("icons8_add_list_32")
        {
            _articleEntry = articleEntry ??
                throw new Exception("Article entry is not assigned.");

            Clicked += Handle_Pressed;
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
            Navigation.PushAsync(new NewList(_articleEntry));
        }
    }
}
