// ****************************************************************************
//    File "SearchButton.cs"
//    Copyright © Dmitry Morozov 2021
// ****************************************************************************

using ArxivExpress.Features.SearchArticles;

namespace ArxivExpress.Features.SelectedArticles.Forms
{
    public class SearchButton : StyledButton
    {
        public SearchButton() : base("icons8_search_32")
        {
            Clicked += Handle_Pressed;
        }

        public void Handle_Pressed(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new SearchAttributes());
        }
    }
}
