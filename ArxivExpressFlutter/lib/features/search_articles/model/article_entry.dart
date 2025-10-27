// ****************************************************************************
//    File "article_entry.dart"
//    Copyright © Dmitry Morozov 2025
//    Flutter reimplementation of ArxivExpress Xamarin app
// ****************************************************************************

import 'package:xml/xml.dart';
import '../../../shared/models/contributor.dart';
import 'i_article_entry.dart';
import '../../../features/liked_articles/model/article.dart';

class ArticleEntry extends Article implements IArticleEntry {
  final XmlElement _entry;

  ArticleEntry(this._entry);

  @override
  List<String> get categories {
    List<String> result = [];
    final categoryElements = _entry.findAllElements('category');
    for (var category in categoryElements) {
      final term = category.getAttribute('term');
      if (term != null) {
        result.add(term);
      }
    }
    return result;
  }

  @override
  List<Contributor> get contributors {
    List<Contributor> result = [];
    final authorElements = _entry.findAllElements('author');
    for (var author in authorElements) {
      final nameElement = author.findElements('name').firstOrNull;
      if (nameElement != null) {
        result.add(Contributor(name: nameElement.innerText));
      }
    }
    return result;
  }

  @override
  String get id {
    final idElement = _entry.findElements('id').firstOrNull;
    return idElement?.innerText ?? '';
  }

  @override
  DateTime? get lastUpdated {
    final updatedElement = _entry.findElements('updated').firstOrNull;
    if (updatedElement != null) {
      try {
        return DateTime.parse(updatedElement.innerText);
      } catch (e) {
        return null;
      }
    }
    return null;
  }

  @override
  String get link {
    final linkElement = _entry.findAllElements('link').firstOrNull;
    return linkElement?.getAttribute('href') ?? '';
  }

  @override
  DateTime? get published {
    final publishedElement = _entry.findElements('published').firstOrNull;
    if (publishedElement != null) {
      try {
        return DateTime.parse(publishedElement.innerText);
      } catch (e) {
        return null;
      }
    }
    return null;
  }

  @override
  String get summary {
    final summaryElement = _entry.findElements('summary').firstOrNull;
    return summaryElement?.innerText.trim() ?? '';
  }

  @override
  String get title {
    final titleElement = _entry.findElements('title').firstOrNull;
    return titleElement?.innerText.trim() ?? '';
  }

  @override
  String get contributorsAbbreviated {
    if (contributors.isEmpty) return '';
    if (contributors.length == 1) return contributors.first.name;
    if (contributors.length <= 3) {
      return contributors.map((c) => c.name).join(', ');
    }
    return '${contributors.first.name} et al.';
  }

  @override
  String get publishedWithLabel {
    if (published != null) {
      return 'Published: ${_formatDate(published!)}';
    }
    return '';
  }

  @override
  String get lastUpdatedWithLabel {
    if (lastUpdated != null) {
      return 'Updated: ${_formatDate(lastUpdated!)}';
    }
    return '';
  }

  String _formatDate(DateTime date) {
    return '${date.day.toString().padLeft(2, '0')}/${date.month.toString().padLeft(2, '0')}/${date.year}';
  }
}