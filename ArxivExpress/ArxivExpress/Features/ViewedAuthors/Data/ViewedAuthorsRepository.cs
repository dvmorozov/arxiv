using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using ArxivExpress.Features.Data;
using ArxivExpress.Features.ViewedAuthors.Model;

namespace ArxivExpress.Features.ViewedAuthors.Data
{
    public class ViewedAuthorsRepository : IListRepository<Author>
    {
        private static ViewedAuthorsRepository _instance;

        protected List<Author> _authors;
        public List<Author> Articles => _authors;

        private ViewedAuthorsRepository()
        {
            _authors = LoadAuthors();
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
            return "viewed_authors.xml";
        }

        private string GetElementName()
        {
            return "ViewedAuthors";
        }

        private string GetRootElementName()
        {
            return "ViewedAuthorsList";
        }

        private string GetFilePath()
        {
            return Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), GetFileName());
        }

        private List<Author> LoadAuthors()
        {
            var filePath = GetFilePath();

            if (File.Exists(filePath))
            {
                var xml = XDocument.Load(filePath);
                var list = (
                    from searchQuery
                    in xml.Root.Descendants(GetElementName())
                    where searchQuery.Attribute("Name") != null
                    select new Author
                    {
                        Name = searchQuery.Attribute("Name").Value
                    }
                ).OrderBy(author => author.Name).ToList();

                return list;
            }

            return new List<Author>();
        }

        private void SaveAuthors()
        {
            var filePath = GetFilePath();

            var xml = new XDocument();
            var authorElements = new XElement[_authors.Count];

            for (var i = 0; i < _authors.Count; i++)
            {
                var attributes = new object[1];
                attributes[0] = new XAttribute("Name", _authors[i].Name);

                authorElements[i] = new XElement(GetElementName(), attributes);
            }

            xml.Add(new XElement(GetRootElementName(), authorElements));
            xml.Save(filePath);
        }

        public async Task<ObservableCollection<Author>> LoadFirstPage()
        {
            var task = new Task<ObservableCollection<Author>>(
                () =>
                {
                    var result = new ObservableCollection<Author>();
                    var start = GetPageNumber() * GetResultsPerPage();
                    var count = IsLastPage() ?
                        _authors.Count - start : GetResultsPerPage();

                    foreach (var author in _authors.GetRange((int)start, (int)count))
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
            if ((GetPageNumber() + 1) * GetResultsPerPage() < _authors.Count)
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
            return GetResultsPerPage() >= _authors.Count - start;
        }

        public void AddAuthor(Author author)
        {
            if (!_authors.Exists(item => item.Name == author.Name))
            {
                _authors.Insert(0, author);
                SaveAuthors();
            }
        }
    }
}
