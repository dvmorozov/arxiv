// ****************************************************************************
//    File "LikedListButton.cs"
//    Copyright © Dmitry Morozov 2021
// ****************************************************************************

using ArxivExpress.Features.LikedArticles;
using ArxivExpress.Features.SearchArticles;

namespace ArxivExpress.Features.SelectedArticles.Forms
{
    public class LikedListButton : StyledButton
    {
        public LikedListButton() : base("icons8_heart_32")
        {
            Clicked += Handle_Pressed;
        }

        public void Handle_Pressed(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(
                new ArticleList(
                    LikedArticlesRepository.GetInstance(), "Liked",
                    new StyledButton[]
                    {
                        new SearchButton(),
                        new RecentlyViewedButton(),
                        new AuthorListButton(),
                        new SelectedArticlesListsButton()
                    }
                ));
        }
    }
}
