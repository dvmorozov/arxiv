// ****************************************************************************
//    File "ViewedAuthorsRepository.cs"
//    Copyright © Dmitry Morozov 2022
//    If you want to use this file please contact me by dvmorozov@hotmail.com.
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ArxivExpress.Features.Data;
using ArxivExpress.Features.LikedArticles;
using ArxivExpress.Features.SelectedArticles.Data;
using ArxivExpress.Features.ViewedAuthors.Model;

namespace ArxivExpress.Features.ViewedAuthors.Data
{
    public class ViewedAuthorsRepository : IListRepository<Author>
    {
        private static ViewedAuthorsRepository _instance;

        protected List<Author> _viewedAuthors;

        private ViewedAuthorsRepository()
        {
            _viewedAuthors = LoadAuthors();
        }

        public static ViewedAuthorsRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ViewedAuthorsRepository();
            }

            return _instance;
        }

        private string GetFileName()
        {
            throw new Exception("This method should not be called.");
        }

        private string GetElementName()
        {
            throw new Exception("This method should not be called.");
        }

        private string GetRootElementName()
        {
            throw new Exception("This method should not be called.");
        }

        private string GetFilePath()
        {
            throw new Exception("This method should not be called.");
        }

        private List<Author> LoadAuthors()
        {
            var likedArticlesRepository = LikedArticlesRepository.GetInstance();
            var likedArticles = likedArticlesRepository.Articles;

            var uniqueAuthors = new SortedSet<string>();

            foreach (var article in likedArticles)
            {
                foreach (var contributor in article.Contributors)
                    uniqueAuthors.Add(contributor.Name);
            }

            var selectedArticlesListsRepository = SelectedArticlesListsRepository.GetInstance();
            var selectedArticlesLists = selectedArticlesListsRepository.ArticlesLists;
            foreach (var selectedArticlesList in selectedArticlesLists)
            {
                var rootNode = selectedArticlesListsRepository.GetArticleListRoot(selectedArticlesList.Name);
                var selectedArticlesListRepository = new SelectedArticlesListRepository(rootNode);

                foreach (var article in selectedArticlesListRepository.Articles)
                {
                    foreach (var contributor in article.Contributors)
                        uniqueAuthors.Add(contributor.Name);
                }
            }

            return (from author
                    in uniqueAuthors
                    select new Author { Name = author }).ToList();
        }

        public async Task<ObservableCollection<Author>> LoadFirstPage()
        {
            var task = new Task<ObservableCollection<Author>>(
                () =>
                {
                    var result = new ObservableCollection<Author>();
                    var start = GetPageNumber() * GetResultsPerPage();
                    var count = IsLastPage() ?
                        _viewedAuthors.Count - start : GetResultsPerPage();

                    foreach (var author in _viewedAuthors.GetRange((int)start, (int)count))
                    {
                        result.Add(author);
                    }
                    return result;
                });

            task.Start();
            return await task;
        }

        public Task<ObservableCollection<Author>> LoadNextPage()
        {
            if ((GetPageNumber() + 1) * GetResultsPerPage() < _viewedAuthors.Count)
            {
                _pageNumber++;
            }
            return LoadFirstPage();
        }

        public Task<ObservableCollection<Author>> LoadPrevPage()
        {
            if (_pageNumber > 0)
            {
                _pageNumber--;
            }
            return LoadFirstPage();
        }

        private uint _pageNumber = 0;

        public uint GetPageNumber()
        {
            return _pageNumber;
        }

        public uint GetResultsPerPage()
        {
            return 25;
        }

        public bool IsLastPage()
        {
            var start = GetPageNumber() * GetResultsPerPage();
            return GetResultsPerPage() >= _viewedAuthors.Count - start;
        }

        public bool IsEmpty()
        {
            return _viewedAuthors.Count == 0;
        }
    }
}
