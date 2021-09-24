using System;
using ArxivExpress.Features.SelectedArticles.Data;
using ArxivExpress.Features.SelectedArticles.Model;
using Xamarin.Forms;

namespace ArxivExpress.Features.SelectedArticles.Forms
{
    public partial class NewList : ContentPage
    {
        public NewList()
        {
            InitializeComponent();
        }

        private string _listName;

        private void Handle_EntryCompleted(object sender, EventArgs e)
        {
            if (sender == EntryListName)
            {
                _listName = ((Entry)sender).Text;
            }
        }

        private void Handle_TextChanged(object sender, EventArgs e)
        {
            if (sender == EntryListName)
            {
                _listName = ((Entry)sender).Text;
            }
        }

        private async void Handle_CreateNewListPressed(object sender, EventArgs e)
        {
            var selectedArticlesListsRepository = SelectedArticlesListsRepository.GetInstance();
            selectedArticlesListsRepository.AddList(_listName);

            await Navigation.PushAsync(new SelectedArticlesLists());
        }
    }
}
