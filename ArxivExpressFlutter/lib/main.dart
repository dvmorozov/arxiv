// ****************************************************************************
//    File "main.dart"
//    Copyright © Dmitry Morozov 2025
//    Flutter reimplementation of ArxivExpress Xamarin app
// ****************************************************************************

import 'package:flutter/material.dart';
import 'features/search_articles/forms/search_attributes.dart';

void main() {
  runApp(ArxivExpressApp());
}

class ArxivExpressApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'ArxivExpress',
      theme: ThemeData(
        primarySwatch: Colors.blue,
        visualDensity: VisualDensity.adaptivePlatformDensity,
      ),
      home: SearchAttributes(),
      debugShowCheckedModeBanner: false,
    );
  }
}