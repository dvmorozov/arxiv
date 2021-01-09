using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ArxivExpress.Features.LikedArticles
{
    public class LikedArticlesRepository
    {
        private static LikedArticlesRepository _instance;
        private static List<string> _articleIds;
        private const string _fileName = "liked_articles.xml";
        private const string _rootElementName = "LikedArticleList";
        private const string _articleElementName = "LikedArticle";

        protected LikedArticlesRepository()
        {
        	_articleIds = new List<string>();
            LoadArticeIds();
        }

        private void LoadArticeIds()
        {
            _articleIds.Clear();

            var filePath = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), _fileName);

            if (File.Exists(filePath))
            {
                var xml = XDocument.Load(filePath);
                var query = from likedArticle
                            in xml.Root.Descendants(_articleElementName)
                            select likedArticle.Attribute("id").Value;

                foreach (var articleId in query)
                {
                    _articleIds.Add(articleId);
                }
            }
        }

        private void SaveArtcleIds()
        {
            var filePath = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), _fileName);

            var xml = new XDocument();
            var likedArticleElements = new XElement[_articleIds.Count];

            for (var i = 0; i < _articleIds.Count; i++)
            {
                likedArticleElements[i] = new XElement(_articleElementName,
                    new XAttribute("id", _articleIds[i]));
            }
            xml.Add(new XElement(_rootElementName, likedArticleElements));
            xml.Save(filePath);
        }

        public static LikedArticlesRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LikedArticlesRepository();
            }

            return _instance;
        }

        public void AddArticle(string id)
        {
            _articleIds.Add(id);
            SaveArtcleIds();
        }

        public void DeleteArticle(string id)
        {
            if (_articleIds.Contains(id))
            {
                _articleIds.Remove(id);
                SaveArtcleIds();
            }
        }

        public bool HasArticle(string id)
        {
            return _articleIds.Contains(id);
        }
    }
}
