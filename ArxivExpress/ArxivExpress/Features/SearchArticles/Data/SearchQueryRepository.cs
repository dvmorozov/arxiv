using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ArxivExpress.Features.SearchArticles.Data
{
    public class SearchQueryRepository
    {
        private static SearchQueryRepository _instance;
        private SearchQuery _searchQuery;

        private SearchQueryRepository()
        {
            _searchQuery = LoadSearchQueryFromFile(GetFilePath());
        }

        public static SearchQueryRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SearchQueryRepository();
            }

            return _instance;
        }

        public SearchQuery GetSearchQuery()
        {
            return _searchQuery;
        }

        private string GetFileName()
        {
            return "search_query.xml";
        }

        private string GetSearchQueryElementName()
        {
            return "SearchQuery";
        }

        private string GetFilePath()
        {
            return Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), GetFileName());
        }

        private SearchQuery LoadSearchQueryFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var xml = XDocument.Load(filePath);

                return (
                    from searchQuery
                    in xml.Root.Descendants(GetSearchQueryElementName())
                    select new SearchQuery
                    {
                        SearchTerm = searchQuery.Attribute("SearchTerm")?.Value,
                        Prefix = searchQuery.Attribute("Prefix")?.Value,
                        ResultsPerPage = searchQuery.Attribute("ResultsPerPage")?.Value,

                        SortByRelevance = bool.Parse(searchQuery.Attribute("SortByRelevance")?.Value),
                        SortByLastUpdatedDate = bool.Parse(searchQuery.Attribute("SortByLastUpdatedDate")?.Value),
                        SortBySubmittedDate = bool.Parse(searchQuery.Attribute("SortBySubmittedDate")?.Value),

                        SortOrderAscending = bool.Parse(searchQuery.Attribute("SortOrderAscending")?.Value),
                        SortOrderDescending = bool.Parse(searchQuery.Attribute("SortOrderDescending")?.Value)
                    }
                ).ToList()[0];
            }

            return new SearchQuery();
        }

        protected void SaveArtcles()
        {
            var filePath = GetFilePath();

            var xml = new XDocument();

            var attributes = new object[8];
            attributes[0] = new XAttribute("SearchTerm", _searchQuery.SearchTerm);
            attributes[1] = new XAttribute("Prefix", _searchQuery.Prefix);
            attributes[2] = new XAttribute("ResultsPerPage", _searchQuery.ResultsPerPage);
            attributes[3] = new XAttribute("SortByRelevance", _searchQuery.SortByRelevance);
            attributes[4] = new XAttribute("SortByLastUpdatedDate", _searchQuery.SortByLastUpdatedDate);
            attributes[5] = new XAttribute("SortBySubmittedDate", _searchQuery.SortBySubmittedDate);
            attributes[6] = new XAttribute("SortOrderAscending", _searchQuery.SortOrderAscending);
            attributes[7] = new XAttribute("SortOrderDescending", _searchQuery.SortOrderDescending);

            xml.Add(new XElement(GetSearchQueryElementName(), attributes));
            xml.Save(filePath);
        }
    }
}
