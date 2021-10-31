// ****************************************************************************
//    File "SelectedArticlesListsRepository.cs"
//    Copyright © Dmitry Morozov 2021
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using ArxivExpress.Features.Data;
using ArxivExpress.Features.SelectedArticles.Model;

namespace ArxivExpress.Features.SelectedArticles.Data
{
    /// <summary>
    /// Represents list of selected article lists.
    /// </summary>
    public class SelectedArticlesListsRepository : IListRepository<SelectedArticlesList>
    {
        private static SelectedArticlesListsRepository _instance;

        private List<SelectedArticlesList> _selectedArticlesLists;
        public List<SelectedArticlesList> Articles => _selectedArticlesLists;

        private SelectedArticlesListsRepository()
        {
            _selectedArticlesLists = LoadSelectedArticlesLists();
        }

        public static SelectedArticlesListsRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SelectedArticlesListsRepository();
            }

            return _instance;
        }

        private string GetFileName()
        {
            return "selected_articles_lists.xml";
        }

        private string GetElementName()
        {
            return "SelectedArticleList";
        }

        private string GetRootElementName()
        {
            return "SelectedArticlesLists";
        }

        private string GetFilePath()
        {
            return Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), GetFileName());
        }

        public XElement GetArticleListRoot(string articleListName)
        {
            var filePath = GetFilePath();

            if (File.Exists(filePath))
            {
                var xml = XDocument.Load(filePath);
                var rootElement = (
                    from searchQuery
                    in xml.Root.Descendants(GetElementName())
                    where searchQuery.Attribute("Name").Value == articleListName
                    select searchQuery
                ).FirstOrDefault();

                return rootElement;
            }

            throw new Exception("Article list \"" + articleListName + "\" not found.");
        }

        /// <summary>
        /// Replaces article list by new element.
        /// </summary>
        /// <param name="root"></param>
        public void ReplaceArticleListElement(XElement root)
        {
            DeleteArticleListElementImpl(root);
            _selectedArticlesLists.Add(new SelectedArticlesList(root));
            SaveSelectedArticlesLists();
        }

        public void DeleteArticleListElement(XElement root)
        {
            DeleteArticleListElementImpl(root);
            SaveSelectedArticlesLists();
        }

        /// <summary>
        /// Deletes article list.
        /// </summary>
        /// <param name="root"></param>
        private void DeleteArticleListElementImpl(XElement root)
        {
            if (root.Attribute("Name") == null)
                throw new Exception("Attribute \"Name\" is not assigned.");

            var savedElements = (
                from item
                in _selectedArticlesLists
                where item.Name != root.Attribute("Name").Value
                select item
            ).OrderBy(list => list.Name).ToList();

            _selectedArticlesLists = savedElements;   
        }

        private List<SelectedArticlesList> LoadSelectedArticlesLists()
        {
            var filePath = GetFilePath();

            if (File.Exists(filePath))
            {
                var xml = XDocument.Load(filePath);
                var list = (
                    from xElement
                    in xml.Root.Descendants(GetElementName())
                    where xElement.Attribute("Name") != null
                    select new SelectedArticlesList(xElement)
                ).OrderBy(list => list.Name).ToList();

                return list;
            }

            return new List<SelectedArticlesList>();
        }

        public void SaveSelectedArticlesLists()
        {
            var filePath = GetFilePath();

            var xml = new XDocument();
            var listElements = new XElement[_selectedArticlesLists.Count];

            for (var i = 0; i < _selectedArticlesLists.Count; i++)
                listElements[i] = _selectedArticlesLists[i].InnerElement;

            xml.Add(new XElement(GetRootElementName(), listElements));
            xml.Save(filePath);
        }

        public async Task<ObservableCollection<SelectedArticlesList>> LoadFirstPage()
        {
            var task = new Task<ObservableCollection<SelectedArticlesList>>(
                () =>
                {
                    var result = new ObservableCollection<SelectedArticlesList>();
                    var start = GetPageNumber() * GetResultsPerPage();
                    var count = IsLastPage() ?
                        _selectedArticlesLists.Count - start : GetResultsPerPage();

                    foreach (var author in _selectedArticlesLists.GetRange((int)start, (int)count))
                    {
                        result.Add(author);
                    }
                    return result;
                });

            task.Start();
            return await task;
        }

        public Task<ObservableCollection<SelectedArticlesList>> LoadNextPage()
        {
            if ((GetPageNumber() + 1) * GetResultsPerPage() < _selectedArticlesLists.Count)
            {
                _pageNumber++;
            }
            return LoadFirstPage();
        }

        public Task<ObservableCollection<SelectedArticlesList>> LoadPrevPage()
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
            return GetResultsPerPage() >= _selectedArticlesLists.Count - start;
        }

        public void AddList(string name)
        {
            if (!_selectedArticlesLists.Exists(item => item.Name == name))
            {
                _selectedArticlesLists.Insert(0, new SelectedArticlesList(GetElementName(), name));
                SaveSelectedArticlesLists();
            }
        }

        public bool IsEmpty()
        {
            return _selectedArticlesLists.Count == 0;
        }
    }
}
