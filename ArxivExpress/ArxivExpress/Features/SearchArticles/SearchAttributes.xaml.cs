using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ArxivExpress
{
    public partial class SearchAttributes : ContentPage
    {
        public SearchAttributes()
        {
            _searchQuery = new ArticleList.SearchQuery();
            _fieldPrefixes = new List<FieldPrefix>()
            {
                new FieldPrefix() { Prefix = "ti", Explanation = "Title" },
                new FieldPrefix() { Prefix = "au", Explanation = "Author" },
                new FieldPrefix() { Prefix = "abs", Explanation = "Abstract" },
                new FieldPrefix() { Prefix = "co", Explanation = "Comment" },
                new FieldPrefix() { Prefix = "jr", Explanation = "Journal Reference" },
                new FieldPrefix() { Prefix = "cat", Explanation = "Subject Category" },
                new FieldPrefix() { Prefix = "rn", Explanation = "Report Number" },
                new FieldPrefix() { Prefix = "all", Explanation = "All Fields" }
            };

            BindingContext = this;

            InitializeComponent();
            FillFormData();
        }

        public class FieldPrefix
        {
            public string Prefix
            {
                get
                {
                    return _prefix;
                }

                set
                {
                    _prefix = value;
                }
            }

            public string Explanation
            {
                get
                {
                    return _explanation;
                }

                set
                {
                    _explanation = value;
                }
            }

            private string _prefix;
            private string _explanation;
        }

        public IList<FieldPrefix> FieldPrefixes => _fieldPrefixes;

        private readonly List<FieldPrefix> _fieldPrefixes;

        private void FillFormData()
        {
            RadioSortByRelevance.IsChecked = _searchQuery.SortByRelevance;
            RadioSortByLastUpdatedDate.IsChecked = _searchQuery.SortByLastUpdatedDate;
            RadioSortBySubmittedDate.IsChecked = _searchQuery.SortBySubmittedDate;

            RadioSortOrderAscending.IsChecked = _searchQuery.SortOrderAscending;
            RadioSortOrderDescending.IsChecked = _searchQuery.SortOrderDescending;

            if (PickerItemType.Items.Count != 0)
                PickerItemType.SelectedIndex = 0;
            if (PickerResultsPerPage.Items.Count > 1)
                PickerResultsPerPage.SelectedIndex = 1;

            EntrySearchTerm.Text = "";
        }

        private void Handle_OnRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender == RadioSortByRelevance)
            {
               _searchQuery.SortByRelevance = e.Value;
            }
            else if (sender == RadioSortByLastUpdatedDate)
            {
                _searchQuery.SortByLastUpdatedDate = e.Value;
            }
            else if (sender == RadioSortBySubmittedDate)
            {
                _searchQuery.SortBySubmittedDate = e.Value;
            }
            else if (sender == RadioSortOrderAscending)
            {
                _searchQuery.SortOrderAscending = e.Value;
            }
            else if (sender == RadioSortOrderDescending)
            {
                _searchQuery.SortOrderDescending = e.Value;
            }
        }

        private void Hangle_OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender == PickerItemType)
            {
                var picker = (Picker)sender;
                FieldPrefix item = (FieldPrefix)picker.ItemsSource[picker.SelectedIndex];
                _searchQuery.Prefix = item.Prefix;
            }
            else if (sender == PickerResultsPerPage)
            {
                var picker = (Picker)sender;
                _searchQuery.ResultsPerPage = (string)picker.ItemsSource[picker.SelectedIndex];
            }
        }

        private void Handle_EntryCompleted(object sender, EventArgs e)
        {
            if (sender == EntrySearchTerm)
            {
                _searchQuery.SearchTerm = ((Entry)sender).Text;
            }
        }

        async void Handle_SearchPressed(object sender, EventArgs e)
        {
            var articleList = new ArticleList(_searchQuery);
            await Navigation.PushAsync(articleList);
            await articleList.MakeRequest();
        }

        private ArticleList.SearchQuery _searchQuery;
    }
}
