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
        public ObservableCollection<IAtomEntry> Items { get; set; }
        private AtomFeedProcessor _atomFeedProcessor;
 
        public ArticleList()
        {
            _atomFeedProcessor = new AtomFeedProcessor(this);
            Items = new ObservableCollection<IAtomEntry>();

            InitializeComponent();
            MakeRequest();

            MyListView.ItemsSource = Items;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        public async void MakeRequest()
        {
            SearchQuery query = new SearchQuery();
            await AtomFeedRequest.MakeRequest(query.GetQueryString(), _atomFeedProcessor);
        }

        private class SearchQuery
        {
            public string GetQueryString()
            {
                return "http://export.arxiv.org/api/query?search_query=all:electron&start=0&max_results=10";
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
                    _articleList.Items.Add(entry);
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
