// ****************************************************************************
//    File "contributor.dart"
//    Copyright © Dmitry Morozov 2025
//    Flutter reimplementation of ArxivExpress Xamarin app
// ****************************************************************************

class Contributor {
  final String name;
  final String? email;

  Contributor({
    required this.name,
    this.email,
  });

  factory Contributor.fromXml(Map<String, dynamic> contributorData) {
    return Contributor(
      name: contributorData['name'] ?? '',
      email: contributorData['email'],
    );
  }
}