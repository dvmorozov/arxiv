using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ArxivExpress.Features.SearchArticles
{
    public partial class SearchAttributes : ContentPage
    {
        public SearchAttributes()
        {
            _searchArticleRepository = SearchArticlesRepository.GetInstance();

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
            RadioSortByRelevance.IsChecked =
                _searchArticleRepository.SearchQuery.SortByRelevance;
            RadioSortByLastUpdatedDate.IsChecked =
                _searchArticleRepository.SearchQuery.SortByLastUpdatedDate;
            RadioSortBySubmittedDate.IsChecked =
                _searchArticleRepository.SearchQuery.SortBySubmittedDate;

            RadioSortOrderAscending.IsChecked =
                _searchArticleRepository.SearchQuery.SortOrderAscending;
            RadioSortOrderDescending.IsChecked =
                _searchArticleRepository.SearchQuery.SortOrderDescending;

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
                _searchArticleRepository.SearchQuery.SortByRelevance = e.Value;
            }
            else if (sender == RadioSortByLastUpdatedDate)
            {
                _searchArticleRepository.SearchQuery.SortByLastUpdatedDate = e.Value;
            }
            else if (sender == RadioSortBySubmittedDate)
            {
                _searchArticleRepository.SearchQuery.SortBySubmittedDate = e.Value;
            }
            else if (sender == RadioSortOrderAscending)
            {
                _searchArticleRepository.SearchQuery.SortOrderAscending = e.Value;
            }
            else if (sender == RadioSortOrderDescending)
            {
                _searchArticleRepository.SearchQuery.SortOrderDescending = e.Value;
            }
        }

        private void Hangle_OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender == PickerItemType)
            {
                var picker = (Picker)sender;
                FieldPrefix item = (FieldPrefix)picker.ItemsSource[picker.SelectedIndex];
                _searchArticleRepository.SearchQuery.Prefix = item.Prefix;
            }
            else if (sender == PickerResultsPerPage)
            {
                var picker = (Picker)sender;
                _searchArticleRepository.SearchQuery.ResultsPerPage =
                    (string)picker.ItemsSource[picker.SelectedIndex];
            }
        }

        private void Handle_EntryCompleted(object sender, EventArgs e)
        {
            if (sender == EntrySearchTerm)
            {
                _searchArticleRepository.SearchQuery.SearchTerm = ((Entry)sender).Text;
            }
        }

        private async void Handle_SearchPressed(object sender, EventArgs e)
        {
            var articleList = new ArticleList(_searchArticleRepository);
            await Navigation.PushAsync(articleList);
        }

        private SearchArticlesRepository _searchArticleRepository;
    }
}
