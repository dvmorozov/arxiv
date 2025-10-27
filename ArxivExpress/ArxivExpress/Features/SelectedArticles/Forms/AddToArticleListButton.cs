// ****************************************************************************
//    File "AddToArticleListButton.cs"
//    Copyright © Dmitry Morozov 2022
//    If you want to use this file please contact me by dvmorozov@hotmail.com.
// ****************************************************************************

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

        private void Handle_Pressed(object sender, EventArgs e)
        {
            if (sender == this)
            {
                AddArticleToListAsync();
            }
        }

        private void AddArticleToListAsync()
        {
            Navigation.PushAsync(new SelectedArticlesLists(_articleEntry));
        }
    }
}
