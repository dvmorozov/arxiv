using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ArxivExpress.Features.Data
{
    public interface IListRepository<T>
    {
        Task<ObservableCollection<T>> LoadFirstPage();
        Task<ObservableCollection<T>> LoadNextPage();
        Task<ObservableCollection<T>> LoadPrevPage();
        uint GetPageNumber();
        uint GetResultsPerPage();
        bool IsLastPage();
    }
}
