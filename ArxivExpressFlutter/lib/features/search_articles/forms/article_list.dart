// ****************************************************************************
//    File "article_list.dart"
//    Copyright © Dmitry Morozov 2025
//    Flutter reimplementation of ArxivExpress Xamarin app
// ****************************************************************************

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:xml/xml.dart';
import '../data/search_query.dart';
import '../model/article_entry.dart';
import '../../article_info/forms/article_info.dart';
import '../../../forms/about.dart';

class ArticleList extends StatefulWidget {
  final SearchQuery searchQuery;

  ArticleList({required this.searchQuery});

  @override
  _ArticleListState createState() => _ArticleListState();
}

class _ArticleListState extends State<ArticleList> {
  List<ArticleEntry> articles = [];
  bool isLoading = true;
  String? errorMessage;

  @override
  void initState() {
    super.initState();
    _loadArticles();
  }

  Future<void> _loadArticles() async {
    setState(() {
      isLoading = true;
      errorMessage = null;
    });

    try {
      final queryUrl = widget.searchQuery.getQueryString();
      final response = await http.get(Uri.parse(queryUrl));

      if (response.statusCode == 200) {
        final document = XmlDocument.parse(response.body);
        final entries = document.findAllElements('entry');
        
        final List<ArticleEntry> loadedArticles = [];
        for (var entry in entries) {
          loadedArticles.add(ArticleEntry(entry));
        }

        setState(() {
          articles = loadedArticles;
          isLoading = false;
        });
      } else {
        setState(() {
          errorMessage = 'Failed to load articles: ${response.statusCode}';
          isLoading = false;
        });
      }
    } catch (e) {
      setState(() {
        errorMessage = 'Error loading articles: $e';
        isLoading = false;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Articles'),
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
      body: _buildBody(),
    );
  }

  Widget _buildBody() {
    if (isLoading) {
      return Center(child: CircularProgressIndicator());
    }

    if (errorMessage != null) {
      return Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Text(
              errorMessage!,
              style: TextStyle(color: Colors.red),
              textAlign: TextAlign.center,
            ),
            SizedBox(height: 16),
            ElevatedButton(
              onPressed: _loadArticles,
              child: Text('Retry'),
            ),
          ],
        ),
      );
    }

    if (articles.isEmpty) {
      return Center(
        child: Text('No articles found'),
      );
    }

    return ListView.separated(
      itemCount: articles.length,
      separatorBuilder: (context, index) => Divider(),
      itemBuilder: (context, index) {
        final article = articles[index];
        return ListTile(
          title: Text(
            article.title,
            style: TextStyle(fontWeight: FontWeight.bold),
            maxLines: 3,
            overflow: TextOverflow.ellipsis,
          ),
          subtitle: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              SizedBox(height: 8),
              Text(
                article.summary,
                maxLines: 3,
                overflow: TextOverflow.ellipsis,
              ),
              SizedBox(height: 8),
              Text(
                article.contributorsAbbreviated,
                style: TextStyle(fontStyle: FontStyle.italic),
                maxLines: 1,
                overflow: TextOverflow.ellipsis,
              ),
              SizedBox(height: 4),
              Row(
                children: [
                  if (article.publishedWithLabel.isNotEmpty)
                    Expanded(
                      child: Text(
                        article.publishedWithLabel,
                        style: TextStyle(fontSize: 12, color: Colors.grey[600]),
                      ),
                    ),
                  if (article.lastUpdatedWithLabel.isNotEmpty)
                    Expanded(
                      child: Text(
                        article.lastUpdatedWithLabel,
                        style: TextStyle(fontSize: 12, color: Colors.grey[600]),
                      ),
                    ),
                ],
              ),
            ],
          ),
          contentPadding: EdgeInsets.all(16),
          onTap: () {
            Navigator.of(context).push(
              MaterialPageRoute(
                builder: (context) => ArticleInfo(articleEntry: article),
              ),
            );
          },
        );
      },
    );
  }
}