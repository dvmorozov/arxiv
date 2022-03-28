// ****************************************************************************
//    File "SearchQuery.cs"
//    Copyright © Dmitry Morozov 2022
//    If you want to use this file please contact me by dvmorozov@hotmail.com.
// ****************************************************************************

namespace ArxivExpress.Features.SearchArticles
{
    //  TODO: Convert SearchQuery to singleton.
    public class SearchQuery
    {
        public string SearchTerm;
        public string Prefix;
        public string ResultsPerPage;

        //  Mutually exclusive.
        public bool SortByRelevance;
        public bool SortByLastUpdatedDate;
        public bool SortBySubmittedDate;

        //  Mutually exclusive.
        public bool SortOrderAscending;
        public bool SortOrderDescending;

        public uint PageNumber;

        public SearchQuery(string searchTerm = "", string prefix = "all")
        {
            FillDefaultValues(searchTerm, prefix);
        }

        private void FillDefaultValues(string searchTerm, string prefix)
        {
            SearchTerm = searchTerm;
            ResultsPerPage = "25";
            Prefix = prefix;

            SortByRelevance = false;
            SortByLastUpdatedDate = false;
            SortBySubmittedDate = true;

            SortOrderAscending = false;
            SortOrderDescending = true;

            PageNumber = 0;
        }

        private string GetQuerySortBy()
        {
            if (SortByRelevance)
            {
                return "relevance";
            }
            else if (SortByLastUpdatedDate)
            {
                return "lastUpdatedDate";
            }
            else //if (SortBySubmittedDate)
            {
                return "submittedDate";
            }
        }

        private string GetQuerySortOrder()
        {
            if (SortOrderAscending)
            {
                return "ascending";
            }
            else //if (SortOrderDescending)
            {
                return "descending";
            }
        }

        public uint GetResultsPerPage()
        {
            if (!uint.TryParse(ResultsPerPage, out uint resultsPerPage))
            {
                resultsPerPage = 25;
            }

            return resultsPerPage;
        }

        public string GetQueryString()
        {
            var queryString = "http://export.arxiv.org/api/query?search_query=";

            if (Prefix != null && Prefix != string.Empty)
            {
                queryString += Prefix;
            }
            else
            {
                queryString += "all";
            }

            if (SearchTerm != null && SearchTerm != string.Empty)
            {
                queryString += ":\"" + SearchTerm + "\"";
            }

            var resultsPerPage = GetResultsPerPage();

            queryString += "&start=" + (PageNumber * resultsPerPage).ToString();
            queryString += "&max_results=" + resultsPerPage.ToString();
            queryString += "&sortBy=" + GetQuerySortBy();
            queryString += "&sortOrder=" + GetQuerySortOrder();

            return queryString;
        }
    }
}
