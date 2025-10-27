// ****************************************************************************
//    File "about.dart"
//    Copyright © Dmitry Morozov 2025
//    Flutter reimplementation of ArxivExpress Xamarin app
// ****************************************************************************

import 'package:flutter/material.dart';
import 'package:url_launcher/url_launcher.dart';

class About extends StatelessWidget {
  final String appTitle = "ArxivExpress";
  final String version = "1.0.0";
  final String buildNumber = "1";

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(appTitle),
      ),
      body: Padding(
        padding: EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Expanded(
                  flex: 3,
                  child: Text('Version', style: Theme.of(context).textTheme.bodyLarge),
                ),
                Expanded(
                  flex: 2,
                  child: Text(version),
                ),
              ],
            ),
            SizedBox(height: 16),
            Row(
              children: [
                Expanded(
                  flex: 3,
                  child: Text('Build', style: Theme.of(context).textTheme.bodyLarge),
                ),
                Expanded(
                  flex: 2,
                  child: Text(buildNumber),
                ),
              ],
            ),
            SizedBox(height: 32),
            Text(
              'Please send us your feedback about the application:',
              style: Theme.of(context).textTheme.bodyLarge,
            ),
            SizedBox(height: 16),
            ElevatedButton(
              onPressed: () async {
                final uri = Uri.parse('mailto:dvmorozov@hotmail.com?subject=ArxivExpress Feedback');
                if (await canLaunchUrl(uri)) {
                  await launchUrl(uri);
                }
              },
              child: Text('Send Feedback'),
            ),
            SizedBox(height: 32),
            Expanded(
              child: Container(
                padding: EdgeInsets.all(16),
                decoration: BoxDecoration(
                  border: Border.all(color: Colors.grey),
                  borderRadius: BorderRadius.circular(8),
                ),
                child: SingleChildScrollView(
                  child: Text(
                    'ArxivExpress is a cross-platform application for browsing and managing arXiv academic papers. '
                    'This Flutter version is a reimplementation of the original Xamarin.Forms app, providing the same '
                    'functionality with modern cross-platform technology.\n\n'
                    'Features:\n'
                    '• Search arXiv papers by various criteria\n'
                    '• View detailed article information\n'
                    '• Bookmark favorite articles\n'
                    '• Track recently viewed papers\n'
                    '• Manage custom article collections\n'
                    '• Browse author information\n\n'
                    'This application uses the arXiv API to access academic papers and metadata.',
                    style: Theme.of(context).textTheme.bodyMedium,
                  ),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}