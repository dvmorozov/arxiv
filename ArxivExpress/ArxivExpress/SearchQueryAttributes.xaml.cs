using System;

using Xamarin.Forms;

namespace ArxivExpress
{
    public partial class SearchQueryAttributes : ContentPage
    {
        public SearchQueryAttributes()
        {
            _searchQuery = new ArticleList.SearchQuery();

            InitializeComponent();
            FillFormData();
        }

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

        async void Handle_SearchPressed(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ArticleList());
        }

        private ArticleList.SearchQuery _searchQuery;
    }
}
