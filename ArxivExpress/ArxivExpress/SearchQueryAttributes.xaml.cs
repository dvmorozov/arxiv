using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ArxivExpress
{
    public partial class SearchQueryAttributes : ContentPage
    {
        public SearchQueryAttributes()
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

        public IList<FieldPrefix> FieldPrefixes
        {
            get
            {
                return _fieldPrefixes;
            }
        }

        private List<FieldPrefix> _fieldPrefixes = new List<FieldPrefix>()
        {
            new FieldPrefix() { Prefix = "ti", Explanation = "Title" },
            new FieldPrefix() { Prefix = "au", Explanation = "Author" },
            new FieldPrefix() { Prefix = "abs", Explanation = "Abstract" },
            new FieldPrefix() { Prefix = "co", Explanation = "Comment" },
            new FieldPrefix() { Prefix = "jr", Explanation = "Journal Reference" },
            new FieldPrefix() { Prefix = "cat", Explanation = "Subject Category" },
            new FieldPrefix() { Prefix = "rn", Explanation = "Report Number" },
            new FieldPrefix() { Prefix = "all", Explanation = "All of the above" }
        };

        private void FillFormData()
        {
            CheckBoxComputerScience.IsChecked = _searchQuery.ComputerScience;
            CheckBoxPhysics.IsChecked = _searchQuery.Physics;
            CheckBoxEconimics.IsChecked = _searchQuery.Economics;
            CheckBoxQuantitativeBiology.IsChecked = _searchQuery.QuantitativeBiology;
            CheckBoxElectricalEngineering.IsChecked = _searchQuery.ElectricalEngineering;
            CheckBoxQuantitativeFinance.IsChecked = _searchQuery.QuantitativeFinance;
            CheckBoxMathematics.IsChecked = _searchQuery.Mathematics;
            CheckBoxStatistics.IsChecked = _searchQuery.Statistics;

            RadioIncludeCrossListedPapers.IsChecked = _searchQuery.IncludeCrossListedPapers;
            RadioExcludeCrossListedPapers.IsChecked = _searchQuery.ExcludeCrossListedPapers;

            RadioAllDates.IsChecked = _searchQuery.AllDates;
            RadioPast12Months.IsChecked = _searchQuery.Past12Months;
            RadioSpecificYear.IsChecked = _searchQuery.SpecificYear;
            RadioDateRange.IsChecked = _searchQuery.DateRange;

            RadioSubmissionDateMostRecent.IsChecked = _searchQuery.SubmissionDateMostRecent;
            RadioSubmissionDateOriginal.IsChecked = _searchQuery.SubmissionDateOriginal;
            RadioAnnouncementDate.IsChecked = _searchQuery.AnnouncementDate;

            RadioShowAbstracts.IsChecked = _searchQuery.ShowAbstracts;
            RadioHideAbstracts.IsChecked = _searchQuery.HideAbstracts;

            CheckBoxIncludeOlderVersions.IsChecked = _searchQuery.IncludeOlderVersions;

            if (PickerItemType.Items.Count != 0)
                PickerItemType.SelectedIndex = 0;
            if (PickerPhysicsSubdivision.Items.Count != 0)
                PickerPhysicsSubdivision.SelectedIndex = 0;
            if (PickerResultsPerPage.Items.Count > 1)
                PickerResultsPerPage.SelectedIndex = 1;

            EntrySearchTerm.Text = "";
            EntryDateFrom.Text = "";
            EntryDateTo.Text = "";
            EntrySpecificYear.Text = "";
        }

        private void Handle_OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender == CheckBoxComputerScience)
            {
                _searchQuery.ComputerScience = e.Value;
            }
            else if (sender == CheckBoxPhysics)
            {
                _searchQuery.Physics = e.Value;
            }
            else if (sender == CheckBoxEconimics)
            {
                _searchQuery.Economics = e.Value;
            }
            else if (sender == CheckBoxQuantitativeBiology)
            {
                _searchQuery.QuantitativeBiology = e.Value;
            }
            else if (sender == CheckBoxElectricalEngineering)
            {
                _searchQuery.ElectricalEngineering = e.Value;
            }
            else if (sender == CheckBoxQuantitativeFinance)
            {
                _searchQuery.QuantitativeFinance = e.Value;
            }
            else if (sender == CheckBoxMathematics)
            {
                _searchQuery.Mathematics = e.Value;
            }
            else if (sender == CheckBoxStatistics)
            {
                _searchQuery.Statistics = e.Value;
            }
            else if (sender == CheckBoxIncludeOlderVersions)
            {
                _searchQuery.IncludeOlderVersions = e.Value;
            }
        }

        private void Handle_OnRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender == RadioIncludeCrossListedPapers)
            {
                _searchQuery.IncludeCrossListedPapers = e.Value;
            }
            else if (sender == RadioExcludeCrossListedPapers)
            {
                _searchQuery.ExcludeCrossListedPapers = e.Value;
            }
            else if (sender == RadioAllDates)
            {
                _searchQuery.AllDates = e.Value;
            }
            else if (sender == RadioPast12Months)
            {
                _searchQuery.Past12Months = e.Value;
            }
            else if (sender == RadioSpecificYear)
            {
                _searchQuery.SpecificYear = e.Value;
            }
            else if (sender == RadioDateRange)
            {
                _searchQuery.DateRange = e.Value;
            }
            else if (sender == RadioSubmissionDateMostRecent)
            {
                _searchQuery.SubmissionDateMostRecent = e.Value;
            }
            else if (sender == RadioSubmissionDateOriginal)
            {
                _searchQuery.SubmissionDateOriginal = e.Value;
            }
            else if (sender == RadioAnnouncementDate)
            {
                _searchQuery.AnnouncementDate = e.Value;
            }
            else if (sender == RadioShowAbstracts)
            {
                _searchQuery.ShowAbstracts = e.Value;
            }
            else if (sender == RadioHideAbstracts)
            {
                _searchQuery.HideAbstracts = e.Value;
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
            else if (sender == PickerPhysicsSubdivision)
            {
                var picker = (Picker)sender;
                _searchQuery.PhysicsSubdivision = (string)picker.ItemsSource[picker.SelectedIndex];
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
            else if (sender == EntryDateFrom)
            {
                _searchQuery.DateFrom = ((Entry)sender).Text;
            }
            else if (sender == EntryDateTo)
            {
                _searchQuery.DateTo = ((Entry)sender).Text;
            }
            else if (sender == EntrySpecificYear)
            {
                _searchQuery.Year = ((Entry)sender).Text;
            }
        }

        async void Handle_SearchPressed(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ArticleList(_searchQuery));
        }

        private ArticleList.SearchQuery _searchQuery;
    }
}
