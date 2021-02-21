using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ArxivExpress.Features.SearchArticles.Data
{
    public class SearchQueryRepository
    {
        private static SearchQueryRepository _instance;

        private SearchQueryRepository()
        {
        }

        public static SearchQueryRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SearchQueryRepository();
            }

            return _instance;
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

        public SearchQuery LoadSearchQuery()
        {
            var filePath = GetFilePath();

            if (File.Exists(filePath))
            {
                var xml = XDocument.Load(filePath);
                var list = (
                    from searchQuery
                    in xml.Descendants(GetSearchQueryElementName())
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
                ).ToList();

                if (list.Count != 0)
                {
                    return list[0];
                }
            }

            return new SearchQuery();
        }

        public void SaveSearchQuery(SearchQuery searchQuery)
        {
            var filePath = GetFilePath();

            var xml = new XDocument();

            var attributes = new object[8];
            attributes[0] = new XAttribute("SearchTerm", searchQuery.SearchTerm);
            attributes[1] = new XAttribute("Prefix", searchQuery.Prefix);
            attributes[2] = new XAttribute("ResultsPerPage", searchQuery.ResultsPerPage);
            attributes[3] = new XAttribute("SortByRelevance", searchQuery.SortByRelevance);
            attributes[4] = new XAttribute("SortByLastUpdatedDate", searchQuery.SortByLastUpdatedDate);
            attributes[5] = new XAttribute("SortBySubmittedDate", searchQuery.SortBySubmittedDate);
            attributes[6] = new XAttribute("SortOrderAscending", searchQuery.SortOrderAscending);
            attributes[7] = new XAttribute("SortOrderDescending", searchQuery.SortOrderDescending);

            xml.Add(new XElement(GetSearchQueryElementName(), attributes));
            xml.Save(filePath);
        }
    }
}
