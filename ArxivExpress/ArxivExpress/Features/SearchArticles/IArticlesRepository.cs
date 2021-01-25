﻿using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ArxivExpress.Features.ArticleList;

namespace ArxivExpress.Features.SearchArticles
{
    public interface IArticlesRepository
    {
        Task<ObservableCollection<IArticleEntry>> LoadArticles();
        Task<ObservableCollection<IArticleEntry>> LoadNextPage();
        Task<ObservableCollection<IArticleEntry>> LoadPrevPage();
        uint GetPageNumber();
        uint GetResultsPerPage();
    }
}
