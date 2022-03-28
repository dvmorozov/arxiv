// ****************************************************************************
//    File "IListRepository.cs"
//    Copyright © Dmitry Morozov 2022
//    If you want to use this file please contact me by dvmorozov@hotmail.com.
// ****************************************************************************

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
        bool IsEmpty();
    }
}
