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
        private static List<LikedArticle> _likedArticles;
        private const string _fileName = "liked_articles.xml";
        private const string _rootElementName = "LikedArticleList";
        private const string _articleElementName = "LikedArticle";

        protected LikedArticlesRepository()
        {
        	_likedArticles = new List<LikedArticle>();
            LoadArticeIds();
        }

        private void LoadArticeIds()
        {
            _likedArticles.Clear();

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
                    _likedArticles.Add(new LikedArticle { Id = articleId });
                }
            }
        }

        private void SaveArtcleIds()
        {
            var filePath = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), _fileName);

            var xml = new XDocument();
            var likedArticleElements = new XElement[_likedArticles.Count];

            for (var i = 0; i < _likedArticles.Count; i++)
            {
                likedArticleElements[i] = new XElement(_articleElementName,
                    new XAttribute("id", _likedArticles[i]));
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

        public void AddArticle(string articleId)
        {
            _likedArticles.Add(new LikedArticle { Id = articleId });
            SaveArtcleIds();
        }

        public void DeleteArticle(string articleId)
        {
            if (_likedArticles.Exists(item => item.Id == articleId))
            {
                _likedArticles.RemoveAll(item => item.Id == articleId);
                SaveArtcleIds();
            }
        }

        public bool HasArticle(string articleId)
        {
            return _likedArticles.Exists(item => item.Id == articleId);
        }
    }
}
