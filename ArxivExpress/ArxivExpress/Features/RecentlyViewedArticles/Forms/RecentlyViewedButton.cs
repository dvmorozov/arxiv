// ****************************************************************************
//    File "RecentlyViewedButton.cs"
//    Copyright © Dmitry Morozov 2021
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
            Navigation.PushAsync(new ArticleList(ViewedArticlesRepository.GetInstance(), "History"));
        }
    }
}
