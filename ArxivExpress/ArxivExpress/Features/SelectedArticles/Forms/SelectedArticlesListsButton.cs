// ****************************************************************************
//    File "SelectedArticlesListsButton.cs"
//    Copyright © Dmitry Morozov 2021
// ****************************************************************************

namespace ArxivExpress.Features.SelectedArticles.Forms
{
    public class SelectedArticlesListsButton : StyledButton
    {
        public SelectedArticlesListsButton() : base("icons8_book_shelf_32")
        {
            Clicked += Handle_Pressed;
        }

        public void Handle_Pressed(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new SelectedArticlesLists());
        }
    }
}
