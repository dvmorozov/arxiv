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

        protected List<SelectedArticlesList> _selectedArticlesLists;
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

        public XElement GetArticleListRootNode(string articleListName)
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

            return new XElement("");
        }

        private List<SelectedArticlesList> LoadSelectedArticlesLists()
        {
            var filePath = GetFilePath();

            if (File.Exists(filePath))
            {
                var xml = XDocument.Load(filePath);
                var list = (
                    from searchQuery
                    in xml.Root.Descendants(GetElementName())
                    where searchQuery.Attribute("Name") != null
                    select new SelectedArticlesList
                    {
                        Name = searchQuery.Attribute("Name").Value
                    }
                ).OrderBy(list => list.Name).ToList();

                return list;
            }

            return new List<SelectedArticlesList>();
        }

        private void SaveSelectedArticlesLists()
        {
            var filePath = GetFilePath();

            var xml = new XDocument();
            var authorElements = new XElement[_selectedArticlesLists.Count];

            for (var i = 0; i < _selectedArticlesLists.Count; i++)
            {
                var attributes = new object[1];
                attributes[0] = new XAttribute("Name", _selectedArticlesLists[i].Name);

                authorElements[i] = new XElement(GetElementName(), attributes);
            }

            xml.Add(new XElement(GetRootElementName(), authorElements));
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

        public void AddList(SelectedArticlesList list)
        {
            if (!_selectedArticlesLists.Exists(item => item.Name == list.Name))
            {
                _selectedArticlesLists.Insert(0, list);
                SaveSelectedArticlesLists();
            }
        }

        public bool IsEmpty()
        {
            return _selectedArticlesLists.Count == 0;
        }
    }
}
