using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ArxivExpress.Features.ArticleList;

namespace ArxivExpress.Features.SearchArticles
{
    public class SearchArticlesRepository : IArticlesRepository
    {
        private static SearchArticlesRepository _instance;
        public readonly SearchQuery SearchQuery;

        private SearchArticlesRepository()
        {
            SearchQuery = new SearchQuery();
        }

        public static SearchArticlesRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SearchArticlesRepository();
            }

            return _instance;
        }

        public async Task<ObservableCollection<ArticleEntry> >LoadArticles()
        {
            var atomFeedProcessor = new AtomFeedProcessor();

            await AtomFeedRequest.MakeRequest(
                SearchQuery.GetQueryString(), atomFeedProcessor);

            return atomFeedProcessor.Items;
        }

        public async Task<ObservableCollection<ArticleEntry>> LoadNextPage()
        {
            SearchQuery.PageNumber++;
            return await LoadArticles();
        }

        public async Task<ObservableCollection<ArticleEntry>> LoadPrevPage()
        {
            if (SearchQuery.PageNumber > 0)
            {
                SearchQuery.PageNumber--;
            }
            return await LoadArticles();
        }

        public uint GetPageNumber()
        {
            return SearchQuery.PageNumber;
        }

        public uint GetResultsPerPage()
        {
            return SearchQuery.GetResultsPerPage();
        }
    }
}
