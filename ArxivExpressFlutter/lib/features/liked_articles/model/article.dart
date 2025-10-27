// ****************************************************************************
//    File "article.dart"
//    Copyright © Dmitry Morozov 2025
//    Flutter reimplementation of ArxivExpress Xamarin app
// ****************************************************************************

import '../../../shared/models/contributor.dart';

abstract class Article {
  List<String> get categories;
  List<Contributor> get contributors;
  String get id;
  DateTime? get lastUpdated;
  String get link;
  DateTime? get published;
  String get summary;
  String get title;
}