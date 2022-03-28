// ****************************************************************************
//    File "NewList.xaml.cs"
//    Copyright © Dmitry Morozov 2022
//    If you want to use this file please contact me by dvmorozov@hotmail.com.
// ****************************************************************************

using System;
using ArxivExpress.Features.SearchArticles;
using ArxivExpress.Features.SelectedArticles.Data;
using Xamarin.Forms;

namespace ArxivExpress.Features.SelectedArticles.Forms
{
    public partial class NewList : ContentPage
    {
        private IArticleEntry _articleEntry;

        public NewList(IArticleEntry articleEntry)
        {
            _articleEntry = articleEntry ??
                throw new Exception("Article entry is not assigned.");

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

        private void Handle_CreateNewListPressed(object sender, EventArgs e)
        {
            var selectedArticlesListsRepository = SelectedArticlesListsRepository.GetInstance();
            selectedArticlesListsRepository.AddList(_listName);

            var selectedArticlesListsHelper = new SelectedArticlesListsHelper(Navigation);
            selectedArticlesListsHelper.AddArticleToList(_listName, _articleEntry);
        }
    }
}
