// ****************************************************************************
//    File "search_query_repository.dart"
//    Copyright © Dmitry Morozov 2025
//    Flutter reimplementation of ArxivExpress Xamarin app
// ****************************************************************************

import 'package:shared_preferences/shared_preferences.dart';
import 'search_query.dart';

class SearchQueryRepository {
  static SearchQueryRepository? _instance;
  
  static SearchQueryRepository getInstance() {
    _instance ??= SearchQueryRepository._internal();
    return _instance!;
  }
  
  SearchQueryRepository._internal();

  Future<SearchQuery> loadSearchQuery() async {
    final prefs = await SharedPreferences.getInstance();
    
    final searchQuery = SearchQuery();
    searchQuery.searchTerm = prefs.getString('searchTerm') ?? '';
    searchQuery.prefix = prefs.getString('prefix') ?? 'all';
    searchQuery.resultsPerPage = prefs.getString('resultsPerPage') ?? '25';
    searchQuery.sortByRelevance = prefs.getBool('sortByRelevance') ?? false;
    searchQuery.sortByLastUpdatedDate = prefs.getBool('sortByLastUpdatedDate') ?? false;
    searchQuery.sortBySubmittedDate = prefs.getBool('sortBySubmittedDate') ?? true;
    searchQuery.sortOrderAscending = prefs.getBool('sortOrderAscending') ?? false;
    searchQuery.sortOrderDescending = prefs.getBool('sortOrderDescending') ?? true;
    searchQuery.pageNumber = prefs.getInt('pageNumber') ?? 0;
    
    return searchQuery;
  }

  Future<void> saveSearchQuery(SearchQuery searchQuery) async {
    final prefs = await SharedPreferences.getInstance();
    
    await prefs.setString('searchTerm', searchQuery.searchTerm);
    await prefs.setString('prefix', searchQuery.prefix);
    await prefs.setString('resultsPerPage', searchQuery.resultsPerPage);
    await prefs.setBool('sortByRelevance', searchQuery.sortByRelevance);
    await prefs.setBool('sortByLastUpdatedDate', searchQuery.sortByLastUpdatedDate);
    await prefs.setBool('sortBySubmittedDate', searchQuery.sortBySubmittedDate);
    await prefs.setBool('sortOrderAscending', searchQuery.sortOrderAscending);
    await prefs.setBool('sortOrderDescending', searchQuery.sortOrderDescending);
    await prefs.setInt('pageNumber', searchQuery.pageNumber);
  }
}