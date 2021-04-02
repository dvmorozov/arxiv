using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ArxivExpress.Features.Data;

namespace ArxivExpress.Features.SearchArticles
{
    public class SearchArticlesRepository : IListRepository<IArticleEntry>
    {
        private static SearchArticlesRepository _instance;
        public SearchQuery SearchQuery;

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

        public async Task<ObservableCollection<IArticleEntry>> LoadFirstPage()
        {
            var atomFeedProcessor = new AtomFeedProcessor();

            await AtomFeedRequest.MakeRequest(
                SearchQuery.GetQueryString(), atomFeedProcessor);

            var result = new ObservableCollection<IArticleEntry>();

            foreach (var article in atomFeedProcessor.Items)
            {
                result.Add(article);
            }

            _isLastPage = result.Count < GetResultsPerPage();

            return result;
        }

        public async Task<ObservableCollection<IArticleEntry>> LoadNextPage()
        {
            SearchQuery.PageNumber++;
            return await LoadFirstPage();
        }

        public async Task<ObservableCollection<IArticleEntry>> LoadPrevPage()
        {
            if (SearchQuery.PageNumber > 0)
            {
                SearchQuery.PageNumber--;
            }
            return await LoadFirstPage();
        }

        public uint GetPageNumber()
        {
            return SearchQuery.PageNumber;
        }

        public uint GetResultsPerPage()
        {
            return SearchQuery.GetResultsPerPage();
        }

        private bool _isLastPage = false;

        public bool IsLastPage()
        {
            return _isLastPage;
        }
    }
}
