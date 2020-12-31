using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.SyndicationFeed;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArxivExpress
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArticleList : ContentPage
    {
        private ObservableCollection<ArticleEntry> _items { get; set; }
        private AtomFeedProcessor _atomFeedProcessor;
        private SearchQuery _searchQuery;

        public ArticleList(SearchQuery searchQuery)
        {
            _atomFeedProcessor = new AtomFeedProcessor(this);
            _searchQuery = searchQuery;
            _items = new ObservableCollection<ArticleEntry>();

            InitializeComponent();
            MakeRequest();

            ArticleListView.ItemsSource = _items;
        }

        public async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await Navigation.PushAsync(new ArticleInfo((ArticleEntry)e.Item));

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        public void Handle_ToolbarItemClicked(object sender, EventArgs e)
        {
            ToolbarItem item = (ToolbarItem)sender;
            if (item == ToolbarItemNextPage)
            {
                _searchQuery.PageNumber++;
                MakeRequest();
            }
            else if (item == _toolbarItemPrevPage)
            {
                if (_searchQuery.PageNumber > 0)
                {
                    _searchQuery.PageNumber--;
                    MakeRequest();
                }
            }
        }

        private string GetItemsRange(uint pageIndex)
        {
            var resultsPerPage = _searchQuery.GetResultsPerPage();
            var startIndex = pageIndex * resultsPerPage + 1;
            var endIndex = (pageIndex + 1) * resultsPerPage;

            return startIndex.ToString() + "-" + endIndex.ToString();
        }

        private ToolbarItem _toolbarItemPrevPage;

        private void DeleteToolbarItemPate()
        {
            if (_toolbarItemPrevPage != null)
            {
                this.ToolbarItems.Remove(_toolbarItemPrevPage);
            }
        }

        private void SetToolbarPageNavigationItems()
        {
            DeleteToolbarItemPate();

            if (_searchQuery.PageNumber > 0)
            {
                var item = new ToolbarItem
                {
                    Text = GetItemsRange(_searchQuery.PageNumber - 1),
                    Order = ToolbarItemOrder.Primary,
                    Priority = 0                    
                };
                item.Clicked += Handle_ToolbarItemClicked;

                this.ToolbarItems.Insert(0, item);
                _toolbarItemPrevPage = item;
            }
 
            ToolbarItemNextPage.Text = GetItemsRange(_searchQuery.PageNumber + 1);
        }

        public async void MakeRequest()
        {
            _items.Clear();

            await AtomFeedRequest.MakeRequest(
                _searchQuery.GetQueryString(), _atomFeedProcessor);

            SetToolbarPageNavigationItems();
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

            public uint PageNumber;

            public SearchQuery()
            {
                FillDefaultValues();
            }

            private void FillDefaultValues()
            {
                SearchTerm = "";
                ResultsPerPage = "25";

                SortByRelevance = false;
                SortByLastUpdatedDate = false;
                SortBySubmittedDate = true;

                SortOrderAscending = false;
                SortOrderDescending = true;

                PageNumber = 0;
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

            public uint GetResultsPerPage()
            {
                if (!uint.TryParse(ResultsPerPage, out uint resultsPerPage))
                {
                    resultsPerPage = 25;
                }

                return resultsPerPage;
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

                var resultsPerPage = GetResultsPerPage();

                queryString += "&start=" + (PageNumber * resultsPerPage).ToString();
                queryString += "&max_results=" + resultsPerPage.ToString();
                queryString += "&sortBy=" + GetQuerySortBy();
                queryString += "&sortOrder=" + GetQuerySortOrder();

                return queryString;
            }
        }

        public class Contributor
        {
            public string Email { get; }
            public string Name { get; }

            public Contributor(string name, string email)
            {
                Name = name;
                Email = email;
            }
        }

        public class ArticleEntry
        {
            private IAtomEntry _entry;

            public ArticleEntry(IAtomEntry entry)
            {
                _entry = entry;
            }

            public IList<string> Categories
            {
                get
                {
                    List<string> result = new List<string>();

                    foreach (var category in _entry.Categories)
                    {
                        result.Add(category.Name);
                    }

                    return result;
                }
            }

            public IList<Contributor> Contributors
            {
                get
                {
                    List<Contributor> result = new List<Contributor>();

                    foreach (var contributor in _entry.Contributors)
                    {
                        result.Add(new Contributor(
                            contributor.Name, contributor.Email)
                        );
                    }

                    return result;
                }
            }

            public string Summary
            {
                get { return MakePlainString(_entry.Summary); }
            }

            public string Title
            {
                get { return MakePlainString(_entry.Title); }
            }

            public string Published
            {
                get { return
                        _entry.Published != null ?
                        "Published: " + _entry.Published.Date.ToShortDateString() :
                        "";
                }
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
                    _articleList._items.Add(new ArticleEntry(entry));
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
