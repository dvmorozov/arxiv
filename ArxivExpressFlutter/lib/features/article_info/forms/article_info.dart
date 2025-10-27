// ****************************************************************************
//    File "article_info.dart"
//    Copyright © Dmitry Morozov 2025
//    Flutter reimplementation of ArxivExpress Xamarin app
// ****************************************************************************

import 'package:flutter/material.dart';
import 'package:url_launcher/url_launcher.dart';
import '../../search_articles/model/article_entry.dart';

class ArticleInfo extends StatelessWidget {
  final ArticleEntry articleEntry;

  ArticleInfo({required this.articleEntry});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Article'),
      ),
      body: SingleChildScrollView(
        padding: EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Title
            Text(
              articleEntry.title,
              style: Theme.of(context).textTheme.headlineSmall?.copyWith(
                fontWeight: FontWeight.bold,
              ),
            ),
            SizedBox(height: 16),
            
            // Summary
            Text(
              articleEntry.summary,
              style: Theme.of(context).textTheme.bodyMedium,
            ),
            SizedBox(height: 16),
            
            // Contributors
            if (articleEntry.contributors.isNotEmpty) ...[
              Text(
                'Authors:',
                style: Theme.of(context).textTheme.titleMedium?.copyWith(
                  fontWeight: FontWeight.bold,
                ),
              ),
              SizedBox(height: 8),
              Wrap(
                spacing: 8,
                runSpacing: 4,
                children: articleEntry.contributors.map((contributor) {
                  return Chip(
                    label: Text(contributor.name),
                    materialTapTargetSize: MaterialTapTargetSize.shrinkWrap,
                  );
                }).toList(),
              ),
              SizedBox(height: 16),
            ],
            
            // Dates
            Row(
              children: [
                if (articleEntry.publishedWithLabel.isNotEmpty)
                  Expanded(
                    child: Text(
                      articleEntry.publishedWithLabel,
                      style: Theme.of(context).textTheme.bodySmall,
                    ),
                  ),
                if (articleEntry.lastUpdatedWithLabel.isNotEmpty)
                  Expanded(
                    child: Text(
                      articleEntry.lastUpdatedWithLabel,
                      style: Theme.of(context).textTheme.bodySmall,
                    ),
                  ),
              ],
            ),
            SizedBox(height: 16),
            
            // Categories
            if (articleEntry.categories.isNotEmpty) ...[
              Text(
                'Categories:',
                style: Theme.of(context).textTheme.titleMedium?.copyWith(
                  fontWeight: FontWeight.bold,
                ),
              ),
              SizedBox(height: 8),
              Wrap(
                spacing: 8,
                runSpacing: 4,
                children: articleEntry.categories.map((category) {
                  return Container(
                    padding: EdgeInsets.symmetric(horizontal: 8, vertical: 4),
                    decoration: BoxDecoration(
                      border: Border.all(color: Colors.grey),
                      borderRadius: BorderRadius.circular(4),
                    ),
                    child: Text(
                      category,
                      style: Theme.of(context).textTheme.bodySmall,
                    ),
                  );
                }).toList(),
              ),
              SizedBox(height: 16),
            ],
            
            // Link to arXiv
            if (articleEntry.link.isNotEmpty) ...[
              ElevatedButton.icon(
                onPressed: () async {
                  final uri = Uri.parse(articleEntry.link);
                  if (await canLaunchUrl(uri)) {
                    await launchUrl(uri);
                  }
                },
                icon: Icon(Icons.open_in_new),
                label: Text('View on arXiv'),
              ),
            ],
          ],
        ),
      ),
    );
  }
}