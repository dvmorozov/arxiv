using System;
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

            ArticleListView.ItemsSource = _items;
        }

        public async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        public void Handle_ToolbarItemClicked(object sender, EventArgs e)
        {
            ToolbarItem item = (ToolbarItem)sender;
            //messageLabel.Text = $"You clicked the \"{item.Text}\" toolbar item.";
        }

        public async void MakeRequest()
        {
            await AtomFeedRequest.MakeRequest(
                _searchQuery.GetQueryString(), _atomFeedProcessor);
        }

        public class SearchQuery
        {
            public string SearchTerm;
            public string Prefix;
            public string ResultsPerPage;

            //  Mutually exclusive.
            public bool SortByRelevance;
            public bool SortByLastUpdatedDate;
            public bool SortBySubmittedDate;

            //  Mutually exclusive.
            public bool SortOrderAscending;
            public bool SortOrderDescending;

            private uint _pageNumber;

            public SearchQuery()
            {
                FillDefaultValues();
            }

            private void FillDefaultValues()
            {
                SearchTerm = "";
                ResultsPerPage = "20";

                SortByRelevance = true;
                SortByLastUpdatedDate = false;
                SortBySubmittedDate = false;

                SortOrderAscending = true;
                SortOrderDescending = false;

                _pageNumber = 0;
            }

            private string GetQuerySortBy()
            {
                if (SortByRelevance)
                {
                    return "relevance";
                }
                else if (SortByLastUpdatedDate)
                {
                    return "lastUpdatedDate";
                }
                else //if (SortBySubmittedDate)
                {
                    return "submittedDate";
                }
            }

            private string GetQuerySortOrder()
            {
                if (SortOrderAscending)
                {
                    return "ascending";
                }
                else //if (SortOrderDescending)
                {
                    return "descending";
                }
            }

            public string GetQueryString()
            {
                var queryString = "http://export.arxiv.org/api/query?search_query=";

                if (Prefix != null && Prefix != string.Empty)
                {
                    queryString += Prefix;
                }
                else
                {
                    queryString += "all";
                }

                if (SearchTerm != null && SearchTerm != string.Empty)
                {
                    queryString += ":\"" + SearchTerm + "\"";
                }

                queryString += "&start=" + _pageNumber.ToString();
                queryString += "&max_results=" + ResultsPerPage;
                queryString += "&sortBy=" + GetQuerySortBy();
                queryString += "&sortOrder=" + GetQuerySortOrder();

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
