// ****************************************************************************
//    File "AuthorListButton.cs"
//    Copyright © Dmitry Morozov 2021
// ****************************************************************************

using ArxivExpress.Features.ViewedAuthors.Forms;

namespace ArxivExpress.Features.SelectedArticles.Forms
{
    public class AuthorListButton : StyledButton
    {
        public AuthorListButton() : base("icons8_contacts_32")
        {
            Clicked += Handle_Pressed;
        }

        public void Handle_Pressed(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AuthorList());
        }
    }
}
