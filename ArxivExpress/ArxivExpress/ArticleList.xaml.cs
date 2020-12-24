using System.Collections.ObjectModel;
using Microsoft.SyndicationFeed;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArxivExpress
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArticleList : ContentPage
    {
        private ObservableCollection<EntryAdapter> _items { get; set; }
        private AtomFeedProcessor _atomFeedProcessor;
        private SearchQuery _searchQuery;

        public ArticleList(SearchQuery searchQuery)
        {
            _atomFeedProcessor = new AtomFeedProcessor(this);
            _searchQuery = searchQuery;
            _items = new ObservableCollection<EntryAdapter>();

            InitializeComponent();
            MakeRequest();

            MyListView.ItemsSource = _items;
        }

        public async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        public async void MakeRequest()
        {
            await AtomFeedRequest.MakeRequest(
                _searchQuery.GetQueryString(), _atomFeedProcessor);
        }

        public class SearchQuery
        {
            public string SearchTerm;
            public string DateFrom;
            public string DateTo;
            public string Year;

            public bool ComputerScience;
            public bool Physics;
            public bool Economics;
            public bool QuantitativeBiology;
            public bool ElectricalEngineering;
            public bool QuantitativeFinance;
            public bool Mathematics;
            public bool Statistics;

            //  Mutually exclusive.
            public bool IncludeCrossListedPapers;
            public bool ExcludeCrossListedPapers;

            //  Mutually exclusive.
            public bool AllDates;
            public bool Past12Months;
            public bool SpecificYear;
            public bool DateRange;

            //  Mutually exclusive.
            public bool SubmissionDateMostRecent;
            public bool SubmissionDateOriginal;
            public bool AnnouncementDate;

            //  Mutually exclusive.
            public bool ShowAbstracts;
            public bool HideAbstracts;

            public bool IncludeOlderVersions;

            public string ItemType;
            public string PhysicsSubdivision;
            public string ResultsPerPage;

            private uint _pageNumber;

            public SearchQuery()
            {
                FillDefaultValues();
            }

            private void FillDefaultValues()
            {
                SearchTerm = "";
                DateFrom = "";
                DateTo = "";
                Year = "";

                ComputerScience = true;
                Physics = true;
                Economics = true;
                QuantitativeBiology = true;
                ElectricalEngineering = true;
                QuantitativeFinance = true;
                Mathematics = true;
                Statistics = true;

                //  Mutually exclusive.
                IncludeCrossListedPapers = true;
                ExcludeCrossListedPapers = false;

                //  Mutually exclusive.
                AllDates = true;
                Past12Months = false;
                SpecificYear = false;
                DateRange = false;

                //  Mutually exclusive.
                SubmissionDateMostRecent = true;
                SubmissionDateOriginal = false;
                AnnouncementDate = false;

                //  Mutually exclusive.
                ShowAbstracts = true;
                HideAbstracts = false;

                IncludeOlderVersions = false;

                ItemType = "Title";
                PhysicsSubdivision = "all";
                ResultsPerPage = "20";

                _pageNumber = 0;
            }

            public string GetQueryString()
            {
                var queryString = "http://export.arxiv.org/api/query?search_query=";

                queryString += PhysicsSubdivision;
                if (SearchTerm != null && SearchTerm != string.Empty)
                {
                    queryString += ":\"" + SearchTerm + "\"";
                }
                queryString += "&start=" + _pageNumber.ToString();
                queryString += "&max_results=" + ResultsPerPage;

                return queryString;
            }
        }

        public class EntryAdapter
        {
            private IAtomEntry _entry;

            public EntryAdapter(IAtomEntry entry)
            {
                _entry = entry;
            }

            public string Summary
            {
                get { return MakePlainString(_entry.Summary); }
            }

            public string Title
            {
                get { return MakePlainString(_entry.Title); }
            }

            private string MakePlainString(string original)
            {
                string result = original;
                result = result.Trim(new char[] { ' ', '\t', '\n' });
                result = result.Replace('\n', ' ');
                result = result.Replace('\t', ' ');

                var length = result.Length;
                while(true)
                {
                    result = result.Replace("  ", " ");
                    if (result.Length < length)
                    {
                        length = result.Length;
                        continue;
                    }
                    break;
                }
                
                return result;
            }
        }

        private class AtomFeedProcessor : AtomFeedRequest.IAtomFeedProcessor
        {
            private ArticleList _articleList;

            public AtomFeedProcessor(ArticleList articleList)
            {
                _articleList = articleList;
            }

            void AtomFeedRequest.IAtomFeedProcessor.ProcessCategory(ISyndicationCategory category)
            {
            }

            void AtomFeedRequest.IAtomFeedProcessor.ProcessImage(ISyndicationImage image)
            {
            }

            void AtomFeedRequest.IAtomFeedProcessor.ProcessEntry(IAtomEntry entry)
            {
                if (entry != null)
                    _articleList._items.Add(new EntryAdapter(entry));
            }

            void AtomFeedRequest.IAtomFeedProcessor.ProcessLink(ISyndicationLink link)
            {
            }

            void AtomFeedRequest.IAtomFeedProcessor.ProcessPerson(ISyndicationPerson person)
            {
            }

            void AtomFeedRequest.IAtomFeedProcessor.ProcessContent(ISyndicationContent content)
            {
            }
        }
    }
}
