// ****************************************************************************
//    File "RecentlyViewedButton.cs"
//    Copyright © Dmitry Morozov 2022
//    If you want to use this file please contact me by dvmorozov@hotmail.com.
// ****************************************************************************

using ArxivExpress.Features.RecentlyViewedArticles.Data;
using ArxivExpress.Features.SearchArticles;

namespace ArxivExpress.Features.SelectedArticles.Forms
{
    public class RecentlyViewedButton : StyledButton
    {
        public RecentlyViewedButton() : base("icons8_activity_history_32")
        {
            Clicked += Handle_Pressed;
        }

        public void Handle_Pressed(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(
                new ArticleList(
                    ViewedArticlesRepository.GetInstance(), "History",
                    new StyledButton[]
                    {
                        new SearchButton(),
                        new LikedListButton(),
                        new AuthorListButton(),
                        new SelectedArticlesListsButton()
                    }
                ));
        }
    }
}
