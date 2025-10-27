// ****************************************************************************
//    File "search_attributes.dart"
//    Copyright © Dmitry Morozov 2025
//    Flutter reimplementation of ArxivExpress Xamarin app
// ****************************************************************************

import 'package:flutter/material.dart';
import '../data/search_query.dart';
import '../data/search_query_repository.dart';
import 'article_list.dart';
import '../../../forms/about.dart';
import '../../liked_articles/forms/liked_articles.dart';
import '../../recently_viewed_articles/forms/recently_viewed_articles.dart';
import '../../viewed_authors/forms/viewed_authors.dart';
import '../../selected_articles/forms/selected_articles.dart';

class FieldPrefix {
  final String prefix;
  final String explanation;

  FieldPrefix({required this.prefix, required this.explanation});
}

class SearchAttributes extends StatefulWidget {
  @override
  _SearchAttributesState createState() => _SearchAttributesState();
}

class _SearchAttributesState extends State<SearchAttributes> {
  late SearchQueryRepository _searchQueryRepository;
  SearchQuery? _searchQuery;
  late List<FieldPrefix> _fieldPrefixes;
  
  final TextEditingController _searchTermController = TextEditingController();
  int _selectedFieldIndex = 0;
  String _selectedResultsPerPage = "25";

  @override
  void initState() {
    super.initState();
    _searchQueryRepository = SearchQueryRepository.getInstance();
    _initializeFieldPrefixes();
    _loadSearchQuery();
  }

  void _initializeFieldPrefixes() {
    _fieldPrefixes = [
      FieldPrefix(prefix: "ti", explanation: "Title"),
      FieldPrefix(prefix: "au", explanation: "Author"),
      FieldPrefix(prefix: "abs", explanation: "Abstract"),
      FieldPrefix(prefix: "co", explanation: "Comment"),
      FieldPrefix(prefix: "jr", explanation: "Journal Reference"),
      FieldPrefix(prefix: "cat", explanation: "Subject Category"),
      FieldPrefix(prefix: "rn", explanation: "Report Number"),
      FieldPrefix(prefix: "all", explanation: "All Fields"),
    ];
  }

  Future<void> _loadSearchQuery() async {
    final query = await _searchQueryRepository.loadSearchQuery();
    setState(() {
      _searchQuery = query;
      _searchTermController.text = query.searchTerm;
      _selectedFieldIndex = _fieldPrefixes.indexWhere((fp) => fp.prefix == query.prefix);
      if (_selectedFieldIndex == -1) _selectedFieldIndex = 0;
      _selectedResultsPerPage = query.resultsPerPage;
    });
  }

  Future<void> _saveSearchQuery() async {
    final query = _searchQuery;
    if (query != null) {
      query.searchTerm = _searchTermController.text;
      query.prefix = _fieldPrefixes[_selectedFieldIndex].prefix;
      query.resultsPerPage = _selectedResultsPerPage;
      await _searchQueryRepository.saveSearchQuery(query);
    }
  }

