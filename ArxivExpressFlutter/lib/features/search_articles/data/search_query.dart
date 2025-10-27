// ****************************************************************************
//    File "search_query.dart"
//    Copyright © Dmitry Morozov 2025
//    Flutter reimplementation of ArxivExpress Xamarin app
// ****************************************************************************

class SearchQuery {
  String searchTerm;
  String prefix;
  String resultsPerPage = "25";

  // Mutually exclusive
  bool sortByRelevance = false;
  bool sortByLastUpdatedDate = false;
  bool sortBySubmittedDate = true;

  // Mutually exclusive
  bool sortOrderAscending = false;
  bool sortOrderDescending = true;

  int pageNumber = 0;

  SearchQuery({
    this.searchTerm = "",
    this.prefix = "all",
  }) {
    _fillDefaultValues();
  }

  void _fillDefaultValues() {
    resultsPerPage = "25";

    sortByRelevance = false;
    sortByLastUpdatedDate = false;
    sortBySubmittedDate = true;

    sortOrderAscending = false;
    sortOrderDescending = true;

    pageNumber = 0;
  }

  String _getQuerySortBy() {
    if (sortByRelevance) {
      return "relevance";
    } else if (sortByLastUpdatedDate) {
      return "lastUpdatedDate";
    } else {
      return "submittedDate";
    }
  }

  String _getQuerySortOrder() {
    if (sortOrderAscending) {
      return "ascending";
    } else {
      return "descending";
    }
  }

  int getResultsPerPage() {
    final parsed = int.tryParse(resultsPerPage);
    return parsed ?? 25;
  }

  String getQueryString() {
    var queryString = "http://export.arxiv.org/api/query?search_query=";

    if (prefix.isNotEmpty) {
      queryString += prefix;
    } else {
      queryString += "all";
    }

    if (searchTerm.isNotEmpty) {
      queryString += ":$searchTerm";
    }

    queryString += "&start=$pageNumber";
    queryString += "&max_results=${getResultsPerPage()}";
    queryString += "&sortBy=${_getQuerySortBy()}";
    queryString += "&sortOrder=${_getQuerySortOrder()}";

    return queryString;
  }
}