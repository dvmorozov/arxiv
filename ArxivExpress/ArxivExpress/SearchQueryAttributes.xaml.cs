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
        }

        public void Handle_OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
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

        async void Handle_SearchPressed(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ArticleList());
        }

        private ArticleList.SearchQuery _searchQuery;
    }
}