  void _onSearchPressed() async {
    await _saveSearchQuery();
    final query = _searchQuery;
    if (query != null) {
      Navigator.of(context).push(
        MaterialPageRoute(
          builder: (context) => ArticleList(searchQuery: query),
        ),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    if (_searchQuery == null) {
      return Scaffold(
        appBar: AppBar(title: Text('Search')),
        body: Center(child: CircularProgressIndicator()),
      );
    }

    return Scaffold(
      appBar: AppBar(
        title: Text('Search'),
        actions: [
          IconButton(
            icon: Icon(Icons.info),
            onPressed: () {
              Navigator.of(context).push(
                MaterialPageRoute(builder: (context) => About()),
              );
            },
          ),
        ],
      ),
      body: Column(
        children: [
          Expanded(
            child: SingleChildScrollView(
              padding: EdgeInsets.all(16),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  // Search Term and Field Selection
                  Row(
                    children: [
                      Expanded(
                        flex: 3,
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text('Search Term', style: Theme.of(context).textTheme.bodyLarge),
                            SizedBox(height: 8),
                            TextField(
                              controller: _searchTermController,
                              decoration: InputDecoration(
                                hintText: 'Search term...',
                                border: OutlineInputBorder(),
                                suffixIcon: IconButton(
                                  icon: Icon(Icons.clear),
                                  onPressed: () => _searchTermController.clear(),
                                ),
                              ),
                              onSubmitted: (_) => _onSearchPressed(),
                            ),
                          ],
                        ),
                      ),
                      SizedBox(width: 16),
                      Expanded(
                        flex: 2,
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text('Search Field', style: Theme.of(context).textTheme.bodyLarge),
                            SizedBox(height: 8),
                            DropdownButtonFormField<int>(
                              value: _selectedFieldIndex,
                              decoration: InputDecoration(border: OutlineInputBorder()),
                              items: _fieldPrefixes.asMap().entries.map((entry) {
                                return DropdownMenuItem<int>(
                                  value: entry.key,
                                  child: Text(entry.value.explanation),
                                );
                              }).toList(),
                              onChanged: (int? value) {
                                if (value != null) {
                                  setState(() {
                                    _selectedFieldIndex = value;
                                  });
                                }
                              },
                            ),
                          ],
                        ),
                      ),
                    ],
                  ),
                  
                  SizedBox(height: 24),
                  
                  // Sort By Section
                  Text('Sort By', style: Theme.of(context).textTheme.headlineSmall),
                  SizedBox(height: 8),
                  Column(
                    children: [
                      RadioListTile<String>(
                        title: Text('Relevance'),
                        value: 'relevance',
                        groupValue: _searchQuery?.sortByRelevance == true ? 'relevance' : 
                                   _searchQuery?.sortByLastUpdatedDate == true ? 'lastUpdated' : 'submitted',
                        onChanged: (value) {
                          final query = _searchQuery;
                          if (query != null) {
                            setState(() {
                              query.sortByRelevance = true;
                              query.sortByLastUpdatedDate = false;
                              query.sortBySubmittedDate = false;
                            });
                          }
                        },
                      ),
                      RadioListTile<String>(
                        title: Text('Last Updated Date'),
                        value: 'lastUpdated',
                        groupValue: _searchQuery?.sortByRelevance == true ? 'relevance' : 
                                   _searchQuery?.sortByLastUpdatedDate == true ? 'lastUpdated' : 'submitted',
                        onChanged: (value) {
                          final query = _searchQuery;
                          if (query != null) {
                            setState(() {
                              query.sortByRelevance = false;
                              query.sortByLastUpdatedDate = true;
                              query.sortBySubmittedDate = false;
                            });
                          }
                        },
                      ),
                      RadioListTile<String>(
                        title: Text('Submitted Date'),
                        value: 'submitted',
                        groupValue: _searchQuery?.sortByRelevance == true ? 'relevance' : 
                                   _searchQuery?.sortByLastUpdatedDate == true ? 'lastUpdated' : 'submitted',
                        onChanged: (value) {
                          final query = _searchQuery;
                          if (query != null) {
                            setState(() {
                              query.sortByRelevance = false;
                              query.sortByLastUpdatedDate = false;
                              query.sortBySubmittedDate = true;
                            });
                          }
                        },
                      ),
                    ],
                  ),
                  
                  SizedBox(height: 16),
                  
                  // Sort Order Section
                  Text('Sort Order', style: Theme.of(context).textTheme.headlineSmall),
                  SizedBox(height: 8),
                  Column(
                    children: [
                      RadioListTile<String>(
                        title: Text('Ascending'),
                        value: 'ascending',
                        groupValue: _searchQuery?.sortOrderAscending == true ? 'ascending' : 'descending',
                        onChanged: (value) {
                          final query = _searchQuery;
                          if (query != null) {
                            setState(() {
                              query.sortOrderAscending = true;
                              query.sortOrderDescending = false;
                            });
                          }
                        },
                      ),
                      RadioListTile<String>(
                        title: Text('Descending'),
                        value: 'descending',
                        groupValue: _searchQuery?.sortOrderAscending == true ? 'ascending' : 'descending',
                        onChanged: (value) {
                          final query = _searchQuery;
                          if (query != null) {
                            setState(() {
                              query.sortOrderAscending = false;
                              query.sortOrderDescending = true;
                            });
                          }
                        },
                      ),
                    ],
                  ),
                  
                  SizedBox(height: 16),
                  
                  // Results Per Page
                  Row(
                    children: [
                      DropdownButton<String>(
                        value: _selectedResultsPerPage,
                        items: ['25', '50', '100', '200'].map((String value) {
                          return DropdownMenuItem<String>(
                            value: value,
                            child: Text(value),
                          );
                        }).toList(),
                        onChanged: (String? value) {
                          if (value != null) {
                            setState(() {
                              _selectedResultsPerPage = value;
                            });
                          }
                        },
                      ),
                      SizedBox(width: 16),
                      Text('results per page'),
                    ],
                  ),
                  
                  SizedBox(height: 24),
                  
                  // Search Button
                  Center(
                    child: ElevatedButton.icon(
                      onPressed: _onSearchPressed,
                      icon: Icon(Icons.search),
                      label: Text('Search'),
                      style: ElevatedButton.styleFrom(
                        padding: EdgeInsets.symmetric(horizontal: 40, vertical: 16),
                        minimumSize: Size(200, 50),
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ),
          
          // Bottom Navigation Buttons
          Container(
            padding: EdgeInsets.symmetric(vertical: 16),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children: [
                _buildNavigationButton(
                  icon: Icons.history,
                  onPressed: () {
                    Navigator.of(context).push(
                      MaterialPageRoute(builder: (context) => RecentlyViewedArticles()),
                    );
                  },
                ),
                _buildNavigationButton(
                  icon: Icons.favorite,
                  onPressed: () {
                    Navigator.of(context).push(
                      MaterialPageRoute(builder: (context) => LikedArticles()),
                    );
                  },
                ),
                _buildNavigationButton(
                  icon: Icons.people,
                  onPressed: () {
                    Navigator.of(context).push(
                      MaterialPageRoute(builder: (context) => ViewedAuthors()),
                    );
                  },
                ),
                _buildNavigationButton(
                  icon: Icons.library_books,
                  onPressed: () {
                    Navigator.of(context).push(
                      MaterialPageRoute(builder: (context) => SelectedArticles()),
                    );
                  },
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildNavigationButton({
    required IconData icon,
    required VoidCallback onPressed,
  }) {
    return Container(
      width: 50,
      height: 50,
      decoration: BoxDecoration(
        border: Border.all(color: Colors.black),
        color: Colors.white,
      ),
      child: IconButton(
        onPressed: onPressed,
        icon: Icon(icon, color: Colors.black),
      ),
    );
  }

  @override
  void dispose() {
    _searchTermController.dispose();
    super.dispose();
  }
}