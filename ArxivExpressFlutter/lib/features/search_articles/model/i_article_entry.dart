// ****************************************************************************
//    File "i_article_entry.dart"
//    Copyright © Dmitry Morozov 2025
//    Flutter reimplementation of ArxivExpress Xamarin app
// ****************************************************************************

import '../../../shared/models/contributor.dart';

abstract class IArticleEntry {
  List<String> get categories;
  List<Contributor> get contributors;
  String get id;
  DateTime? get lastUpdated;
  String get link;
  DateTime? get published;
  String get summary;
  String get title;
  
  String get contributorsAbbreviated;
  String get publishedWithLabel;
  String get lastUpdatedWithLabel;
}