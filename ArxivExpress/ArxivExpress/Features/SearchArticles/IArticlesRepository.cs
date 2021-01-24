using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ArxivExpress.Features.ArticleList;

namespace ArxivExpress.Features.SearchArticles
{
    public interface IArticlesRepository
    {
        Task<ObservableCollection<ArticleEntry>> LoadArticles();
        Task<ObservableCollection<ArticleEntry>> LoadNextPage();
        Task<ObservableCollection<ArticleEntry>> LoadPrevPage();
        uint GetPageNumber();
        uint GetResultsPerPage();
    }
}
